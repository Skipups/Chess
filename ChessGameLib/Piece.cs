using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public interface IPromotablePiece
    {
        bool IsValidPromotionPosition(Coord coordinate);
    }

    public interface ICheckable
    {
       
    }

    public interface ICastlable
    {
        
    }

    [DataContract()]
    abstract public class Piece
    {
        [DataMember]
        public bool White { get; private set; }

        [DataMember]
        public char FirstLetter { get; }

        [DataMember]
        public bool HasBeenMoved { get; set; }

        [DataMember]
        public int PieceId { get; private set; }

        [IgnoreDataMember]
        public string DisplayPieceInfo
        {
            get
            {
                if (this.White)
                {
                    return $"White {this.GetType().FullName.Substring(this.GetType().FullName.LastIndexOf('.') + 1)}";
                }
                else
                {
                    return $"Black {this.GetType().FullName.Substring(this.GetType().FullName.LastIndexOf('.') + 1)}";
                }
            }
        }
        // All pieces, except knight, need to know if their are other pieces in their path
        [IgnoreDataMember]
        public virtual bool CheckForPiecesInPath { get { return true; } }
        [IgnoreDataMember]
        public virtual IPromotablePiece IsPiecePromotable  { get { return null; } }
        [IgnoreDataMember]
        public virtual ICheckable IsPieceCheckable { get { return null; } }
        [IgnoreDataMember]
        public virtual ICastlable IsPieceCastlable { get { return null; } }
        abstract public List<Coord> GetAllpossibleMoveCoordinates(Coord start);

        public Piece(bool white, char firstLetter, int pieceId)
        {
            White = white;
            FirstLetter = firstLetter;
            HasBeenMoved = false;
            PieceId = pieceId;
        }
        abstract public bool ValidateMove(Coord start, Coord end, Piece piece=null);
    }
    [DataContract()]
    class Pawn : Piece, IPromotablePiece
    {
        private int moveDirectionMultiplier;
        private int startingY;

        public Pawn(bool color, int pieceId) : base(color,'P', pieceId)
        {
            if (color)
            {
                this.moveDirectionMultiplier = -1;
                this.startingY = 6;
            }
            else
            {
                this.moveDirectionMultiplier = 1;
                this.startingY = 1;
            }
        }

        public bool IsValidPromotionPosition(Coord coord)
        {
            return (this.White && coord.Y == 0) || (!this.White && coord.Y == 7);
           
        }

        public override IPromotablePiece IsPiecePromotable { get { return this; } }
       
        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
            if (piece != null)
            {
                // can only move diagonally one square
                return piece.White != this.White // has to be opposing piece at destination
                    && Math.Abs(start.X  -  end.X) == 1 // exactly one square in X direction either way
                    && (end.Y - start.Y) == this.moveDirectionMultiplier; // exactly one square in the correct Y direction
            }
            else
            {
                int maxMoveDistance = 1;
                // are we at our starting position?  If so, it's legal to move two squares
                if (start.Y == this.startingY)
                {
                    maxMoveDistance = 2;
                }

                int maxY = maxMoveDistance * this.moveDirectionMultiplier;
                int minY = 1 * this.moveDirectionMultiplier;
                if (maxY < minY)
                {
                    minY = maxY;
                    maxY= 1 * this.moveDirectionMultiplier;
                }

                return start.X == end.X // can't move to the side
                    && ((end.Y - start.Y) >= minY) // have to move at least 1 square
                    && ((end.Y - start.Y) <= maxY); // can move at most 1 or 2 squares depending on if we're at start
            }
        }

        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var pawnPossibleMoveCoordList = new List<Coord>();

            for (int i = start.X - 2 >= 0 ? start.X - 2 : 0; i <= (start.X + 2 <= 7 ? start.X + 2 : 7); i++)
            {
                for (int j = start.Y - 2 >= 0 ? start.Y - 2 : 0; j <= (start.Y + 2 <= 7 ? start.Y + 2 : 7); j++)
                {
                    if (i != start.X && j != start.Y)
                    {
                        var possibleEndCoord = new Coord(i, j);

                        pawnPossibleMoveCoordList.Add(possibleEndCoord);
                    }

                }
            }
            return pawnPossibleMoveCoordList;

        }
    }
    [DataContract()]
    class Knight : Piece
    {

        public Knight(bool color, int pieceId) : base(color,'k', pieceId)
        { 
        }
        public override bool CheckForPiecesInPath { get { return false; } }

        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var knightPossibleMoveCoordList = new List<Coord>();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    var possibleEndCoord = new Coord(i, j);
                    if (ValidateMove(start, possibleEndCoord))
                    {
                        knightPossibleMoveCoordList.Add(new Coord(i, j));
                    }
                }
            }
            return knightPossibleMoveCoordList;

        }

        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
            //vertically 1 sqaure && hortizontally 2 squares  or vertically 2 sqaures && hortizontally 1 square
            return ((Math.Abs(start.X - end.X) == 1 && Math.Abs(start.Y - end.Y) == 2) || (Math.Abs(start.X-end.X) == 2 && Math.Abs(start.Y-end.Y) ==1));
        }
    }
    [DataContract()]
    class King : Piece, ICheckable
        {
            public King(bool color, int pieceId) : base(color,'K', pieceId)
        {
            }
        public override ICheckable IsPieceCheckable { get { return this; } }

        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var kingPossibleMoveCoordList = new List<Coord>();
            
            for (int i = start.X-1 >=0 ? start.X - 1 : 0; i <= (start.X+1 <=7 ? start.X + 1 : 7); i++)
            {
                for (int j = start.Y - 1 >= 0 ? start.Y - 1 : 0; j <= (start.Y + 1 <= 7 ? start.Y + 1 : 7); j++)
                {
                    if(i != start.X && j!= start.Y)
                    {
                        var possibleEndCoord = new Coord(i, j);

                        kingPossibleMoveCoordList.Add(possibleEndCoord);
                    }
                    
                }
            }
            return kingPossibleMoveCoordList;


        }
        // horizontal, diagonal, and vertical move drection, but exactly one square

        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
            return ((start.X.Equals(end.X) && Math.Abs(start.Y - end.Y) == 1) ||
                    (start.Y.Equals(end.Y) && (Math.Abs(start.X - end.X) == 1) ||
                    ((Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y)) && Math.Abs(start.X - end.X) == 1)));   
        }
    }
    [DataContract()]
    class Queen : Piece
        {
           public Queen(bool color, int pieceId) : base(color,'Q', pieceId)
        {
            }
        // horizontal, diagonal, and vertical moves
        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
          return  (start.X.Equals(end.X) || start.Y.Equals(end.Y)) || (Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y));
        }

        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var queenPossibleMoveCoordList = new List<Coord>();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    var possibleEndCoord = new Coord(i, j);
                    if (ValidateMove(start, possibleEndCoord))
                    {
                        queenPossibleMoveCoordList.Add(new Coord(i, j));
                    }
                }
            }
            return queenPossibleMoveCoordList;
            

        }
    }
    [DataContract()]
    class Bishop : Piece
     {
            public Bishop(bool color, int pieceId) : base(color,'B', pieceId)
        {
            }
        //diagonal moves
        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
          return  Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y);
        }
        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var bishopPossibleMoveCoordList = new List<Coord>();
          for(int i =0; i<=7; i++)
            {
                for(int j=0; j<=7; j++)
                {
                    var possibleEndCoord = new Coord(i, j);
                    if (ValidateMove(start, possibleEndCoord))
                    {
                        bishopPossibleMoveCoordList.Add(new Coord(i, j));
                    }
                }
            }
            return bishopPossibleMoveCoordList;


        }
    }

    [DataContract()]
    class Rook : Piece, ICastlable
        {
            public Rook(bool color, int pieceId) : base(color, 'R', pieceId)
        {
            }
        public  override ICastlable IsPieceCastlable { get { return this; } }
        //horizontal and vertical moves
        public override bool ValidateMove(Coord start, Coord end, Piece piece = null)
        {
            return start.X.Equals(end.X) || start.Y.Equals(end.Y);
        }
        public override List<Coord> GetAllpossibleMoveCoordinates(Coord start)
        {
            var rookPossibleMoveCoordList= new List<Coord>();
            int counter = 0;
            //create all coords in current row
            while(counter <= 7)
            {
                if(counter != start.Y){
                    rookPossibleMoveCoordList.Add(new Coord(start.X, counter));
                }
                counter++;
            }
            counter = 0;
            //create all coords in current column
            while (counter <= 7)
            {
                if (counter != start.X)
                {
                    rookPossibleMoveCoordList.Add(new Coord(counter, start.Y));
                }
                counter++;
            }
            return rookPossibleMoveCoordList;

        }
    }
    
}

