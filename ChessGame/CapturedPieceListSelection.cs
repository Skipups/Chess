using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public struct CapturedPieceListSelection : IEquatable<CapturedPieceListSelection>
    {
        
       
            public int Num;
          
            public CapturedPieceListSelection(int num)
            {
                this.Num = num;
              
            }

            public static CapturedPieceListSelection InvalidSelection = new CapturedPieceListSelection(-1);

            public bool Equals(CapturedPieceListSelection other)
            {
            throw new NotImplementedException();
            }


            public override int GetHashCode()
            {
            throw new NotImplementedException();
            }

          

        
    }
}
