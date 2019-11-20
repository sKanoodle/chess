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

        public bool IsInCheck(Board board)
        {
            return board.CouldPieceCaptureAt(Color.Invert(), X, Y);
        }

        public bool IsInCheckmate(Board board)
        {
            // TODO: it would also be possible to capture pieces or block their way to get out of check and prevent checkmate
            // king can not castle to get out of check
            if (!IsInCheck(board))
                return false; // if not in check no movement is required to prevent checkmate
            foreach (var movement in GetPossibleMovements(board))
                if (!board.CouldPieceCaptureAt(Color.Invert(), movement.X, movement.Y))
                    return false; // king could safely move here
            return true;
        }
    }
}
