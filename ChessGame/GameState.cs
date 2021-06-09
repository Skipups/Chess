using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    [DataContract()]
    public class GameState
    {
        [DataMember]
        public Player PlayerWhite { get; private set; }
        [DataMember]
        public Player PlayerBlack { get; private set; }

        [IgnoreDataMember]
        public IBoard Board { get { return _board; } }
        [DataMember]
        public bool TurnWhite;

        [DataMember]
        private Board _board;

        [IgnoreDataMember]
        public Player CurrentTurnPlayer
        {
            get
            {
                if (TurnWhite)
                {
                    return PlayerWhite;
                }
                else
                {
                    return PlayerBlack;
                };
            }
            set
            {
                if (TurnWhite)
                {
                    TurnWhite = false;
                }
                else
                {
                    TurnWhite = true;

                };
            }

        }

        [IgnoreDataMember]
        public Player OtherPlayer
        {
            get
            {
                if (TurnWhite)
                {
                    return PlayerBlack;
                }
                else
                {
                    return PlayerWhite;
                };
            }
            set
            {
                if (TurnWhite)
                {
                    TurnWhite = true;
                }
                else
                {
                    TurnWhite = false;

                };
            }

        }

        [DataMember]
        public Guid gameID;

        public GameState(string player1Name, string player2Name, Board board)
        {
            PlayerWhite = new Player(player1Name, true);
            PlayerBlack = new Player(player2Name, false);
            _board = board;
            TurnWhite = true;
        }
        private GameState()
        {

        }
        public static GameState StartNewGame(string player1Name, string player2Name)
        {
            var toReturn = new GameState();

            toReturn.PlayerWhite = new Player(player1Name, true);
            toReturn.PlayerBlack = new Player(player2Name, false);
            toReturn._board = ChessGame.Board.CreateNewGame();
            toReturn.TurnWhite = true;
            toReturn.gameID = GenerateGuid();
            return toReturn;
        }

        public static Guid GenerateGuid()
        {
            return new Guid();
        }

        public void ShowPlayers()
        {
                Console.WriteLine($"Player1: {PlayerWhite.Name}, Color:{PlayerWhite.White} and Player2: {PlayerBlack.Name}, Color:{PlayerBlack.White}.");
        }
        public void ShowWhoseTurn()
        {
            if (TurnWhite)
            {
                Console.WriteLine($"{PlayerWhite.Name}'s Turn! Select the white piece you want to move");
            }
            else
            {
                Console.WriteLine($"{PlayerBlack.Name}'s Turn! Select the black piece you want to move");
            }
        }
        public MoveResult Move(int pieceId, Coord end)
        {
            var start = _board.GetStartCoordFromPieceId(pieceId);
            var moveResult = _board.Move(start, end);
           
            if (moveResult.CapturedPiece != null)
            {
            
                if (moveResult.CapturedPiece.White)
                {
                    PlayerBlack.CapturedList.Add(moveResult.CapturedPiece);
                }
                else
                {
                    PlayerWhite.CapturedList.Add(moveResult.CapturedPiece);
                }
            }
            return moveResult;

        }
        public void Promote(Coord end, Piece selectionFromList, MoveResult moveResult)
        {
          
            var listToUpdate = TurnWhite ? PlayerBlack.CapturedList : PlayerWhite.CapturedList;
            var pawnToPromote = moveResult.PawnToPromote;
         
                listToUpdate.Remove(selectionFromList);
                listToUpdate.Add(pawnToPromote);
         
   
            _board.Promote(end, selectionFromList);
          
        }
        public bool PlayTurn(IGameUI gameUI)
        {
        
            while (true)
            {
                //currentTurn player in check
                var listOfCoordsThatHaveKingInCheck = Board.PiecesThatHaveKingInCheck(!CurrentTurnPlayer.White);
                if (listOfCoordsThatHaveKingInCheck.Count > 0)
                {
                    //check for checkMate
                    if (Board.CheckMate(CurrentTurnPlayer))
                    {
                        //gameover in checkmate
                        gameUI.DisplayGameOver(this, CurrentTurnPlayer);
                        return false;
                    }
                    //display reminder current player is in check
                    gameUI.DisplayReminderKingInCheck(CurrentTurnPlayer);
                    gameUI.DisplayCoordsThatHaveKingInCheck(listOfCoordsThatHaveKingInCheck);
                }
                    //ask user for move
                    var (coordStart, coordEnd) = gameUI.GetMove(this, CurrentTurnPlayer);

                try
                {
                    var pieceId = _board.GetPieceIdFromCoord(coordStart);
                    var moveResult = this.Move(pieceId, coordEnd);
                    //check for capture
                    if (moveResult.CapturedPiece != null)
                    {
                        gameUI.DisplayCapturedPiece(moveResult.CapturedPiece);
                    }

                    //check for promotion opportunity
                    if (moveResult.PawnToPromote != null)
                    {
                        var listToUpdate = CurrentTurnPlayer.White ? PlayerBlack.CapturedList : PlayerWhite.CapturedList;

                        var pieceToPromote = gameUI.GetPieceToPromote(this, CurrentTurnPlayer, listToUpdate);
                        this.Promote(coordEnd, pieceToPromote, moveResult);
                    }

                        //calculate if the current player has checked the other player
                        listOfCoordsThatHaveKingInCheck = Board.PiecesThatHaveKingInCheck(CurrentTurnPlayer.White);
                         if (listOfCoordsThatHaveKingInCheck.Count > 0)
                         {
                             gameUI.DisplayNotificationCheckOccured(CurrentTurnPlayer, OtherPlayer);
                             gameUI.DisplayCoordsThatHaveKingInCheck(listOfCoordsThatHaveKingInCheck);
                         }

                    //display move was successful
                    gameUI.DisplayMoveSuccessful(this, CurrentTurnPlayer, coordStart, coordEnd);
                    this.TurnWhite = !this.TurnWhite;
                    return true;
                }
                catch (InvalidMoveException exceptionMessage)
                {
                    gameUI.DisplayInvalidMove(CurrentTurnPlayer, exceptionMessage.Message);
                   
                }
            }
        }

        public bool IsCastlingPossibleAndExecuted(IGameUI gameUI)
        {
            //currentTurn player in check
            var listOfCoordsThatHaveKingInCheck = Board.PiecesThatHaveKingInCheck(!CurrentTurnPlayer.White);
            if (listOfCoordsThatHaveKingInCheck.Count > 0)
            {
                return false;
            }
            var possibleCastlingOptions = Board.TryCastle(CurrentTurnPlayer);
            if (possibleCastlingOptions.Count > 0)
            {
                var coordSelection = gameUI.GetCastlingMove(possibleCastlingOptions);
                if (coordSelection != null)
                {
                    return Board.ExecuteCastlingMove(coordSelection);
                }
            }

            return false;
        }
    }
}
