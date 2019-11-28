using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    static class ConversionHelpers
    {
        public static string XToFile(int x) => ((char)(x + 'a')).ToString();
        public static int FileToX(char file)
        {
            if (file < 'a' || file > 'h')
                throw new ArgumentException();
            return file - 'a';
        }
        public static int NotationToX(string notation) => FileToX(notation[0]);

        public static string YToRank(int y) => (y + 1).ToString();
        public static int RankToY(char rank)
        {
            if (rank < '1' || rank > '8')
                throw new ArgumentException();
            return rank - '1';
        }
        public static int NotationToY(string notation) => RankToY(notation[^1]);

        public static string PositionToNotation(int x, int y) => XToFile(x) + YToRank(y);
        public static string PositionToNotation((int X, int Y) position) => PositionToNotation(position.X, position.Y);

        public static (int X, int Y) NotationToPosition(string notation) => (NotationToX(notation), NotationToY(notation));
    }
}
