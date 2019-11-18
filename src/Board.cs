using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Board
    {
        private readonly Piece[] _Pieces = new Piece[64];
        public IEnumerable<Piece> Pieces => _Pieces.Where(p => p != default);
        private IEnumerable<Piece> PiecesByColor(Color color) => Pieces.Where(p => p.Color == color);
        public Piece this[int x, int y]
        {
            get => _Pieces[y * 8 + x];
            set => _Pieces[y * 8 + x] = value;
        }

        public Piece this[string position]
        {
            get => this[GetXFromStringPosition(position), GetYFromStringPosition(position)];
            set => this[GetXFromStringPosition(position), GetYFromStringPosition(position)] = value;
        }

        public Board()
        {
            BuildPawnLine(1, Color.White);
            BuildOtherLine(0, Color.White);

            BuildPawnLine(6, Color.Black);
            BuildOtherLine(7, Color.Black);
        }

        private void BuildPawnLine(int y, Color color)
        {
            for (int x = 0; x < 8; x++)
                this[x, y] = new Pawn(color, x, y);
        }

        private void BuildOtherLine(int y, Color color)
        {
            foreach (int x in new[] { 0, 7 })
                this[x, y] = new Rook(color, x, y);
            foreach (int x in new[] { 1, 6 })
                this[x, y] = new Knight(color, x, y);
            foreach (int x in new[] { 2, 5 })
                this[x, y] = new Bishop(color, x, y);
            this[3, y] = new Queen(color, 3, y);
            this[4, y] = new King(color, 4, y);
        }

        private int GetXFromStringPosition(string position)
        {
            var c = position[0];
            if (c < 'a' || c > 'h')
                throw new ArgumentException();
            return c - 'a';
        }

        private int GetYFromStringPosition(string position)
        {
            var c = position[1];
            if (c < '1' || c > '8')
                throw new ArgumentException();
            return c - '1';
        }

        public bool TryMovePiece(Piece piece, int x, int y)
        {
            if (!piece.CanMoveTo(x, y, this))
                return false;
            if (piece is King king && CouldPieceCaptureAt(king.Color.Invert(), x, y)) // TODO: missing castling
                return false;
            if (this[x, y] != default)
                CapturePiece(this[x, y]);
            MovePiece(piece, x, y);
            return true;
        }

        private void CapturePiece(Piece piece)
        {

        }

        private void MovePiece(Piece piece, int x, int y)
        {
            this[piece.X, piece.Y] = default;
            this[x, y] = piece;
            piece.MoveTo(x, y);
        }

        private bool CouldPieceCaptureAt(Color color, int x, int y)
        {
            return PiecesByColor(color).Any(p => p.CanMoveTo(x, y, this));
        }

        public bool TryPerformAlgebraicChessNotationMove(Color color, string notation)
        {
            try
            {
                PerformAlgebraicChessNotationMove(color, notation);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void PerformAlgebraicChessNotationMove(Color color, string notation)
        {
            var pieces = PiecesByColor(color);
            pieces = notation[0] switch
            {
                'K' => pieces.OfType<King>(),
                'Q' => pieces.OfType<Queen>(),
                'R' => pieces.OfType<Rook>(),
                'B' => pieces.OfType<Bishop>(),
                'N' => pieces.OfType<Knight>(),
                _ => pieces.OfType<Pawn>(),
            };
            pieces = pieces.ToArray();

            var offset = char.IsUpper(notation[0]) ? 1 : 0;
            var destination = notation.Substring(offset, 2);
            var x = GetXFromStringPosition(destination);
            var y = GetYFromStringPosition(destination);

            var piece = pieces.FirstOrDefault(p => p.CanMoveTo(x, y, this));
            if (piece == default || !TryMovePiece(piece, x, y))
                throw new ArgumentException("no piece can move to this position");
        }
    }
}
