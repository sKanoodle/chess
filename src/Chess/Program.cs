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
            // https://www.chessgames.com/perl/chessgame?gid=1060694
            var moveString = "d4.Nf6.c4.e6.Nf3.d5.g3.Bb4+.Bd2.Be7.Bg2.O-O.O-O.c6.Bf4.b6.Nc3.Ba6.cxd5.cxd5.Rc1.Nc6.Nxd5.Qxd5.Ne5.Nxd4.Bxd5.Nxe2+.Qxe2.Bxe2.Bxa8.Rxa8.Rfe1.Bb5.Rc2.Nd5.Rec1.Bc5.Bd2.f6.b4.Bf8.Ng4.Rd8.Rc8.Rd7.Nh6+.gxh6.Bxh6.Rf7.Rd8.Ne7.Rc7.Ng6.Rcc8.e5.f4.Bd7.Ra8.Bh3.Kf2.b5.Rdb8.exf4.gxf4.Bd7.h4.Bc6.h5.Bxa8.hxg6.hxg6.Rxa8.f5.Kg3.a6.Kh4.Rg7.Kg5.1-0";
            var moves = moveString.Split('.').ToArray();
            PlayWhatever(i => { Console.ReadKey(false); return moves[i - 1]; });
        }

        private static void PlayManually()
        {
            PlayWhatever(_ => Console.ReadLine());
        }

        private static void PlayWhatever(Func<int, string> getNextMove)
        {
            int turn = 1;
            while (!Board.CheckForEndOfGame())
            {
                RenderBoard();
                Console.WriteLine($"move {turn}:");
                var nextMove = getNextMove(turn);
                if (Board.TryPerformAlgebraicChessNotationMove(nextMove))
                    turn += 1;
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
            Console.WriteLine(Board.GetForsythEdwardsNotation() + "                                   ");
        }
    }
}
