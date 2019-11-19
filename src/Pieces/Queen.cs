using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Pieces
{
    class Queen : Piece
    {
        public Queen(Color color, int x, int y) : base(color, x, y)
        {
        }

        protected override IEnumerable<(int X, int Y)> GetPossibleMovements(Board board)
        {
            return AllMovements(board);
        }
    }
}
