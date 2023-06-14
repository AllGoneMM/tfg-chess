using Chess.Utils;

namespace Chess.Models
{
    public class Board
    {
        public static readonly int[] TopBounds = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

        public static readonly int[] BottomBounds = new int[] { 56, 57, 58, 59, 60, 61, 62, 63 };

        public static readonly int[] LeftBounds = new int[] { 0, 8, 16, 24, 32, 40, 48, 56 };

        public static readonly int[] RightBounds = new int[] { 7, 15, 23, 31, 39, 47, 55, 63 };

        public Tile[] BoardTiles { get; }

        public Board()
        {
            this.BoardTiles = FenConverter.ToTileArray(FenConverter.StartingFen);
        }

        public Board(string fen)
        {
            this.BoardTiles = FenConverter.ToTileArray(fen);
        }

        public Board(Tile[] boardTiles)
        {
            this.BoardTiles = new Tile[64];
            for (int i = 0; i < 64; i++)
            {
                this.BoardTiles[i] = new Tile(i);
                if (!boardTiles[i].IsEmpty())
                {
                    Piece piece = new Piece(boardTiles[i].Piece.Team, boardTiles[i].Piece.Type);
                    piece.NeedsPromotions = boardTiles[i].Piece.NeedsPromotions;
                    piece.IsAvailableForEnPassant = boardTiles[i].Piece.IsAvailableForEnPassant;
                    piece.HasMoved = boardTiles[i].Piece.HasMoved;
                    this.BoardTiles[i].PlacePiece(piece);
                }
            }
        }

        public string GetFen()
        {
            return FenConverter.ToFen(this.BoardTiles);
        }

        public Piece GetPiece(int tileNumber)
        {
            return this.BoardTiles[tileNumber].Piece;
        }

        public Tile GetTile(int tileNumber)
        {
            return this.BoardTiles[tileNumber];
        }

        public Board GetCopy()
        {
            return new Board(this.BoardTiles);
        }

        public Tile[] GetTileLayout()
        {
            return this.BoardTiles;
        }
    }
}
