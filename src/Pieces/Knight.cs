using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Pieces
{
    class Knight : Piece
    {
        public Knight(Color color, int x, int y) : base(color, x, y)
        {
        }

        protected override IEnumerable<(int X, int Y)> GetPossibleMovements(Board board)
        {
            IEnumerable<(int X, int Y)> getDirectPositions()
            {
                yield return (X + 2, Y + 1);
                yield return (X + 2, Y - 1);
                yield return (X - 2, Y - 1);
                yield return (X - 2, Y + 1);
                yield return (X + 1, Y + 2);
                yield return (X + 1, Y - 2);
                yield return (X - 1, Y - 2);
                yield return (X - 1, Y + 2);
            }

            Piece tempPiece;
            foreach (var position in getDirectPositions())
                if (IsInBounds(position.X, position.Y) && ((tempPiece = board[position.X, position.Y]) == default || tempPiece.Color == Color.Invert()))
                    yield return position;
        }
    }
}
