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

        public Piece this[(int X, int Y) position]
        {
            get => this[position.X, position.Y];
            protected set => this[position.X, position.Y] = value;
        }

        public Piece this[string notation]
        {
            get => this[ConversionHelpers.NotationToPosition(notation)];
            protected set => this[ConversionHelpers.NotationToPosition(notation)] = value;
        }

        public (int X, int Y) LastMoveOrigin { get; private set; } = (-1, -1);
        public (int X, int Y) LastMoveDestination { get; private set; } = (-1, -1);

        public (int X, int Y) PossibleEnPassantTarget { get; private set; } = (-1, -1);

        public Color ColorToMoveNext { get; private set; } = Color.White;
        /// <summary>
        /// number of halfmoves since the last capture or pawn advance
        /// </summary>
        public int HalfmoveClock { get; private set; }
        /// <summary>
        /// number of full moves in the game
        /// </summary>
        public int FullmoveNumber { get; private set; } = 1;

        public Color? Winner { get; private set; }
        public bool IsDraw { get; private set; }
        public bool WinnerIsUnkown { get; private set; }
        public bool IsGameOver => IsDraw || WinnerIsUnkown || Winner != default;

        protected Board(Piece[] pieces)
        {
            if (pieces != default)
                _Pieces = pieces;
        }

        public Board()
        {
            BuildStartingBoard();
        }

        private Board Clone()
        {
            return new Board(_Pieces.ToArray())
            {
                ColorToMoveNext = ColorToMoveNext,
                FullmoveNumber = FullmoveNumber,
                HalfmoveClock = HalfmoveClock,
                IsDraw = IsDraw,
                LastMoveDestination = LastMoveDestination,
                LastMoveOrigin = LastMoveOrigin,
                Winner = Winner,
                PossibleEnPassantTarget = PossibleEnPassantTarget,
            };
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

        public Board PreviewMove(Piece piece, int x, int y)
        {
            // only move piece in array, dont modify position in piece (piece in copied array is still the same piece as in source array)
            // should be no problem, because all relevant checks that this is used for only rely on position in array
            var result = Clone();
            result.MovePiece(piece, x, y, false);
            return result;
        }

        public bool TryMovePiece(Piece piece, int x, int y)
        {
            if (piece.Color != ColorToMoveNext)
                return false;

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
                MovePieceInternal(rook, rookDestinationX, y);
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

        /// <param name="moveActualPiece">whether to set move target coordinates in the piece, otherwise just move piece in the array without changing the piece</param>
        private void MovePiece(Piece piece, int x, int y, bool moveActualPiece = true)
        {
            LastMoveOrigin = piece.Position;
            LastMoveDestination = (x, y);
            var enPassant = PossibleEnPassantTarget;
            PossibleEnPassantTarget = (-1, -1);

            HalfmoveClock += 1;
            if (this[x, y] != default || piece is Pawn) // piece will be captured or pawn is moved
                HalfmoveClock = 0;

            if (this[x, y] != default)
                CapturePiece(this[x, y]);

            if (piece is Pawn)
            {
                if ((x, y) == enPassant) // capturing enemy pawn en passant
                    CapturePiece(this[x, piece.Y]);

                if (Math.Abs(piece.Y - y) == 2) // pawn moved 2 tiles
                    PossibleEnPassantTarget = (x, Math.Min(piece.Y, y) + 1);
            }

            MovePieceInternal(piece, x, y, moveActualPiece);

            if (ColorToMoveNext == Color.Black)
                FullmoveNumber += 1;
            ColorToMoveNext = ColorToMoveNext.Invert();
        }

        /// <param name="moveActualPiece">whether to set move target coordinates in the piece, otherwise just move piece in the array without changing the piece</param>
        private void MovePieceInternal(Piece piece, int x, int y, bool moveActualPiece = true)
        {
            this[piece.Position] = default;
            this[x, y] = piece;
            if (moveActualPiece)
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

        public bool TryPerformCoordinateNotationMove(string notation)
        {
            try { PerformCoordinateNotationMove(notation); }
            catch { return false; }
            return true;
        }

        public void PerformCoordinateNotationMove(string notation)
        {
            var match = Regex.Match(notation, "^([a-h][1-8])([a-h][1-8])$");
            if (!match.Success)
                throw new ArgumentException("could not parse notation");
            var piece = this[match.Groups[1].Value];
            if (piece == default || piece.Color != ColorToMoveNext)
                throw new ArgumentException("no piece at starting position or it is not the color's turn");
            (int x, int y) = ConversionHelpers.NotationToPosition(match.Groups[2].Value);
            if (!TryMovePiece(piece, x, y))
                throw new ArgumentException("piece could not perform move");
        }

        public bool TryPerformAlgebraicChessNotationMove(string notation)
        {
            try { PerformAlgebraicChessNotationMove(notation); }
            catch { return false; }
            return true;
        }

        private readonly string[] KingSideCastle = new[] { "0-0", "O-O" };
        private readonly string[] QueenSideCastle = new[] { "0-0-0", "O-O-O" };
        private const string WhiteWon = "1-0";
        private const string BlackWon = "0-1";
        private readonly string[] Draw = new[] { "½-½", "1/2-1/2" };
        private const string WinnerUnknown = "*";

        public void PerformAlgebraicChessNotationMove(string notation)
        {
            var color = ColorToMoveNext;
            bool isKingSideCastle = KingSideCastle.Any(s => notation.StartsWith(s));
            bool isQueenSideCastle = QueenSideCastle.Any(s => notation.StartsWith(s));

            if (notation == WhiteWon || notation == BlackWon || Draw.Contains(notation) || notation == WinnerUnknown)
            {
                switch (notation)
                {
                    case WhiteWon: Winner = Color.White; return;
                    case BlackWon: Winner = Color.Black; return;
                    case WinnerUnknown: WinnerIsUnkown = true; return;
                    case string _ when Draw.Contains(notation): IsDraw = true; return;
                }
            }

            if (isKingSideCastle || isQueenSideCastle)
            {
                var piece = KingOfColor(color);
                // definitely have to ask for queenSide castle first, because of startsWith check, kingSideCastle is true when queenSideCastle is requested
                if (!TryMovePiece(piece, piece.X + (isQueenSideCastle ? -2 : 2), piece.Y))
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

            (var x, var y) = ConversionHelpers.NotationToPosition(match.Groups["destination"].Value);

            var pieces = _pieces.Where(p => p.CanMoveTo(x, y, this)).ToArray();
            if (!pieces.Any())
                throw new ArgumentException("no piece can move to this position");

            (var sourceX, var sourceY) = (match.Groups["sourceX"].Value, match.Groups["sourceY"].Value);
            pieces = pieces.Where(p => (string.IsNullOrEmpty(sourceX) || p.X == ConversionHelpers.NotationToX(sourceX))
                && (string.IsNullOrEmpty(sourceY) || p.Y == ConversionHelpers.NotationToY(sourceY))).ToArray();

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

        public string GetForsythEdwardsNotation()
        {
            string makeRow(int y)
            {
                StringBuilder row = new StringBuilder();
                int emptyFields = 0;
                for (int x = 0; x < 8; x++)
                {
                    var piece = this[x, y];
                    if (piece == default)
                    {
                        emptyFields += 1;
                        continue;
                    }
                    if (emptyFields > 0)
                    {
                        row.Append(emptyFields);
                        emptyFields = 0;
                    }
                    var pieceAbbreviation = piece switch
                    {
                        Pawn _ => "p",
                        Rook _ => "r",
                        Knight _ => "n",
                        Bishop _ => "b",
                        Queen _ => "q",
                        King _ => "k",
                        _ => throw new NotImplementedException(),
                    };
                    if (piece.Color == Color.White)
                        pieceAbbreviation = pieceAbbreviation.ToUpper();
                    row.Append(pieceAbbreviation);
                }
                if (emptyFields > 0)
                    row.Append(emptyFields);
                return row.ToString();
            }

            bool CanCastle(string rookPosition)
            {
                var pos = ConversionHelpers.NotationToPosition(rookPosition);
                var rook = this[pos] as Rook;
                var king = this[4, pos.Y] as King;
                if (rook == default || rook.HasMoved)
                    return false;
                if (king == default || king.HasMoved)
                    return false;
                return true;
            }

            var board = string.Join('/', Enumerable.Range(0, 8).Reverse().Select(makeRow));

            var possibleCastles = string.Join("", new (string Position, string Notation)[] { ("h1", "K"), ("a1", "Q"), ("h8", "k"), ("h1", "q") }
                .Where(p => CanCastle(p.Position))
                .Select(p => p.Notation));
            if (string.IsNullOrEmpty(possibleCastles))
                possibleCastles = "-";

            var enPassant = PossibleEnPassantTarget != (-1, -1) ? ConversionHelpers.PositionToNotation(PossibleEnPassantTarget) : "-";

            return $"{board} {(ColorToMoveNext == Color.Black ? "b" : "w")} {possibleCastles} {enPassant} {HalfmoveClock} {FullmoveNumber}";
        }
    }
}
