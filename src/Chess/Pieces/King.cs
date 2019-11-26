using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public King(Color color, int x, int y) : base(color, x, y)
        {
        }

        protected override IEnumerable<(int X, int Y)> GetPossibleMovements(Board board)
        {
            return AllMovements(board, 1);
        }

        public bool IsInCheck(Board board, out Piece[] checkingPieces)
        {
            checkingPieces = board.GetPiecesThatCouldMoveTo(this, true).ToArray();
            return checkingPieces.Length > 0;
        }

        public bool IsInCheckmate(Board board)
        {
            // king can not castle to get out of check
            if (!IsInCheck(board, out var checkingPieces))
                return false; // if not in check no movement is required to prevent checkmate

            foreach (var movement in GetPossibleMovements(board))
                if (!board.PreviewMove(this, movement.X, movement.Y).GetPiecesThatCouldMoveTo(Color.Invert(), movement.X, movement.Y, true).Any())
                    return false; // king could safely move here

            if (checkingPieces.Length > 1)
                return true; // king could not move away from check, there is no way to block or capture multiple checking pieces

            var checkingPiece = checkingPieces.First();

            if (board.GetPiecesThatCouldMoveTo(checkingPiece).Any())
                return false; // checking piece could be captured, thus removing the check

            if (checkingPiece is Knight)
                return true; // knights move cant be blocked

            if (checkingPiece is Pawn || checkingPiece is King)
                return false; // those should be handled by king itself

            // get empty fields between checking piece and king that could be blocked to remove check
            var travelOverFields = Enumerable.Empty<(int X, int Y)>();
            if (checkingPiece is Rook || checkingPiece is Queen && (checkingPiece.X == X || checkingPiece.Y == Y)) // only check queen if it would move like a rook
            {
                int count = Math.Max(Math.Abs(checkingPiece.X - X), Math.Abs(checkingPiece.Y - Y)) - 1;
                travelOverFields = checkingPiece.X == X
                    ? Enumerable.Range(Math.Min(checkingPiece.Y, Y) + 1, count).Select(y => (X, y))
                    : Enumerable.Range(Math.Min(checkingPiece.X, X) + 1, count).Select(x => (x, Y));
            }
            else if (checkingPiece is Bishop || checkingPiece is Queen)
            {
                int count = Math.Abs(checkingPiece.Y - Y) - 1;
                var xValues = Enumerable.Range(Math.Min(checkingPiece.X, X) + 1, count);
                var yValues = Enumerable.Range(Math.Min(checkingPiece.Y, Y) + 1, count);
                // get X and Y values in the right order
                if (checkingPiece.X > X)
                    xValues = xValues.Reverse();
                if (checkingPiece.Y > Y)
                    yValues = yValues.Reverse();

                travelOverFields = xValues.Zip(yValues, (x, y) => (x, y));
            }
            foreach (var tile in travelOverFields)
                if (board.GetPiecesThatCouldMoveTo(Color, tile.X, tile.Y).Any())
                    return false; // piece can move between king and checking piece

            return true; // we tried everything to remove check, but there is nothing we can do
        }
    }
}
