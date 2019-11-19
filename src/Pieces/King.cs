using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Pieces
{
    class King : Piece
    {
        public King(Color color, int x, int y) : base(color, x, y)
        {
        }

        protected override IEnumerable<(int X, int Y)> GetPossibleMovements(Board board)
        {
            return AllMovements(board, 1);
        }
    }
}
