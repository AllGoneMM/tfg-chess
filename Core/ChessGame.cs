using Chess.Exceptions;
using Chess.Models;
using Chess.Utils;
using Type = Chess.Models.Enumerations.Type;
using Chess.Models.Enumerations;

namespace Chess.Core
{
    public class ChessGame
    {
        public Status Status { get; private set; }

        public Team Turn
        {
            get
            {
                return _turn;
            }
            private set
            {
                CurrentTile = null;
                _turn = value;
            }
        }

        public bool PromotionAvailable { get; private set; }

        public Board Board { get; }

        public Tile CurrentTile { get; set; }

        private Team _turn;

        public ChessGame()
        {
            _turn = Team.WHITE;
            Board = new Board();
            PromotionAvailable = false;
            Status = Status.IN_PROGRESS;
        }

        public ChessGame(string fen, Team turn)
        {
            Turn = turn;
            Board = new Board(fen);
            Status = Status.IN_PROGRESS;
        }

        private bool IsStatusValid()
        {
            return Status == Status.IN_PROGRESS;
        }

        private bool IsSelectedTileValid(Tile tile)
        {
            return !tile.IsEmpty() && tile.Piece.Team == Turn;
        }

        public List<Move> SelectTile(string tile)
        {
            if (!IsStatusValid()) throw new IllegalStatusException();
            if (PromotionAvailable) throw new PromotionNeededException();

            Tile selectedTile = Board.GetTile(TileConverter.ToTileNumber(tile));

            if (!IsSelectedTileValid(selectedTile)) throw new IllegalTileException();

            CurrentTile = selectedTile;

            MoveValidator moveValidator = new MoveValidator();
            List<Move> selectedPieceLegalMoves = moveValidator.GetLegalMoves(Board, CurrentTile);
            if (selectedPieceLegalMoves.Count == 0) throw new NoLegalMovesException();
            return selectedPieceLegalMoves;
        }

        public void Promote(string pieceFen)
        {
            pieceFen = pieceFen.ToLower();
            Type promoteType = Type.NONE;
            foreach (Tile t in Board.BoardTiles)
            {
                if (!t.IsEmpty() && t.Piece.Type == Type.PAWN && t.Piece.NeedsPromotions)
                {
                    switch (pieceFen)
                    {
                        case "q": promoteType = Type.QUEEN; break;
                        case "r": promoteType = Type.ROOK; break;
                        case "b": promoteType = Type.BISHOP; break;
                        case "n": promoteType = Type.KNIGHT; break;
                        default: throw new ArgumentException();
                    }
                    t.Piece.Type = promoteType;
                }
            }
            PromotionAvailable = false;
            if (IsCheckMate())
            {
                if (Turn == Team.WHITE) Status = Status.WIN_BLACK;
                else Status = Status.WIN_WHITE;
            }
        }

        public void DeselectTile()
        {
            CurrentTile = null;
        }

        public void MovePiece(string tile)
        {
            int moveTile = TileConverter.ToTileNumber(tile);

            MoveValidator moveValidator = new MoveValidator();
            List<Move> legalMoves = moveValidator.GetLegalMoves(Board, CurrentTile);

            Move myMove = null;

            foreach (Move move in legalMoves)
            {
                if (move.MoveTile == moveTile)
                {
                    myMove = move; break;
                }
            }

            if (myMove == null) throw new IllegalMoveException();

            Move.MovePiece(myMove, Board);

            CheckPromotion();

            ChangeTurn();

            if (IsCheckMate())
            {
                if (Turn == Team.WHITE) Status = Status.WIN_BLACK;
                else Status = Status.WIN_WHITE;
            }

        }

        private void CheckPromotion()
        {
            foreach (Tile t in Board.BoardTiles)
            {
                if (!t.IsEmpty() && t.Piece.Type == Type.PAWN && t.Piece.NeedsPromotions)
                {
                    PromotionAvailable = true;
                }
            }
        }

        private bool IsCheckMate()
        {
            List<Move> allLegalMoves = new List<Move>();
            MoveValidator moveValidator = new MoveValidator();
            for (int i = 0; i < 64; i++)
            {
                if (!Board.GetTile(i).IsEmpty() && Board.GetPiece(i).Team == Turn)
                {
                    allLegalMoves.AddRange(moveValidator.GetLegalMoves(Board, Board.GetTile(i)));
                }
            }
            if (allLegalMoves.Count == 0)
            {
                return true;
            }
            return false;
        }

        public void ChangeTurn()
        {
            if (Turn == Team.WHITE)
            {
                Turn = Team.BLACK;
            }
            else
                Turn = Team.WHITE;
        }

        public string GetFullFen()
        {
            return FenConverter.ToFullFen(this.Board.GetTileLayout(), this.Turn);
        }
    }

}
