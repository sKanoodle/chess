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
            ReplayGame();
        }

        private static void ReplayGame()
        {
            var moves = new[]
            {
                "e4",
                "d5",
                "exd5",
                "Qxd5",
                "Nc3",
                "Qa5",
                "d4",
                "Nf6",
                "Nf3",
                "c6",
                "Bc4",
                "Bf5",
                "Ne5",
                "e6",
                "g4",
                "Bg6",
                "h4",
                "Nbd7",
                "Nxd7",
                "Nxd7",
                "h5",
                "Be4",
                "Rh3",
                "Bg2",
                "Re3",
                "Nb6",
                "Bd3",
                "Nd5",
                "f3",
                "Bb4",
                "Kf2",
                "Bxc3",
                "bxc3",
                "Qxc3",
                "Rb1",
                "Qxd4",
                "Rxb7",
                "Rd8",
                "h6",
                "gxh6",
                "Bg6",
                "Ne7",
                "Qxd4",
                "Rxd4",
                "Rd3",
                "Rd8",
                "Rxd8+",
                "Kxd8",
                "Bd3",
                "1-0",
            };
            PlayWhatever(i => { Console.ReadKey(false); return moves[i - 1]; });
        }

        private static void PlayManually()
        {
            PlayWhatever(_ => Console.ReadLine());
        }

        private static void PlayWhatever(Func<int, string> getNextMove)
        {
            var colorTurn = Color.White;
            int turn = 1;

            while (!Board.CheckForEndOfGame())
            {
                RenderBoard();
                var nextMove = getNextMove(turn);
                if (Board.TryPerformAlgebraicChessNotationMove(colorTurn, nextMove))
                {
                    colorTurn = colorTurn.Invert();
                    turn += 1;
                }
            }
            RenderBoard();
            Console.WriteLine($"{Board.Winner} won!");
            Console.ReadKey(false);
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
                    if (x == 8)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    if (x == -1 || x == 8)
                    {
                        Console.Write(rows[y]);
                        continue;
                    }

                    if (Board.LastMoveOrigin == (x, y) || Board.LastMoveDestination == (x, y))
                        Console.BackgroundColor = ConsoleColor.Red;
                    else
                        Console.BackgroundColor = (x % 2 == 0) ^ (y % 2 == 0) ? ConsoleColor.Green : ConsoleColor.DarkGreen;
                    Console.ForegroundColor = Board[x, y]?.Color == Color.Black ? ConsoleColor.Black : ConsoleColor.Gray;
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
