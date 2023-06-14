using Chess.Models.Enumerations;
using Chess.Utils;
using Type = Chess.Models.Enumerations.Type;

namespace Chess.Models
{
    public class Move
    {
        public int MoveTile { get; private set; }

        public int OriginTile { get; private set; }

        public SpecialMove SpecialMove { get; private set; }

        public Move(int moveTile, int originTile, SpecialMove specialMove)
        {
            this.SpecialMove = specialMove;
            this.OriginTile = originTile;
            this.MoveTile = moveTile;
        }

        public static void MovePiece(Move move, Board board)
        {
            MoveValidator moveValidator = new MoveValidator();
            int moveTile = move.MoveTile;
            int originTile = move.OriginTile;
            Team turn = board.GetPiece(originTile).Team;

            switch (move.SpecialMove)
            {
                case SpecialMove.NONE:
                    board.BoardTiles[moveTile].PlacePiece(board.BoardTiles[originTile].Piece);
                    board.BoardTiles[moveTile].Piece.HasMoved = true;
                    board.BoardTiles[originTile].RemovePiece();
                    break;
                case SpecialMove.CASTLING:
                    board.BoardTiles[moveTile].PlacePiece(board.BoardTiles[originTile].Piece);
                    board.BoardTiles[moveTile].Piece.HasMoved = true;
                    board.BoardTiles[originTile].RemovePiece();
                    if (turn == Team.WHITE)
                    {
                        if (moveTile == 62)
                        {
                            board.GetTile(61).PlacePiece(board.GetPiece(63));
                            board.GetPiece(61).HasMoved = true;
                            board.GetTile(63).RemovePiece();
                        }
                        else
                        {
                            board.GetTile(59).PlacePiece(board.GetPiece(56));
                            board.GetPiece(59).HasMoved = true;
                            board.GetTile(56).RemovePiece();
                        }
                    }
                    else
                    {
                        if (moveTile == 6)
                        {
                            board.GetTile(5).PlacePiece(board.GetPiece(7));
                            board.GetPiece(5).HasMoved = true;
                            board.GetTile(7).RemovePiece();
                        }
                        else
                        {
                            board.GetTile(3).PlacePiece(board.GetPiece(0));
                            board.GetPiece(3).HasMoved = true;
                            board.GetTile(0).RemovePiece();
                        }
                    }
                    break;
                case SpecialMove.PROMOTION:
                    board.BoardTiles[moveTile].PlacePiece(board.BoardTiles[originTile].Piece);
                    board.BoardTiles[moveTile].Piece.HasMoved = true;
                    board.BoardTiles[moveTile].Piece.NeedsPromotions = true;
                    board.BoardTiles[originTile].RemovePiece();

                    break;
                case SpecialMove.DOBLE_STEP:
                    board.BoardTiles[moveTile].PlacePiece(board.BoardTiles[originTile].Piece);
                    board.BoardTiles[moveTile].Piece.HasMoved = true;
                    board.BoardTiles[moveTile].Piece.IsAvailableForEnPassant = true;
                    board.BoardTiles[originTile].RemovePiece();
                    break;
                case SpecialMove.EN_PASSANT:
                    int backTile = 0;
                    if (board.GetPiece(originTile).Team == Team.WHITE) backTile = Direction.Down;
                    else backTile = Direction.Up;

                    board.BoardTiles[moveTile].PlacePiece(board.BoardTiles[originTile].Piece);
                    board.BoardTiles[moveTile].Piece.HasMoved = true;
                    board.BoardTiles[originTile].RemovePiece();

                    board.BoardTiles[moveTile + backTile].RemovePiece();
                    break;
            }

            //ELIMINA EN PASSANT
            foreach (Tile t in board.BoardTiles)
            {
                if (!t.IsEmpty() && t.Piece.Type == Type.PAWN && t.Piece.Team != turn && t.Piece.IsAvailableForEnPassant == true)
                {
                    t.Piece.IsAvailableForEnPassant = false;
                }
            }
        }
    }
}


