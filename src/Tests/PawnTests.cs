using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Pawn
    {
        [TestMethod]
        public void TestBasicMovement()
        {
            var board = TestBoard.PopulatedDefault();
            Assert.IsTrue(board.TryMovePiece(board[2, 1], 2, 2), "pawn could not move 1 forward");
            Assert.IsFalse(board.TryMovePiece(board[2, 2], 2, 4), "pawn could move 2 forward, despite having already moved");
            Assert.IsTrue(board.TryMovePiece(board[3, 1], 3, 3), "pawn could not move 2 forward");
        }
    }
}
