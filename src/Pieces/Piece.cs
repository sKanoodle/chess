using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Pieces
{
    abstract class Piece
    {
        // bottom left field is 0:0
        public int X { get; private set; } // col
        public int Y { get; private set; } // row
        public Color Color { get; }
        protected bool HasMoved { get; private set; } = false;

        public string Rank => (Y + 1).ToString();
        public string File => X switch
        {
            0 => "a",
            1 => "b",
            2 => "c",
            3 => "d",
            4 => "e",
            5 => "f",
            6 => "g",
            7 => "h",
        };
        public string Position => File + Rank;

        public Piece(Color color, int x, int y)
        {
            Color = color;
            X = x;
            Y = y;
        }

        protected abstract IEnumerable<(int X, int Y)> GetPossibleMovements(Board board);

        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
            HasMoved = true;
        }

        public bool CanMoveTo(int x, int y, Board board)
        {
            var movements = GetPossibleMovements(board).ToArray();

            foreach (var movement in GetPossibleMovements(board))
                if (movement.X == x && movement.Y == y)
                    return true;
            return false;
        }

        protected IEnumerable<(int X, int Y)> AllMovements(Board board, int maxMultiplier = -1)
        {
            return DiagonalMovements(board, maxMultiplier)
                .Concat(StraightMovements(board, maxMultiplier));
        }

        protected IEnumerable<(int X, int Y)> DiagonalMovements(Board board, int maxMultiplier = -1)
        {
            return MultiplesMovements(1, 1, board, maxMultiplier)
                .Concat(MultiplesMovements(1, -1, board, maxMultiplier))
                .Concat(MultiplesMovements(-1, 1, board, maxMultiplier))
                .Concat(MultiplesMovements(-1, -1, board, maxMultiplier));
        }

        protected IEnumerable<(int X, int Y)> StraightMovements(Board board, int maxMultiplier = -1)
        {
            return MultiplesMovements(1, 0, board, maxMultiplier)
                .Concat(MultiplesMovements(-1, 0, board, maxMultiplier))
                .Concat(MultiplesMovements(0, 1, board, maxMultiplier))
                .Concat(MultiplesMovements(0, -1, board, maxMultiplier));
        }

        protected IEnumerable<(int X, int Y)> MultiplesMovements(int deltaX, int deltaY, Board board, int maxMultiplier = -1)
        {
            int multiplier = 1;
            int newX, newY;
            while ((maxMultiplier < 0 || multiplier <= maxMultiplier) && IsInBounds(newX = X + deltaX * multiplier, newY = Y + deltaY * multiplier))
            {
                if (board[newX, newY] == default)
                    yield return (newX, newY);
                else
                {
                    if (board[newX, newY].Color != Color)
                        yield return (newX, newY);
                    break;
                }

                multiplier += 1;
            }
        }

        protected bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
    }
}
