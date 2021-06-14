using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public interface IGameEngineUI
    {
        UserAction GetUserAction(Player player, GameState gameState);
        void SaveGame(GameState game);
        string LoadGame(GameState game);
    }
    public interface IGameUI
    {
        Tuple<Piece, Coord> GetMove(GameState gamestate, Player player);
        Piece GetPieceToPromote(GameState gamestate, Player player, List<Piece> capturedPieces);
        void DisplayInvalidMove(Player player, string message);
        void DisplayCapturedPiece(Piece piece);
        void DisplayMoveSuccessful(GameState game, Player player, Coord start, Coord end);
        void DisplayGameOver(GameState gamestate, Player player);
        void DrawCapturedList(Player player);
        void DisplayNotificationCheckOccured(Player currentPlayerTurn, Player playerInCheck);
        void DisplayReminderKingInCheck(Player currentPlayerTurn);
        void DisplayCoordsThatHaveKingInCheck(List<Coord> listOfCoordsTheHaveKingInCheck);
        bool AskUserAboutCastling(Player player);
        CastlePieces GetCastlingMove(List<CastlePieces> possibleCastlingOptions, GameState game);
    }
    public enum UserAction
    {
        SaveGame,
        LoadGame,
        NewGame,
        Quit,
        PlayTurn
    }
    public class GameEngine
    {
        private readonly IGameEngineUI _gameEngineUI;
        private readonly IGameUI _gameUI;

        public GameEngine(IGameEngineUI gameEngineUI, IGameUI gameUI)
        {
            _gameEngineUI = gameEngineUI;
            _gameUI = gameUI;
        }

        public void Run()
        {
            var game = GameState.StartNewGame("Svetlana", "Mike");
            UserAction userAction = this._gameEngineUI.GetUserAction(game.CurrentTurnPlayer, game);
            while(userAction != UserAction.Quit)
            {
                switch (userAction)
                {
                    case UserAction.PlayTurn:
                        if (game.IsCastlingPossibleAndExecuted(_gameUI))
                        {
                        }
                        else
                        {
                            game.PlayTurn(_gameUI);
                        }
                     
                        break;

                    case UserAction.SaveGame:
                        this._gameEngineUI.SaveGame(game);
                        break;

                    case UserAction.NewGame:
                        game = GameState.StartNewGame("Susan", "Jim");
                        break;

                    case UserAction.LoadGame:
                        var json =  this._gameEngineUI.LoadGame(game);
                        game = JsonConvert.DeserializeObject<GameState>(json, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                        });

                        game.Board.PopulatePieceToIdMap(game.PlayerWhite.CapturedList, game.PlayerBlack.CapturedList);

                        break;

                    default:
                        throw new InvalidOperationException();
                }

             userAction = this._gameEngineUI.GetUserAction(game.CurrentTurnPlayer, game);
            }
        }
    }
}
