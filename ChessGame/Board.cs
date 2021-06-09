using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public interface IBoard
    {
        bool IsValidMoveCoord(Coord coord, bool isStartCord, bool turnWhite);
        Piece GetPieceFromCoord(Coord coord);
        int GetPieceIdFromCoord(Coord coord);
        Coord GetStartCoordFromPieceId(int pieceId);
        List<CastleCoord> TryCastle(Player currentPlayer);
        List<Coord> PiecesThatHaveKingInCheck(bool playerColor);
        bool ExecuteCastlingMove(CastleCoord startCoord);
        bool CheckMate(Player player);
    }
    [DataContract()]
    public class Board : IBoard
    {
        [DataMember]
        private Dictionary<Coord, Piece> PieceCoordMap;
        [DataMember]
        public static Dictionary<Piece, int> PieceIdMap;
        public static Dictionary<int, Piece> IdPieceMap;
        private const string Horizontal = "+--------";
        private const string Vertical = "| ";
        private const int Size = 8;

        public Board()
        {
            PieceCoordMap = new Dictionary<Coord, Piece>();
            PieceIdMap = new Dictionary<Piece, int>();
            IdPieceMap = new Dictionary<int, Piece>();
        }

        //public void DrawBoard()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine("Board");
        //    Console.WriteLine("      0        1        2        3        4        5        6        7"); // column number
        //    for (int y = 0; y < Size; y++)
        //    {
        //        StringBuilder nextLine = new StringBuilder();
        //        nextLine.Append("  "); // move board left 2 spaces
        //        for (int x = 0; x < Size; x++)
        //        {
        //            nextLine.Append(Horizontal); // write horizontal part of the cell
        //        }
        //        //Console.Write("+\n");  // add + at the end of row
        //        nextLine.Append("+\n");
        //        for (int x = 0; x < Size; x++)
        //        {
        //            if (x == 0)
        //            {

        //                nextLine.Append(y + " "); //row number
        //            }

        //            char color = 'B';   // color of the celll
        //            if ((y + x) % 2 == 0)
        //            {
        //                color = 'W';
        //            }

        //            Piece piece;
        //            PieceCoordMap.TryGetValue(new Coord(x, y), out piece);

        //            string cellInfo = PrintCell(x, y, color, piece);
        //            nextLine.Append(Vertical + cellInfo + " ");
        //        }
        //        nextLine.Append("|");
        //        Console.WriteLine(nextLine);
        //    }
        //    // add bottom border
        //    StringBuilder border = new StringBuilder();
        //    border.Append("  ");

        //    for (int c = 0; c < Size; c++)
        //    {

        //        border.Append(Horizontal);
        //    }
        //    border.Append("+\n\n");
        //    Console.WriteLine(border);

        //}
        public static Board CreateNewGame()
        {
            var toReturn = new Board();
            toReturn.PopulatePieceCoordMap();
            toReturn.PopulatePieceToIdMaps();
            return toReturn;
        }

        private void PopulatePieceToIdMaps()
        {
            foreach (var kvp in PieceCoordMap)
            {
                Piece piece = kvp.Value;
                int id = piece.PieceId;
                PieceIdMap.Add(piece, id);
                IdPieceMap.Add(id, piece);
            }
        }
        public Coord GetStartCoordFromPieceId(int pieceId)
        {
            var piece = IdPieceMap[pieceId];
            Coord toReturn = new Coord(-1, -1);
            foreach (var kvp in PieceCoordMap)
            {
                if (kvp.Value == piece)
                {
                    toReturn = kvp.Key;
                }
            }

            return toReturn;
        }
        public int GetPieceIdFromCoord(Coord coord)
        {
            Piece piece = null;
            PieceCoordMap.TryGetValue(coord, out piece);
            return piece.PieceId;
        }

        public List<Coord> PiecesThatHaveKingInCheck(bool playerWhite)
        {
            //get king coord of opposing player
            var kingCoord = GetKingCoord(!playerWhite);
            var listPieceResult = new List<Coord>();

            //get current players pieces adn the coords
            foreach (var kvp in GetPiecesForColor(playerWhite))
            {
                try
                {
                    // if we get a moveResult, the piece on kvp.Key can move to the kingCoord so we will add it to the list
                    var moveResult = CalculateMoveResult(kvp.Key, kingCoord);

                    listPieceResult.Add(kvp.Key);
                }
                catch (InvalidMoveException e)
                {

                }

            }
            return listPieceResult;
        }
        private Coord GetKingCoord(bool isWhite)
        {
            foreach (var kvp in PieceCoordMap)
            {
                if (kvp.Value.IsPieceCheckable != null)
                {
                    if (kvp.Value.White == isWhite)
                    {
                        return kvp.Key;
                    }
                }
            }
            throw new InvalidOperationException("Couldn't find King");
        }

        private List<Coord> GetCoordsOfRooksThatHaveNotMoved(bool isWhite)
        {
            var unMovedRookCoords = new List<Coord>(2);
            foreach (var kvp in PieceCoordMap)
            {
                if (kvp.Value.IsPieceCastlable != null && !kvp.Value.HasBeenMoved)
                {
                    if (kvp.Value.White == isWhite)
                    {
                        unMovedRookCoords.Add(kvp.Key);
                    }
                }
            }
            return unMovedRookCoords;
        }
        public List<CastleCoord> TryCastle(Player currentPlayer)
        {
            var verifiedCastlingCoordPairs = new List<CastleCoord>(2);

            //if have piece that hasn't moved and checkable
            var kingCoord = GetKingCoord(currentPlayer.White);
            Piece king;
            PieceCoordMap.TryGetValue(kingCoord, out king);
            if (king.HasBeenMoved)
            {
                return verifiedCastlingCoordPairs;
            }
            //if we have pieces that haven't moved and are castlable
            var unMovedRookCoords = GetCoordsOfRooksThatHaveNotMoved(currentPlayer.White);
            if (unMovedRookCoords.Count == 0)
            {
                return verifiedCastlingCoordPairs;
            }

            //is the path between them clear
            foreach (var rookCoord in unMovedRookCoords)
            {
                if (IsPathClear(kingCoord, rookCoord))
                {
                    verifiedCastlingCoordPairs.Add(new CastleCoord(kingCoord, rookCoord));
                }
            }

            return verifiedCastlingCoordPairs;
        }

        private IEnumerable<KeyValuePair<Coord, Piece>> GetPiecesForColor(bool isWhite)
        {
            foreach (var kvp in PieceCoordMap)
            {
                if (kvp.Value.White == isWhite)
                {
                    yield return kvp;
                }
            }
            yield break;
        }
 
        public bool CheckMate(Player player)
        {
            IEnumerable<KeyValuePair<Coord, Piece>> kvp = GetPiecesForColor(player.White);
            foreach (var item in kvp)
            {
                var startCoord = item.Key;
                var piece = item.Value;

                var possibleEndCoords = piece.GetAllpossibleMoveCoordinates(startCoord);
                foreach (var endCoord in possibleEndCoords)
                {
                    try
                    {
                        var moveResult = CalculateMoveResult(startCoord, endCoord);
                        if (WouldBeInCheck(moveResult, startCoord, endCoord).Count == 0)
                        {
                            return false;
                        }
                    }
                    catch (InvalidMoveException e)
                    {

                    }

                }
            }
            return true;
        }

        protected internal MoveResult Move(Coord start, Coord end)
        {
            var moveResult = CalculateMoveResult(start, end);
            if (WouldBeInCheck(moveResult, start, end).Count > 0)
            {
                throw new InvalidMoveException("This move is invalid because it puts you in check");
            }
            if (moveResult.CapturedPiece != null)
            {
                moveResult.CapturedPiece.HasBeenMoved = true;
            }

            if (moveResult.MovedPiece != null)
            {
                moveResult.MovedPiece.HasBeenMoved = true;
            }

            //updateMap
            UpdateMap(start, end, moveResult.MovedPiece);

            return moveResult;
        }
        public MoveResult CalculateMoveResult(Coord start, Coord end)
        {
            var moveResult = new MoveResult();
            moveResult.MovedPiece = GetPieceFromCoord(start);
            var endPiece = GetPieceFromCoord(end);

            //validate start and end are not the same
            if (start.Equals(end))
            {
                throw new InvalidMoveException("Start and end coordinates must be different");
            }

            //is start piece their color?

            //ask start piece if it can move that many squares
            if (moveResult.MovedPiece != null)
            {
                if (!moveResult.MovedPiece.ValidateMove(start, end, endPiece))
                {
                    throw new InvalidMoveException("Starting piece can't move that many squares.");
                }
            }

            //not a valid move if moveResult.MovedPieceand endPice are are the same color
            if (endPiece != null && (moveResult.MovedPiece.White == endPiece.White))
            {
                throw new InvalidMoveException("Start and end pieces can't be the same color");
            }



            //does start piece care about pathCheck
            //yes- is the path clear?
            if (moveResult.MovedPiece.CheckForPiecesInPath)
            {
                if (!IsPathClear(start, end))
                {
                    throw new InvalidMoveException("The path is not clear to make this move.");
                }

            }


            IPromotablePiece startPiecePromotable = moveResult.MovedPiece.IsPiecePromotable;
            if (startPiecePromotable != null && startPiecePromotable.IsValidPromotionPosition(end))
            {
                //are we in the final opposite colors row
                moveResult.PawnToPromote = moveResult.MovedPiece;

            }


            moveResult.CapturedPiece = endPiece;
            return moveResult;
        }
        protected internal void Promote(Coord end, Piece pieceToPromote)
        {

            var start = end;

            //updateMap
            UpdateMap(start, end, pieceToPromote);
        }

        private void UpdateMap(Coord start, Coord end, Piece piece)
        {
            PieceCoordMap.Remove(start);
            PieceCoordMap.Remove(end);
            PieceCoordMap.Add(end, piece);
        }

        private void UndoUpdateMap(Coord start, Coord end, MoveResult moveResult)
        {
            PieceCoordMap.Remove(end);
            PieceCoordMap.Add(start, moveResult.MovedPiece);
            if (moveResult.CapturedPiece != null)
            {
                PieceCoordMap.Add(end, moveResult.CapturedPiece);
            }
        }
        private List<Coord> WouldBeInCheck(MoveResult moveResult, Coord start, Coord end)
        {

            UpdateMap(start, end, moveResult.MovedPiece);
            var list = PiecesThatHaveKingInCheck(moveResult.MovedPiece.White);

            UndoUpdateMap(start, end, moveResult);
            return list;
        }

        // Program calls to check if users input is a valid coord request
        public bool IsValidMoveCoord(Coord coord, bool isStartCord, bool turnWhite)
        {
            ///are coords within grid
            if (!IsWithInBoard(coord))
            {
                return false;
            }

            Piece piece = GetPieceFromCoord(coord);
            if (isStartCord && piece == null)
            {
                return false;
            }
            if (turnWhite && isStartCord && !piece.White) //whites turn, iStartCoord, piece present, piece color == current player color
            {
                return false;
            }

            if (!turnWhite && isStartCord && piece.White)
            {
                return false;
            }

            return true;
        }

        private bool IsWithInBoard(Coord coord)
        {
            return !(coord.X < 0 || coord.X > 7 || coord.Y < 0 || coord.Y > 7);

        }

        //doesn't check start or end coord
        private bool IsPathClear(Coord start, Coord end)
        {
            var pathList = Coord.CalculatePath(start, end);
            foreach (var coord in pathList)
            {
                //is there a piece along the path
                if (PieceCoordMap.ContainsKey(coord) && !coord.Equals(start) && !coord.Equals(end))
                {
                    return false;
                }
            }

            return true;
        }


        public Piece GetPieceFromCoord(Coord coordinate)
        {
            Piece piece;
            PieceCoordMap.TryGetValue(coordinate, out piece);
            return piece ?? null;
        }
        //Mike: how to specify king coord is first
        public bool ExecuteCastlingMove(CastleCoord startCoords)
        {
            //are we castling kingside or queenside
            var numXCoordsBetween = Math.Abs(startCoords.KingCoord.X - startCoords.RookCoord.X);
            var kingPiece = GetPieceFromCoord(startCoords.KingCoord);
            var rookPiece = GetPieceFromCoord(startCoords.RookCoord);
            CastleCoord endCoords = null;

            //kingside
            if (numXCoordsBetween == 3)
            {
                if (kingPiece.White)
                {
                    endCoords = new CastleCoord(new Coord(6, 7), new Coord(5, 7));
                }
                else
                {
                    endCoords = new CastleCoord(new Coord(6, 0), new Coord(5, 0));

                }

            }
            //queenside
            else if (numXCoordsBetween == 4)
            {
                if (kingPiece.White)
                {
                    endCoords = new CastleCoord(new Coord(2, 7), new Coord(3, 7));
                }
                else
                {
                    endCoords = new CastleCoord(new Coord(2, 0), new Coord(3, 0));
                }

            }

            var moveResult = new MoveResult();
            moveResult.MovedPiece = kingPiece;
            var checkListResult1 = this.WouldBeInCheck(moveResult, startCoords.KingCoord, endCoords.KingCoord);
            moveResult.MovedPiece = rookPiece;
            var checkListResult2 = this.WouldBeInCheck(moveResult, startCoords.RookCoord, endCoords.RookCoord);
            if (checkListResult1.Count > 0 || checkListResult2.Count > 0)
            {
                return false;
            }
            UpdateMap(startCoords.KingCoord, endCoords.KingCoord, kingPiece);
            UpdateMap(startCoords.RookCoord, endCoords.RookCoord, rookPiece);

            return true;
        }


        private void PopulatePieceCoordMap()
        {
            // 8 B P
            PieceCoordMap.Add(new Coord(0, 1), new Pawn(false, 0));
            PieceCoordMap.Add(new Coord(1, 1), new Pawn(false, 1));
            PieceCoordMap.Add(new Coord(2, 1), new Pawn(false, 2));
            PieceCoordMap.Add(new Coord(3, 1), new Pawn(false, 3));
            PieceCoordMap.Add(new Coord(4, 1), new Pawn(false, 4));
            PieceCoordMap.Add(new Coord(5, 1), new Pawn(false, 5));
            PieceCoordMap.Add(new Coord(6, 1), new Pawn(false, 6));
            PieceCoordMap.Add(new Coord(7, 1), new Pawn(false, 7));
            // 2 B R
            PieceCoordMap.Add(new Coord(0, 0), new Rook(false, 8));
            PieceCoordMap.Add(new Coord(7, 0), new Rook(false, 9));
            // 2 B k
            PieceCoordMap.Add(new Coord(1, 0), new Knight(false, 10));
            PieceCoordMap.Add(new Coord(6, 0), new Knight(false, 11));
            // 2 B B
            PieceCoordMap.Add(new Coord(2, 0), new Bishop(false, 12));
            PieceCoordMap.Add(new Coord(5, 0), new Bishop(false, 13));
            // 1 B K
            PieceCoordMap.Add(new Coord(4, 0), new King(false, 14));
            // 1 B Q
            PieceCoordMap.Add(new Coord(3, 0), new Queen(false, 15));

            // White Pieces
            // 8 W P
            PieceCoordMap.Add(new Coord(0, 6), new Pawn(true, 16));
            PieceCoordMap.Add(new Coord(1, 6), new Pawn(true, 17));
            PieceCoordMap.Add(new Coord(2, 6), new Pawn(true, 18));
            PieceCoordMap.Add(new Coord(3, 6), new Pawn(true, 19));
            PieceCoordMap.Add(new Coord(4, 6), new Pawn(true, 20));
            PieceCoordMap.Add(new Coord(5, 6), new Pawn(true, 21));
            PieceCoordMap.Add(new Coord(6, 6), new Pawn(true, 22));
            PieceCoordMap.Add(new Coord(7, 6), new Pawn(true, 23));
            // 2 W R
            PieceCoordMap.Add(new Coord(0, 7), new Rook(true, 24));
            PieceCoordMap.Add(new Coord(7, 7), new Rook(true, 25));
            // 2 W k           
            PieceCoordMap.Add(new Coord(1, 7), new Knight(true, 26));
            PieceCoordMap.Add(new Coord(6, 7), new Knight(true, 27));
            // 2 W B           
            PieceCoordMap.Add(new Coord(2, 7), new Bishop(true, 28));
            PieceCoordMap.Add(new Coord(5, 7), new Bishop(true, 29));
            // 1 W K           
            PieceCoordMap.Add(new Coord(4, 7), new King(true, 30));
            // 1 W Q           
            PieceCoordMap.Add(new Coord(3, 7), new Queen(true, 31));
        }

    }
}

