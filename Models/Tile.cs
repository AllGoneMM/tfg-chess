namespace Chess.Models
{
    public class Tile
    {
        public Tile(int tileNumber)
        {
            this.TileNumber = tileNumber;
        }

        public Piece Piece { get; private set; }

        public int TileNumber { get; }

        public bool IsEmpty()
        {
            return Piece == null;
        }

        public void PlacePiece(Piece piece)
        {
            this.Piece = piece;
        }

        public void RemovePiece()
        {
            this.Piece = null;
        }
    }
}
