using Chess.Pieces;
using System;
using System.Linq;

namespace Chess
{
    class Program
    {
        private static Board Board;

        static void Main(string[] args)
        {
            Board = new Board();
            var colorTurn = Color.White;

            while (true)
            {
                RenderBoard();
                var nextMove = Console.ReadLine();
                if (Board.TryPerformAlgebraicChessNotationMove(colorTurn, nextMove))
                    colorTurn = colorTurn.Invert();
            }

        }

        private static void RenderBoard()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    Console.Write(Board[x, y] switch
                    {
                        null => " ",
                        Pawn _ => "P",
                        Bishop _ => "B",
                        Knight _ => "K",
                        Rook _ => "R",
                        Queen _ => "Q",
                        King _ => "K",
                        _ => throw new NotImplementedException(),
                    });
                }
                Console.WriteLine();
            }
        }
    }
}
