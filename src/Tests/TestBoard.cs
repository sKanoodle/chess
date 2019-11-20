using Chess;
using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    class TestBoard : Board
    {
        public TestBoard() : base(null) { }

        public static TestBoard PopulatedDefault()
        {
            var result = new TestBoard();
            result.BuildStartingBoard();
            return result;
        }

        public void SetPiece(Piece piece)
        {
            this[piece.X, piece.Y] = piece;
        }
    }
}
