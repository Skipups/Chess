using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    [DataContract()]
    public class Player
    {
        [DataMember]
        public List<Piece> CapturedList { get; set; }

        [DataMember]
        public string Name { get; }

        [DataMember]
        public bool White { get; }

        [DataMember]
        public bool IsInCheck { get; set; }

        [DataMember]
        public int PlayerId { get; }
        


        public Player(string name, bool white)
        {
            Name = name;
            White = white;
            CapturedList = new List<Piece>();
            IsInCheck = false;
            PlayerId = white == true ? 0 : 1;
           

        }
    }
}
