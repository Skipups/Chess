using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ChessGame;

namespace ChessGameWeb.Controllers
{
    public class PieceController : ApiController
    {
        private GameStateStore Store = new GameStateStore();

        [HttpGet]
        [Route("api/gamestate/{gameid:guid}/pieces/{pieceid:int}")]
        public GameState UpdatePiece(Guid gameId,
            int pieceId,
            [FromUri] int x,
            [FromUri] int y,
            [FromUri] string playerName)
        {
           var loadedGame = Store.Load(gameId);
           
            if(loadedGame != null)
            {
                if(loadedGame.CurrentTurnPlayer.Name == playerName)
                {
                    var coord = new Coord(x, y);
                    var piece = loadedGame.Board.GetPieceGivenId(pieceId);
                    if (piece != null)
                    {
                        var ui = new ChessWebUI();
                        ui.PieceToMove = piece;
                        ui.MoveDestination = coord;
                        loadedGame.PlayTurn(ui);
                        Store.Save(loadedGame);
                    }
                }
            }

            return loadedGame;
        }

        [HttpGet]
        [Route("api/gamestate/{gameid:guid}/piecesToPromote")]
        public GameState PromotePiece(Guid gameId,
         int pieceId,
         [FromUri] int x,
         [FromUri] int y,
         [FromUri] string playerName)
        {
            var loadedGame = Store.Load(gameId);

            if (loadedGame != null)
            {
                if (loadedGame.CurrentTurnPlayer.Name == playerName)
                {
                    var coord = new Coord(x, y);
                    var piece = loadedGame.Board.GetPieceGivenId(pieceId);
                    if (piece != null)
                    {
                        var ui = new ChessWebUI();
                        ui.PieceToMove = piece;
                        ui.MoveDestination = coord;
                        loadedGame.PlayTurn(ui);
                        Store.Save(loadedGame);
                    }
                }
            }

            return loadedGame;
        }

        [HttpGet]
        [Route("api/gamestate/{gameid:guid}/pieces")]
        public GameState DisplayBoard(Guid gameId)
        {
            var loadedGame = Store.Load(gameId);
            return loadedGame;
        }

        
    }
}