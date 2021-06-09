using System;
using System.Collections.Generic;
using System.Text;

namespace ChessBoard
{
    class Board
    {
        public Cell[,] theBoard {get; set;}
        public char Color { get; }

        //Creating the 8 X 8 board, full of new cells with a x and y coordinate and color
        public Board()
        {
            Color = '-';
            theBoard = new Cell[8, 8];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    char color = 'W';
                    if (x%2==0 && y % 2 == 0)
                    {
                        color = 'B';
                    }
                    theBoard[x, y] = new Cell(x, y, color);
                }
            }
        }
    }
}
