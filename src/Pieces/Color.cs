using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Pieces
{
    enum Color
    {
        Black,
        White,
    }

    static class ColorExtensions
    {
        public static Color Invert(this Color color)
        {
            return color switch
            {
                Color.Black => Color.White,
                Color.White => Color.Black,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
