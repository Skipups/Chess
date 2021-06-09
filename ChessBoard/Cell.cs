using System;

namespace ChessBoard
{
    public class Cell
    {
        public int RowNum { get; set; }
        public int ColNum { get; set; }
        public bool IsOccupied { get; set; }

        public string Square { get; set; }

        private readonly IPiece Piece;

        public Cell(int x, int y, char color)
        {
            RowNum = x;
            ColNum = y;
            IsOccupied = false;
            Square = $"{color}(--)";
        }
        public Cell(int x, int y, string color, IPiece piece)
        {
            RowNum = x;
            ColNum = y;
            IsOccupied = false;
            Square = $"{color}({piece})";
            Piece = piece;
        }
    }
}
