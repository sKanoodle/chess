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
            var cols = Enumerable.Range(0, 8).Select(i => (char)(i + 'a')).ToArray();
            var rows = Enumerable.Range(1, 8).ToArray();

            Console.SetCursorPosition(0, 0);
            for (int y = 8; y >= -1; y--)
            {
                if (y == 8 || y == -1)
                {
                    Console.WriteLine($" {new string(cols)} ");
                    continue;
                }

                for (int x = -1; x < 9; x++)
                {
                    if (x == -1 || x == 8)
                    {
                        Console.Write(rows[y]);
                        continue;
                    }

                    Console.Write(Board[x, y] switch
                    {
                        null => " ",
                        Pawn _ => "P",
                        Bishop _ => "B",
                        Knight _ => "N",
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
