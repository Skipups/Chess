using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string exceptionPhrase): base( String.Format("Invalid move: {0}", exceptionPhrase))
        {

        }
    }

    public class InvalidSelectionException : Exception
    {
        public InvalidSelectionException(string exceptionPhrase) : base(String.Format("Invalid Selection: {0}", exceptionPhrase))
        {

        }
    }
}
