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


        public Player(string name, bool white)
        {
            Name = name;
            White = white;
            CapturedList = new List<Piece>();
            IsInCheck = false;
           

        }
     

        //drawListcaptured list without pawns

       
    }
}

//public void DrawCapturedList()
//{
//    if (this.CapturedList.Count == 0) return;

//    StringBuilder line = new StringBuilder();
//    line.Append("    "); // 
//    line.Append(($"{ this.Name}'s captured pieces: "));
//    string commaSeparated = string.Join(",", this.CapturedList.Keys.Select(key => key.DisplayPieceInfo));
//    line.Append(commaSeparated);
//    Console.Write(line);
//    Console.WriteLine();
//}
