using Chess.Models;
using Chess.Models.Enumerations;
using Type = Chess.Models.Enumerations.Type;

namespace Chess.Utils
{
    public class MoveValidator
    {
        private List<Move> _legalMoves;

        private Board _board;

        private Tile _tile;

        private Piece _piece;

        private int _originTile;

        public List<Move> GetLegalMoves(Board board, Tile originTile)
        {
            List<Move> legalMoves = GetPieceMoves(board, originTile);
            legalMoves = RemoveCheckMoves(legalMoves);
            return legalMoves;
        }

        private List<Move> GetPieceMoves(Board board, Tile originTile)
        {
            _legalMoves = new();
            _board = board;
            _tile = originTile;
            _piece = originTile.Piece;
            _originTile = _tile.TileNumber;

            if (_piece != null)
            {
                switch (_piece.Type)
                {
                    case Type.PAWN: AddPawnMoves(); break;
                    case Type.KNIGHT: AddKnightMoves(); break;
                    case Type.BISHOP: AddBishopMoves(); break;
                    case Type.ROOK: AddRookMoves(); break;
                    case Type.QUEEN: AddRookMoves(); AddBishopMoves(); break;
                    case Type.KING: AddKingMoves(); break;
                }
            }
            return new List<Move>(_legalMoves);
        }

        private List<Move> RemoveCheckMoves(List<Move> legalMoves)
        {
            List<Move> updatedLegalMoves = new(legalMoves);
            foreach (Move move in legalMoves)
            {
                //COPIA LA MESA
                Board auxBoard = _board.GetCopy();

                //REALIZA EL MOVIMIENTO EN LA MESA DE COPIA
                Move.MovePiece(move, auxBoard);

                //RECORRE LA MESA DE COPIA Y VA GUARDANDO TODOS LOS MOVIMIENTOS LEGALES DE TODAS LAS PIEZAS ENEMIGAS
                List<Move> allEnemyLegalMoves = new();
                const int boardTileCount = 64;

                for (int i = 0; i < boardTileCount; i++)
                {
                    if (!auxBoard.GetTile(i).IsEmpty() && auxBoard.GetPiece(i).Team != _tile.Piece.Team)
                    {
                        MoveValidator validator = new MoveValidator();
                        foreach (Move enemyMove in validator.GetPieceMoves(auxBoard, auxBoard.GetTile(i)))
                        {
                            allEnemyLegalMoves.Add(enemyMove);
                        }
                    }
                }

                //VUELVE A RECORRER LA COPIA DE LA MESA, POR CADA PIEZA ALIADA, DETECTA SI EL REY SE ENCUENTRA EN LA LISTA DE MOVIMIENTOS ENEMIGOS
                for (int i = 0; i < boardTileCount; i++)
                {
                    if (!auxBoard.GetTile(i).IsEmpty() && auxBoard.GetPiece(i).Team == _piece.Team && auxBoard.GetPiece(i).Type == Type.KING)
                    {
                        foreach (Move enemyMove in allEnemyLegalMoves)
                        {
                            if (i == enemyMove.MoveTile)
                            {
                                updatedLegalMoves.Remove(move);
                            }
                            if (move.SpecialMove == SpecialMove.CASTLING)
                            {
                                if(enemyMove.MoveTile == move.OriginTile)
                                {
                                    updatedLegalMoves.Remove(move);
                                }
                                int originTile = move.OriginTile;
                                if(move.OriginTile == 60)
                                {
                                    if(move.MoveTile == 62)
                                    {
                                        if (enemyMove.MoveTile == 61) updatedLegalMoves.Remove(move);
                                    }
                                    else
                                    {
                                        if (enemyMove.MoveTile == 59) updatedLegalMoves.Remove(move);
                                    }
                                }
                                else
                                {
                                    if(move.MoveTile == 6)
                                    {
                                        if (enemyMove.MoveTile == 5) updatedLegalMoves.Remove(move);
                                    }
                                    else
                                    {
                                        if (enemyMove.MoveTile == 3) updatedLegalMoves.Remove(move);
                                    }
                                }
                            }
                        } 
                            
                    }
                }
            }
            return updatedLegalMoves;
        }

        private void CalculateKingMoves(int direction)
        {
            if (IsMoveInsideBounds(direction, _originTile) && IsMoveTileEmptyOrOcuppiedByEnemy(direction, _originTile))
            {
                Move move = new Move(direction + _originTile, _originTile, SpecialMove.NONE);
                _legalMoves.Add(move);
            }
        }

        private void AddKingMoves()
        {
            int[] allDirections = new int[8] { Direction.Left, Direction.Right, Direction.Up, Direction.Down, Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight };

            foreach (int direction in allDirections)
            {
                CalculateKingMoves(direction);
            }

            //CASTLING ... AÑADIR QUE NO PUEDE ESTAR EN JAQUE
            if (!_piece.HasMoved)
            {
                //CASTLING PARA BLANCAS
                if (_piece.Team == Team.WHITE)
                {
                    //CORTO
                    if (IsMoveInsideBounds(Direction.Right, _originTile) && IsMoveTileEmpty(Direction.Right, _originTile) && IsMoveInsideBounds(Direction.Right, _originTile + Direction.Right) && IsMoveTileEmpty(Direction.Right, _originTile + Direction.Right))
                    {
                        if (!_board.GetTile(63).IsEmpty() && _board.GetPiece(63).Team == _piece.Team && !_board.GetPiece(63).HasMoved)
                        {
                            Move move = new Move(_originTile + Direction.Right + Direction.Right, _originTile, SpecialMove.CASTLING);
                            _legalMoves.Add(move);
                        }
                    }

                    //LARGO
                    if (IsMoveInsideBounds(Direction.Left, _originTile) && IsMoveTileEmpty(Direction.Left, _originTile) && IsMoveInsideBounds(Direction.Left, _originTile + Direction.Left) && IsMoveTileEmpty(Direction.Left, _originTile + Direction.Left) && IsMoveInsideBounds(Direction.Left, _originTile + Direction.Left + Direction.Left) && IsMoveTileEmpty(Direction.Left, _originTile + Direction.Left + Direction.Left))
                    {
                        if (!_board.GetTile(56).IsEmpty() && _board.GetPiece(56).Team == _piece.Team && !_board.GetPiece(56).HasMoved)
                        {
                            Move move = new Move(_originTile + Direction.Left + Direction.Left, _originTile, SpecialMove.CASTLING);
                            _legalMoves.Add(move);
                        }
                    }

                }
                //CASTLING PARA NEGRAS
                else
                {
                    //CORTO
                    if (IsMoveInsideBounds(Direction.Right, _originTile) && IsMoveTileEmpty(Direction.Right, _originTile) && IsMoveInsideBounds(Direction.Right, _originTile + Direction.Right) && IsMoveTileEmpty(Direction.Right, _originTile + Direction.Right))
                    {
                        if (!_board.GetTile(7).IsEmpty() && _board.GetPiece(7).Team == _piece.Team && !_board.GetPiece(7).HasMoved)
                        {
                            Move move = new Move(_originTile + Direction.Right + Direction.Right, _originTile, SpecialMove.CASTLING);
                            _legalMoves.Add(move);
                        }
                    }

                    //LARGO
                    if (IsMoveInsideBounds(Direction.Left, _originTile) && IsMoveTileEmpty(Direction.Left, _originTile) && IsMoveInsideBounds(Direction.Left, _originTile + Direction.Left) && IsMoveTileEmpty(Direction.Left, _originTile + Direction.Left) && IsMoveInsideBounds(Direction.Left, _originTile + Direction.Left + Direction.Left) && IsMoveTileEmpty(Direction.Left, _originTile + Direction.Left + Direction.Left))
                    {
                        if (!_board.GetTile(0).IsEmpty() && _board.GetPiece(0).Team == _piece.Team && !_board.GetPiece(0).HasMoved)
                        {
                            Move move = new Move(_originTile + Direction.Left + Direction.Left, _originTile, SpecialMove.CASTLING);
                            _legalMoves.Add(move);
                        }
                    }
                }
            }

        }

        private void CalculateKnightMoves(int direction)
        {
            int tempLocation = _originTile;

            if (IsMoveInsideBounds(direction, tempLocation))
            {
                tempLocation += direction;
                if (IsMoveInsideBounds(direction, tempLocation))
                {
                    tempLocation += direction;

                    if (direction == Direction.Up | direction == Direction.Down)
                    {
                        if (IsMoveInsideBounds(Direction.Left, tempLocation) && IsMoveTileEmptyOrOcuppiedByEnemy(Direction.Left, tempLocation))
                        {
                            Move move = new Move(Direction.Left + tempLocation, _originTile, SpecialMove.NONE);
                            _legalMoves.Add(move);
                        }
                        if (IsMoveInsideBounds(Direction.Right, tempLocation) && IsMoveTileEmptyOrOcuppiedByEnemy(Direction.Right, tempLocation))
                        {
                            Move move = new Move(Direction.Right + tempLocation, _originTile, SpecialMove.NONE);
                            _legalMoves.Add(move);
                        }
                    }
                    if (direction == Direction.Left | direction == Direction.Right)
                    {
                        if (IsMoveInsideBounds(Direction.Up, tempLocation) && IsMoveTileEmptyOrOcuppiedByEnemy(Direction.Up, tempLocation))
                        {
                            Move move = new Move(Direction.Up + tempLocation, _originTile, SpecialMove.NONE);
                            _legalMoves.Add(move);
                        }
                        if (IsMoveInsideBounds(Direction.Down, tempLocation) && IsMoveTileEmptyOrOcuppiedByEnemy(Direction.Down, tempLocation))
                        {
                            Move move = new Move(Direction.Down + tempLocation, _originTile, SpecialMove.NONE);
                            _legalMoves.Add(move);
                        }
                    }
                }
            }
        }

        private void AddKnightMoves()
        {
            int[] knightStartingDirections = new int[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

            foreach (int knightStartingDirection in knightStartingDirections)
            {
                CalculateKnightMoves(knightStartingDirection);
            }
        }

        private void AddSlidingMove(int direction)
        {
            for (int moveLocation = _originTile; IsMoveInsideBounds(direction, moveLocation) && IsMoveTileEmptyOrOcuppiedByEnemy(direction, moveLocation); moveLocation += direction)
            {
                Move move = new Move(direction + moveLocation, _originTile, SpecialMove.NONE);
                _legalMoves.Add(move);
                if (IsMoveTileOcuppiedByEnemy(direction, moveLocation))
                {
                    break;
                }
            }
        }

        private void AddRookMoves()
        {
            int[] rookDirections = new int[4] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

            foreach (int rookDirection in rookDirections)
            {
                AddSlidingMove(rookDirection);
            }
        }

        private void AddBishopMoves()
        {
            int[] bishopDirections = new int[4] { Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight };

            foreach (int bishopDirection in bishopDirections)
            {
                AddSlidingMove(bishopDirection);
            }
        }

        private void AddPawnMoves()
        {
            if (_piece.Team == Team.WHITE)
            {
                //Comprueba si se puede mover un paso o dos pasos hacia delante
                if (IsMoveInsideBounds(Direction.Up, _originTile) &&
                    IsMoveTileEmpty(Direction.Up, _originTile))
                {
                    int moveLocation = _originTile + Direction.Up;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }


                    if (!_piece.HasMoved &&
                        IsMoveInsideBounds(Direction.Up, moveLocation) &&
                        IsMoveTileEmpty(Direction.Up, moveLocation))
                    {
                        moveLocation += Direction.Up;
                        Move move = new Move(moveLocation, _originTile, SpecialMove.DOBLE_STEP);
                        _legalMoves.Add(move);
                    }
                }

                //Comprueba si puede capturar en diagonal
                if (IsMoveInsideBounds(Direction.UpLeft, _originTile) &&
                    IsMoveTileOcuppiedByEnemy(Direction.UpLeft, _originTile))
                {
                    int moveLocation = _originTile + Direction.UpLeft;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }
                }

                if (IsMoveInsideBounds(Direction.UpRight, _originTile) &&
                    IsMoveTileOcuppiedByEnemy(Direction.UpRight, _originTile))
                {
                    int moveLocation = _originTile + Direction.UpRight;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }
                }

                //Comprueba si se puede realizar la captura al paso
                if (CanEnPassant(Direction.UpLeft, _originTile))
                {
                    int moveLocation = _originTile + Direction.UpLeft;
                    Move move = new Move(moveLocation, _originTile, SpecialMove.EN_PASSANT);
                    _legalMoves.Add(move);
                }

                if (CanEnPassant(Direction.UpRight, _originTile))
                {
                    int moveLocation = _originTile + Direction.UpRight;
                    Move move = new Move(moveLocation, _originTile, SpecialMove.EN_PASSANT);
                    _legalMoves.Add(move);
                }
            }
            else
            {
                //Comprueba si se puede mover un paso o dos pasos hacia delante
                if (IsMoveInsideBounds(Direction.Down, _originTile) &&
                    IsMoveTileEmpty(Direction.Down, _originTile))
                {
                    int moveLocation = _originTile + Direction.Down;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }

                    if (!_piece.HasMoved &&
                        IsMoveInsideBounds(Direction.Down, moveLocation) &&
                        IsMoveTileEmpty(Direction.Down, moveLocation))
                    {
                        moveLocation += Direction.Down;
                        Move move = new Move(moveLocation, _originTile, SpecialMove.DOBLE_STEP);
                        _legalMoves.Add(move);
                    }
                }

                //Comprueba si puede capturar en diagonal
                if (IsMoveInsideBounds(Direction.DownLeft, _originTile) &&
                    IsMoveTileOcuppiedByEnemy(Direction.DownLeft, _originTile))
                {
                    int moveLocation = _originTile + Direction.DownLeft;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }
                }

                if (IsMoveInsideBounds(Direction.DownRight, _originTile) &&
                    IsMoveTileOcuppiedByEnemy(Direction.DownRight, _originTile))
                {
                    int moveLocation = _originTile + Direction.DownRight;
                    if (IsPromotionMove(moveLocation))
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.PROMOTION);
                        _legalMoves.Add(move);
                    }
                    else
                    {
                        Move move = new Move(moveLocation, _originTile, SpecialMove.NONE);
                        _legalMoves.Add(move);
                    }
                }

                //Comprueba si se puede realizar la captura al paso
                if (CanEnPassant(Direction.DownLeft, _originTile))
                {
                    int moveLocation = _originTile + Direction.DownLeft;
                    Move move = new Move(moveLocation, _originTile, SpecialMove.EN_PASSANT);
                    _legalMoves.Add(move);
                }

                if (CanEnPassant(Direction.DownRight, _originTile))
                {
                    int moveLocation = _originTile + Direction.DownRight;
                    Move move = new Move(moveLocation, _originTile, SpecialMove.EN_PASSANT);
                    _legalMoves.Add(move);
                }
            }
        }

        private bool IsPromotionMove(int moveLocation)
        {
            if (_piece.Team == Team.WHITE)
            {
                foreach (int promotionTile in Board.TopBounds)
                {
                    if (promotionTile == moveLocation) return true;
                }
            }
            else
            {
                foreach (int promotionTile in Board.BottomBounds)
                {
                    if (promotionTile == moveLocation) return true;
                }
            }
            return false;
        }

        private bool CanEnPassant(int direction, int tileLocation)
        {
            if (!IsMoveInsideBounds(direction, tileLocation)) return false;
            if (!IsMoveTileEmpty(direction, tileLocation)) return false;
            if (!IsInsideEnPassantBounds(tileLocation)) return false;

            switch (direction)
            {
                case Direction.UpLeft:
                    return IsMoveTileOcuppiedByEnemy(Direction.Left, tileLocation) &&
                           _board.GetTile(Direction.Left + tileLocation).Piece.IsAvailableForEnPassant;
                case Direction.UpRight:
                    return IsMoveTileOcuppiedByEnemy(Direction.Right, tileLocation) &&
                           _board.GetTile(Direction.Right + tileLocation).Piece.IsAvailableForEnPassant;
                case Direction.DownLeft:
                    return IsMoveTileOcuppiedByEnemy(Direction.Left, tileLocation) &&
                           _board.GetTile(Direction.Left + tileLocation).Piece.IsAvailableForEnPassant;
                case Direction.DownRight:
                    return IsMoveTileOcuppiedByEnemy(Direction.Right, tileLocation) &&
                           _board.GetTile(Direction.Right + tileLocation).Piece.IsAvailableForEnPassant;
            }
            return false;
        }

        private bool IsInsideEnPassantBounds(int tileLocation)
        {
            if (_piece.Team == Team.WHITE)
            {
                int[] whiteBounds = new int[] { 24, 25, 26, 27, 28, 29, 30, 31 };
                foreach (int bound in whiteBounds)
                {
                    if (bound == tileLocation) return true;
                }
            }
            else
            {
                int[] blackBounds = new int[] { 32, 33, 34, 35, 36, 37, 38, 39 };
                foreach (int bound in blackBounds)
                {
                    if (bound == tileLocation) return true;
                }
            }
            return false;
        }

        private bool IsMoveTileOcuppiedByEnemy(int direction, int location)
        {
            return !_board.GetTile(direction + location).IsEmpty() &&
                    _board.GetTile(direction + location).Piece.Team != _piece.Team;
        }

        private bool IsMoveTileEmptyOrOcuppiedByEnemy(int direction, int location)
        {
            if (_board.GetTile(direction + location).IsEmpty()) return true;
            else if (_board.GetTile(direction + location).Piece.Team != _piece.Team) return true;
            else return false;
        }

        private bool IsMoveTileEmpty(int direction, int location)
        {
            return _board.GetTile(direction + location).IsEmpty();
        }

        private static bool IsMoveInsideBounds(int direction, int location)
        {
            int pieceLocation = location;

            switch (direction)
            {
                case Direction.Up:

                    foreach (int bound in Board.TopBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.Down:

                    foreach (int bound in Board.BottomBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.Left:

                    foreach (int bound in Board.LeftBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.Right:

                    foreach (int bound in Board.RightBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.UpLeft:

                    foreach (int bound in Board.TopBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    foreach (int bound in Board.LeftBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.UpRight:

                    foreach (int bound in Board.RightBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    foreach (int bound in Board.TopBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.DownLeft:

                    foreach (int bound in Board.BottomBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    foreach (int bound in Board.LeftBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;

                case Direction.DownRight:

                    foreach (int bound in Board.BottomBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    foreach (int bound in Board.RightBounds)
                    {
                        if (pieceLocation == bound) return false;
                    }
                    return true;
            }
            return false;
        }
    }
}
