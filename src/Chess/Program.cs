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
            PlayVsEngine();
        }

        private static void ReplayGame()
        {
            // https://www.chessgames.com/perl/chessgame?gid=1060694
            var moveString = "d4.Nf6.c4.e6.Nc3.Bb4.e3.d5.a3.Bxc3+.bxc3.c5.cxd5.exd5.Bd3.O-O.Ne2.b6.O-O.Ba6.Bxa6.Nxa6.Bb2.Qd7.a4.Rfe8.Qd3.c4.Qc2.Nb8.Rae1.Nc6.Ng3.Na5.f3.Nb3.e4.Qxa4.e5.Nd7.Qf2.g6.f4.f5.exf6.Nxf6.f5.Rxe1.Rxe1.Re8.Re6.Rxe6.fxe6.Kg7.Qf4.Qe8.Qe5.Qe7.Ba3.Qxa3.Nh5+.gxh5.Qg5+.Kf8.Qxf6+.Kg8.e7.Qc1+.Kf2.Qc2+.Kg3.Qd3+.Kh4.Qe4+.Kxh5.Qe2+.Kh4.Qe4+.g4.Qe1+.Kh5.1-0";
            var moves = moveString.Split('.').ToArray();
            PlayWhatever(i => { Console.ReadKey(false); return moves[i - 1]; });
        }

        private static void PlayManually()
        {
            PlayWhatever(_ => Console.ReadLine());
        }

        private const string StockfishPath = @"F:\stockfish_10_x64.exe";
        private static void PlayVsEngine()
        {
            var engineColor = Color.Black;
            IEngine engine = new StockfishEngine(StockfishPath);

            PlayWhatever(_ =>
            {
                if (Board.ColorToMoveNext == engineColor)
                    return engine.GetMove(Board.GetForsythEdwardsNotation());
                else
                    return Console.ReadLine();
            });

        }

        private static void PlayWhatever(Func<int, string> getNextMove)
        {
            int turn = 1;
            while (!Board.CheckForEndOfGame())
            {
                RenderBoard();
                Console.WriteLine($"move {turn}:");
                var nextMove = getNextMove(turn);
                if (Board.TryPerformCoordinateNotationMove(nextMove) || Board.TryPerformAlgebraicChessNotationMove(nextMove))
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
