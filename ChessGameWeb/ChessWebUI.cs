using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessGame;

namespace ChessGameWeb
{
    public class ChessWebUI : IGameUI
    {
        public Coord MoveDestination;
        public Piece PieceToMove;

        public bool AskUserAboutCastling(Player player)
        {
            return false;
        }

        public void DisplayCapturedPiece(Piece piece)
        {
        }

        public void DisplayCoordsThatHaveKingInCheck(List<Coord> listOfCoordsTheHaveKingInCheck)
        {
        }

        public void DisplayGameOver(GameState gamestate, Player player)
        {
        }

        public void DisplayInvalidMove(Player player, string message)
        {
            throw new ChessWebUIException(400, message);
        }

        public void DisplayMoveSuccessful(GameState game, Player player, Coord start, Coord end)
        {
        }

        public void DisplayNotificationCheckOccured(Player currentPlayerTurn, Player playerInCheck)
        {
        }

        public void DisplayReminderKingInCheck(Player currentPlayerTurn)
        {
        }

        public void DrawCapturedList(Player player)
        {
        }

        public CastlePieces GetCastlingMove(List<CastlePieces> possibleCastlingOptions, GameState game)
        {
            throw new ChessWebUIException(400, "Need Castle Param");
        }

        public Tuple<Piece, Coord> GetMove(GameState gamestate, Player player)
        {
            return Tuple.Create(PieceToMove, MoveDestination);
        }

        public Piece GetPieceToPromote(GameState gamestate, Player player, List<Piece> capturedPieces)
        {
            throw new ChessWebUIException(400, "Need Piece to Promote");
        }
    }

    public class ChessWebUIException : Exception
    {
        public int HttpCode;
        public ChessWebUIException(int httpCode, string message) :base (message)
        {
            HttpCode = httpCode;
        }
    }
}