using Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chess
{
    public class Board
    {
        private readonly Piece[] _Pieces = new Piece[64];
        public IEnumerable<Piece> Pieces => _Pieces.Where(p => p != default);
        public IEnumerable<Piece> PiecesByColor(Color color) => Pieces.Where(p => p.Color == color);
        public King KingOfColor(Color color) => PiecesByColor(color).OfType<King>().Single();
        public Piece this[int x, int y]
        {
            get => _Pieces[y * 8 + x];
            protected set => _Pieces[y * 8 + x] = value;
        }

        public (int X, int Y) LastMoveOrigin { get; private set; } = (-1, -1);
        public (int X, int Y) LastMoveDestination { get; private set; } = (-1, -1);

        public (int X, int Y) PossibleEnPassantPosition { get; private set; } = (-1, -1);

        public Color? Winner { get; private set; }
        public bool IsDraw { get; private set; }
        public bool IsGameOver => IsDraw || Winner != default;

        public Piece this[string position]
        {
            get => this[ParseXFromStringPosition(position), ParseYFromStringPosition(position)];
            set => this[ParseXFromStringPosition(position), ParseYFromStringPosition(position)] = value;
        }

        protected Board(Piece[] pieces)
        {
            if (pieces != default)
                _Pieces = pieces;
        }

        public Board()
        {
            BuildStartingBoard();
        }

        protected void BuildStartingBoard()
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

        public void Surrender(Color color)
        {
            Winner = color.Invert();
        }

        public bool IsAnyoneInCheck(out Color color)
        {
            if (KingOfColor(color = Color.White).IsInCheck(this, out _))
                return true;
            return KingOfColor(color = Color.Black).IsInCheck(this, out _);
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
            if (piece is King king && y == king.Y && (x == king.X - 2 || x == king.X + 2)) // castling requested
            {
                if (king.HasMoved || king.IsInCheck(this, out _))
                    return false; // king may not castle when in check or has moved
                var rook = this[x > 4 ? 7 : 0, y];
                if (!(rook is Rook) || rook.HasMoved)
                    return false; // there is no rook where a rook should be or it has moved TODO: handicap games would allow castling without a rook
                var squaresBetween = Enumerable.Range(Math.Min(rook.X, king.X) + 1, rook.X == 0 ? 3 : 2); // if rook.X is 0 it will be a queen-side castle
                foreach (var _x in squaresBetween)
                    if (this[_x, y] != default)
                        return false; // there may not be any pieces between king and rook
                var kingMovementSquares = Enumerable.Range(Math.Min(rook.X + 1, king.X) + 1, 2); // rook.X + 1 to adjust for queen-side castling
                foreach (var _x in kingMovementSquares)
                    if (PreviewMove(king, _x, y).GetPiecesThatCouldMoveTo(king.Color.Invert(), _x, y, true).Any())
                        return false; // king may not move through or into check

                int rookDestinationX = king.X + (rook.X < king.X ? -1 : 1);
                MovePiece(rook, rookDestinationX, y);
                MovePiece(king, x, y);
                return true;
            }

            if (!piece.CanMoveTo(x, y, this))
                return false;
            MovePiece(piece, x, y);
            return true;
        }

        private void CapturePiece(Piece piece)
        {
            this[piece.X, piece.Y] = default;
        }

        private void MovePiece(Piece piece, int x, int y)
        {
            LastMoveOrigin = (piece.X, piece.Y);
            LastMoveDestination = (x, y);
            var enPassant = PossibleEnPassantPosition;
            PossibleEnPassantPosition = (-1, -1);

            if (this[x, y] != default)
                CapturePiece(this[x, y]);

            if (piece is Pawn)
            {
                if ((x, y) == enPassant) // capturing enemy pawn en passant
                    CapturePiece(this[x, piece.Y]);

                if (Math.Abs(piece.Y - y) == 2) // pawn moved 2 tiles
                    PossibleEnPassantPosition = (x, Math.Min(piece.Y, y) + 1);
            }

            this[piece.X, piece.Y] = default;
            this[x, y] = piece;
            piece.MoveTo(x, y);
        }

        public void PromotePawn(Piece piece)
        {
            var pawn = this[piece.X, piece.Y] as Pawn;
            if (pawn == default || pawn.Color != piece.Color
                || pawn.Color == Color.White && pawn.Y != 7
                || pawn.Color == Color.Black && pawn.Y != 0)
                return; // there is no pawn or pawn has wrong position/color
            if (piece is King || piece is Pawn)
                return; // cannot promote to king or pawn
            this[piece.X, piece.Y] = piece;
        }

        public bool CheckForEndOfGame()
        {
            if (IsGameOver) return true;
            if (IsAnyoneInCheckmate(out var loser))
            {
                Winner = loser.Invert();
                return true;
            }
            return false;
        }

        public IEnumerable<Piece> GetPiecesThatCouldMoveTo(Piece piece, bool isOnlyCheckDetection = false) =>
            GetPiecesThatCouldMoveTo(piece.Color.Invert(), piece.X, piece.Y, isOnlyCheckDetection);

        public IEnumerable<Piece> GetPiecesThatCouldMoveTo(Color color, int x, int y, bool isOnlyCheckDetection = false)
        {
            return PiecesByColor(color).Where(p => p.CanMoveTo(x, y, this, isOnlyCheckDetection));
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

        private readonly string[] KingSideCastle = new[] { "0-0", "O-O" };
        private readonly string[] QueenSideCastle = new[] { "0-0-0", "O-O-O" };
        private const string WhiteWon = "1-0";
        private const string BlackWon = "0-1";
        private const string Draw = "½-½";

        public void PerformAlgebraicChessNotationMove(Color color, string notation)
        {
            if (notation == WhiteWon || notation == BlackWon || notation == Draw)
            {
                switch (notation)
                {
                    case WhiteWon: Winner = Color.White; return;
                    case BlackWon: Winner = Color.Black; return;
                    case Draw: IsDraw = true; return;
                }
            }

            if (KingSideCastle.Contains(notation) || QueenSideCastle.Contains(notation))
            {
                var piece = KingOfColor(color);
                if (!TryMovePiece(piece, piece.X + (KingSideCastle.Contains(notation) ? 2 : -2), piece.Y))
                    throw new ArgumentException("castling failed");
                return;
            }

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

            var promotion = match.Groups["promotion"].Value;
            if (!string.IsNullOrEmpty(promotion))
                PromotePawn(promotion[1] switch
                {
                    'Q' => new Queen(color, x, y),
                    'R' => new Rook(color, x, y),
                    'N' => new Knight(color, x, y),
                    'B' => new Bishop(color, x, y),
                    _ => throw new ArgumentException("promotion to this piece is not defined")
                });
        }
    }
}
