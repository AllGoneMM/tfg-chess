using Chess.Models;
using Chess.Exceptions;
using Type = Chess.Models.Enumerations.Type;
using Chess.Models.Enumerations;
using System.Text;

namespace Chess.Utils
{
    public static class FenConverter
    {
        public static readonly string StartingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

        public static Tile[] ToTileArray(string fen)
        {
            if (!IsFenValid(fen)) throw new FenFormatException();

            Tile[] tiles = new Tile[64];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(i);
            }

            string[] fenParts = fen.Split('/');

            int tileNumber = 0;
            foreach (string part in fenParts)
            {
                foreach (char partComponent in part)
                {
                    if (char.IsDigit(partComponent))
                    {
                        tileNumber += int.Parse(partComponent.ToString());
                    }
                    else
                    {
                        tiles[tileNumber].PlacePiece(GetPieceFromFen(partComponent));
                        tileNumber++;
                    }
                }
            }
            return tiles;
        }

        public static string ToFen(Tile[] tiles)
        {
            if (!IsTileArrayValid(tiles)) throw new InvalidTileArrayException();

            StringBuilder fenBuilder = new StringBuilder();
            int emptyCount = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column <= 7; column++)
                {
                    int tileNumber = row * 8 + column;
                    if (tiles[tileNumber].IsEmpty()) emptyCount++;
                    else
                    {
                        if (emptyCount > 0)
                        {
                            fenBuilder.Append(emptyCount.ToString());
                            emptyCount = 0;
                        }
                        Piece? piece = tiles[tileNumber].Piece;
                        if (piece != null) fenBuilder.Append(GetPieceFen(piece));
                    }
                }

                if (emptyCount > 0)
                {
                    fenBuilder.Append(emptyCount.ToString());
                    emptyCount = 0;
                }

                if (row < 7)
                {
                    fenBuilder.Append("/");
                }
            }

            return fenBuilder.ToString();
        }

        public static string ToFullFen(Tile[] tiles, Team turn)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ToFen(tiles));
            builder.Append(GetTurnString(turn));
            builder.Append(GetCastlingString(tiles));
            builder.Append(GetEnPassantString(tiles, turn));
            builder.Append("0 1");

            return builder.ToString();
        }

        private static string GetEnPassantString(Tile[] tiles, Team turn)
        {
            String enPassant = "-";

            if (turn == Team.WHITE)
            {
                int[] passantWhiteRange = new int[] { 24, 25, 26, 27, 28, 29, 30, 31 };
                foreach (int i in passantWhiteRange)
                {
                    if (!tiles[i].IsEmpty() && tiles[i].Piece.Team == Team.WHITE && tiles[i].Piece.Type == Type.PAWN)
                    {
                        if (i == 24)
                        {
                            if (!tiles[25].IsEmpty() && tiles[25].Piece.Team == Team.BLACK && tiles[25].Piece.Type == Type.PAWN && tiles[25].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(25 + Direction.Up);
                            }
                        }
                        else if (i == 31)
                        {
                            if (!tiles[30].IsEmpty() && tiles[30].Piece.Team == Team.BLACK && tiles[30].Piece.Type == Type.PAWN && tiles[30].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(30 + Direction.Up);
                            }
                        }
                        else
                        {
                            if (!tiles[i + 1].IsEmpty() && tiles[i + 1].Piece.Team == Team.BLACK && tiles[i + 1].Piece.Type == Type.PAWN && tiles[i + 1].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(i + 1 + Direction.Up);
                            }
                            if (!tiles[i - 1].IsEmpty() && tiles[i - 1].Piece.Team == Team.BLACK && tiles[i - 1].Piece.Type == Type.PAWN && tiles[i - 1].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(i - 1 + Direction.Up);
                            }
                        }
                    }
                }
            }
            else
            {
                int[] passantBlackRange = new int[] { 32, 33, 34, 35, 36, 37, 38, 39 };
                foreach (int i in passantBlackRange)
                {
                    if (!tiles[i].IsEmpty() && tiles[i].Piece.Team == Team.BLACK && tiles[i].Piece.Type == Type.PAWN)
                    {
                        if (i == 32)
                        {
                            if (!tiles[33].IsEmpty() && tiles[33].Piece.Team == Team.WHITE && tiles[33].Piece.Type == Type.PAWN && tiles[33].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(33 + Direction.Down);
                            }
                        }
                        else if (i == 39)
                        {
                            if (!tiles[39].IsEmpty() && tiles[39].Piece.Team == Team.WHITE && tiles[39].Piece.Type == Type.PAWN && tiles[39].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(39 + Direction.Down);
                            }
                        }
                        else
                        {
                            if (!tiles[i + 1].IsEmpty() && tiles[i + 1].Piece.Team == Team.WHITE && tiles[i + 1].Piece.Type == Type.PAWN && tiles[i + 1].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(i + 1 + Direction.Down);
                            }
                            if (!tiles[i - 1].IsEmpty() && tiles[i - 1].Piece.Team == Team.WHITE && tiles[i - 1].Piece.Type == Type.PAWN && tiles[i - 1].Piece.IsAvailableForEnPassant)
                            {
                                enPassant = ConvertPosition(i - 1 + Direction.Down);
                            }
                        }
                    }
                }
            }
            enPassant = enPassant + " ";
            return enPassant;
        }

        public static string ConvertPosition(int position)
        {
            if (position < 0 || position > 63)
            {
                throw new ArgumentOutOfRangeException("posicion", "La posición debe estar entre 0 y 63.");
            }

            int columna = position % 8;
            int fila = position / 8;

            char letraColumna = (char)('a' + columna);
            int numeroFila = 8 - fila;

            return letraColumna.ToString() + numeroFila.ToString();
        }

        private static string GetCastlingString(Tile[] tiles)
        {
            Tile whiteKingTile = tiles[60];
            Tile blackKingTile = tiles[4];
            Tile whiteRookQueenSide = tiles[56];
            Tile whiteRookKingSide = tiles[63];
            Tile blackRookQueenSide = tiles[0];
            Tile blackRookKingSide = tiles[7];

            StringBuilder builder = new StringBuilder();
            if (!whiteKingTile.IsEmpty() && whiteKingTile.Piece.Type == Type.KING && whiteKingTile.Piece.HasMoved == false)
            {
                if (!whiteRookKingSide.IsEmpty() && whiteRookKingSide.Piece.Type == Type.ROOK && whiteRookKingSide.Piece.HasMoved == false)
                {
                    builder.Append('K');
                }
                if (!whiteRookQueenSide.IsEmpty() && whiteRookQueenSide.Piece.Type == Type.ROOK && whiteRookQueenSide.Piece.HasMoved == false)
                {
                    builder.Append('Q');
                }
            }
            if (!blackKingTile.IsEmpty() && blackKingTile.Piece.Type == Type.KING && blackKingTile.Piece.HasMoved == false)
            {
                if (!blackRookKingSide.IsEmpty() && blackRookKingSide.Piece.Type == Type.ROOK && blackRookKingSide.Piece.HasMoved == false)
                {
                    builder.Append('k');
                }
                if (!blackRookQueenSide.IsEmpty() && blackRookQueenSide.Piece.Type == Type.ROOK && blackRookQueenSide.Piece.HasMoved == false)
                {
                    builder.Append('q');
                }
            }
            if (string.IsNullOrEmpty(builder.ToString())) return "- ";
            else
            {
                builder.Append(" ");
                return builder.ToString();
            }
        }

        private static string GetTurnString(Team turn)
        {
            if (turn == Team.WHITE)
            {
                return " w ";
            }
            else return " b ";
        }

        private static char GetPieceFen(Piece piece)
        {
            char pieceFen = '!';
            switch (piece.Type)
            {
                case Type.PAWN:
                    pieceFen = 'p';
                    break;
                case Type.KNIGHT:
                    pieceFen = 'n';
                    break;
                case Type.ROOK:
                    pieceFen = 'r';
                    break;
                case Type.BISHOP:
                    pieceFen = 'b';
                    break;
                case Type.QUEEN:
                    pieceFen = 'q';
                    break;
                case Type.KING:
                    pieceFen = 'k';
                    break;
            }
            if (piece.Team == Team.WHITE) pieceFen = char.ToUpper(pieceFen);
            return pieceFen;
        }

        private static Piece GetPieceFromFen(char pieceFen)
        {
            Team team = Team.NONE;
            if (char.IsLower(pieceFen)) team = Team.BLACK;
            else team = Team.WHITE;

            Type type = Type.NONE;

            pieceFen = char.ToUpper(pieceFen);

            switch (pieceFen)
            {
                case 'P': type = Type.PAWN; break;
                case 'R': type = Type.ROOK; break;
                case 'B': type = Type.BISHOP; break;
                case 'N': type = Type.KNIGHT; break;
                case 'Q': type = Type.QUEEN; break;
                case 'K': type = Type.KING; break;
            }

            return new Piece(team, type);

        }

        public static bool IsFenValid(string fen)
        {
            if (fen.Length < 15) return false;

            string[] parts = fen.Split('/');
            if (parts.Length != 8) return false;

            foreach (string p in parts)
            {
                if (p.Length > 8) return false;

                int rowCount = 0;
                foreach (char c in p)
                {
                    if (!"rnbqkpRNBQKP".Contains(c) && !char.IsDigit(c)) return false;
                    if ("rnbqkpRNBQKP".Contains(c)) rowCount++;
                    else rowCount += int.Parse(c.ToString());
                }
                if (rowCount != 8) return false;
            }
            return true;
        }

        private static bool IsTileArrayValid(Tile[] tiles)
        {
            if (tiles.Length == 64)
            {
                foreach (Tile tile in tiles)
                {
                    if (tile == null) return false;
                }
                return true;
            }
            return false;
        }
    }
}
