using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ChessGame;

namespace ChessGameWeb.Controllers
{
    public class GameStateController : ApiController
    {
        private GameStateStore Store = new GameStateStore();

        [HttpGet]
        [Route("api/gamestate/new")]
        public GameState CreateNew([FromUri]string player1, [FromUri] string player2)
        {
            var toReturn = GameState.StartNewGame(player1, player2);
            Store.Save(toReturn);
            return toReturn;
        }

        [HttpGet]
        [Route("api/gamestate/{gameid:guid}")]
        public GameState Get(Guid gameId)
        {
            var toReturn = Store.Load(gameId);
            return toReturn;
        }

    }

}