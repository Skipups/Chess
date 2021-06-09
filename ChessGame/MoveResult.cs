using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
   public class MoveResult
    {
        public Piece CapturedPiece;
        public Piece PawnToPromote;
        public bool GameIsOver;
        public Piece MovedPiece;
       
    }
}
