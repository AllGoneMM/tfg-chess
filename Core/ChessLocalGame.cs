using Chess.Models.Enumerations;
using Chess.Exceptions;
using Chess.Models;
using Chess.UI;

namespace Chess.Core
{
    public static class ChessLocalGame
    {
        public static void Start(string fen = "", Team turn = Team.WHITE)
        {
            ChessGame chess;
            if (string.IsNullOrEmpty(fen))
            {
                chess = new ChessGame();
            }
            else
            {
                chess = new ChessGame(fen, turn);
            }
            ChessUI UI = new ChessUI();

            string errorMessage = "";
            string selectTile = "";
            string moveTile = "";

            while (chess.Status == Status.IN_PROGRESS)
            {
                try
                {
                    UI.ShowSelectionLayout(chess.Board.GetTileLayout(), chess.Turn, errorMessage);
                    selectTile = Console.ReadLine();
                    selectTile = selectTile.ToUpper();


                    List<Move> legalMoves = chess.SelectTile(selectTile);
                    errorMessage = "";
                    bool validInput = false;

                    do
                    {
                        UI.ShowMoveLayout(chess.Board.GetTileLayout(), chess.Turn, legalMoves, errorMessage);
                        moveTile = Console.ReadLine();
                        moveTile = moveTile.ToUpper();
                        errorMessage = "";

                        try
                        {
                            if (moveTile == "X")
                            {
                                chess.DeselectTile();
                                errorMessage = "";
                                validInput = true;
                            }
                            else
                            {
                                chess.MovePiece(moveTile);
                                while (chess.PromotionAvailable)
                                {
                                    UI.ShowPromotion(chess.Board.GetTileLayout(), errorMessage);
                                    string promoteFen = Console.ReadLine();
                                    try
                                    {
                                        chess.Promote(promoteFen);
                                        errorMessage = "";
                                    }
                                    catch (ArgumentException)
                                    {
                                        errorMessage = $"\"{promoteFen}\" no es una pieza válida";
                                    }
                                }

                                validInput = true;
                            }
                        }
                        catch (IllegalMoveException)
                        {
                            errorMessage = $"No se puede realizar un movimiento a \"{moveTile}\"";
                        }
                        catch (ArgumentException)
                        {
                            errorMessage = $"La casilla \"{selectTile}\" no existe";
                        }

                    } while (!validInput);

                }
                catch (ArgumentException)
                {
                    errorMessage = $"La casilla \"{selectTile}\" no existe";
                }
                catch (IllegalTileException)
                {
                    errorMessage = $"La casilla \"{selectTile}\" está vacía o contiene una pieza enemiga";
                }
                catch (NoLegalMovesException)
                {
                    errorMessage = $"No hay ningún movimiento disponible en la casilla \"{selectTile}\"";
                }
            }
            UI.ShowLayout(chess.Board.GetTileLayout());
            if (chess.Status == Status.WIN_WHITE)
            {
                Console.WriteLine("VICTORIA BLANCAS");
            }
            else Console.WriteLine("VICTORIA NEGRAS");
            Console.ReadKey();
        }
    }
}
