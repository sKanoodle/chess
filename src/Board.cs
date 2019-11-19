using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chess
{
    class Board
    {
        private readonly Piece[] _Pieces = new Piece[64];
        public IEnumerable<Piece> Pieces => _Pieces.Where(p => p != default);
        public IEnumerable<Piece> PiecesByColor(Color color) => Pieces.Where(p => p.Color == color);
        public King KingOfColor(Color color) => PiecesByColor(color).OfType<King>().Single();
        public Piece this[int x, int y]
        {
            get => _Pieces[y * 8 + x];
            set => _Pieces[y * 8 + x] = value;
        }

        public Piece this[string position]
        {
            get => this[ParseXFromStringPosition(position), ParseYFromStringPosition(position)];
            set => this[ParseXFromStringPosition(position), ParseYFromStringPosition(position)] = value;
        }

        private Board(Piece[] pieces)
        {
            _Pieces = pieces;
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

        public bool IsAnyoneInCheck(out Color color)
        {
            if (KingOfColor(color = Color.White).IsInCheck(this))
                return true;
            return KingOfColor(color = Color.Black).IsInCheck(this);
        }

        public bool IsAnyoneInCheckmate(out Color color)
        {
            if (KingOfColor(color = Color.White).IsInCheckmate(this))
                return true;
            return KingOfColor(color = Color.Black).IsInCheckmate(this);
        }

        private (int X, int Y) ParseStringPosition(string position)
        {
            return (ParseXFromStringPosition(position), ParseYFromStringPosition(position));
        }

        private int ParseXFromStringPosition(string position)
        {
            var c = position[0];
            if (c < 'a' || c > 'h')
                throw new ArgumentException();
            return c - 'a';
        }

        private int ParseYFromStringPosition(string position)
        {
            var c = position[^1];
            if (c < '1' || c > '8')
                throw new ArgumentException();
            return c - '1';
        }

        public Board PreviewMove(Piece piece, int x, int y)
        {
            // only move piece in array, dont modify position in piece (piece in copied array is still the same piece as in source array)
            // should be no problem, because all relevant checks that this is used for only rely on position in array
            var result = new Board(_Pieces.ToArray());
            result[piece.X, piece.Y] = default;
            result[x, y] = piece;
            return result;
        }

        public bool TryMovePiece(Piece piece, int x, int y)
        {
            // TODO: missing castling
            if (!piece.CanMoveTo(x, y, this))
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

        public bool CouldPieceCaptureAt(Color color, int x, int y)
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
            if (notation == "0-0" || notation == "0-0-0")
                throw new NotImplementedException("castling is not implemented yet");

            var match = Regex.Match(notation, "^(?<piece>[KQRNB]?)(?<sourceX>[a-h]?)(?<sourceY>[1-8]?)(?<captures>x?)(?<destination>[a-h][1-8])(?<promotion>(=[QRNB])?)(?<check>\\+?)(?<mate>#?)$",
                RegexOptions.Singleline & RegexOptions.ExplicitCapture);
            if (!match.Success)
                throw new ArgumentException("cannot parse notation");

            var _pieces = PiecesByColor(color);
            _pieces = match.Groups["piece"].Value switch
            {
                "K" => _pieces.OfType<King>(),
                "Q" => _pieces.OfType<Queen>(),
                "R" => _pieces.OfType<Rook>(),
                "B" => _pieces.OfType<Bishop>(),
                "N" => _pieces.OfType<Knight>(),
                _ => _pieces.OfType<Pawn>(),
            };

            (var x, var y) = ParseStringPosition(match.Groups["destination"].Value);

            var pieces = _pieces.Where(p => p.CanMoveTo(x, y, this)).ToArray();
            if (!pieces.Any())
                throw new ArgumentException("no piece can move to this position");

            (var sourceX, var sourceY) = (match.Groups["sourceX"].Value, match.Groups["sourceY"].Value);
            pieces = pieces.Where(p => (string.IsNullOrEmpty(sourceX) || p.X == ParseXFromStringPosition(sourceX))
                && (string.IsNullOrEmpty(sourceY) || p.Y == ParseYFromStringPosition(sourceY))).ToArray();

            if (!pieces.Any())
                throw new ArgumentException("no piece matches the source of movement");

            if (pieces.Count() > 1)
                throw new ArgumentException("multiple pieces match this notation");

            if (!TryMovePiece(pieces.First(), x, y))
                throw new ArgumentException("move operation failed");
        }
    }
}
