using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Color color, int x, int y) : base(color, x, y)
        {
        }

        protected override IEnumerable<(int X, int Y)> GetPossibleMovements(Board board)
        {
            int yMovement = Color switch
            {
                Color.White => +1,
                Color.Black => -1,
                _ => throw new NotImplementedException(),
            };

            IEnumerable<(int X, int Y)> getCaptureMoves()
            {
                yield return (X - 1, Y + yMovement);
                yield return (X + 1, Y + yMovement);
            }

            IEnumerable<(int X, int Y)> getRegularMoves()
            {
                yield return (X, Y + yMovement);
                if (!HasMoved)
                    yield return (X, Y + yMovement * 2);
            }

            Piece tempPiece;
            foreach (var movement in getCaptureMoves())
                if (IsInBounds(movement.X, movement.Y) && (tempPiece = board[movement.X, movement.Y]) != default && tempPiece.Color == Color.Invert())
                    yield return movement;
            foreach (var movement in getRegularMoves())
                if (IsInBounds(movement.X, movement.Y) && board[movement.X, movement.Y] == default)
                    yield return movement;
        }
    }
}
