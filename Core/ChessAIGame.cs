﻿using Chess.Exceptions;
using Chess.Models;
using Chess.Models.Enumerations;
using Chess.UI;
using Stockfish.NET;

namespace Chess.Core
{
    public static class ChessAIGame
    {
        public static void Start(Team playerTeam, string fen = "", Team turn = Team.WHITE)
        {
            IStockfish stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe");
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
                if (chess.Turn == playerTeam)
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
                else
                {
                    string fullFen = chess.GetFullFen();
                    stockfish.SetFenPosition(fullFen);
                    string stockfishMove = stockfish.GetBestMove();
                    string selectStockFishTile = stockfishMove[1].ToString() + stockfishMove[0].ToString();
                    string moveStockGishTile = stockfishMove[3].ToString() + stockfishMove[2].ToString();
                    chess.SelectTile(selectStockFishTile);
                    chess.MovePiece(moveStockGishTile);
                    if (chess.PromotionAvailable)
                    {
                        chess.Promote("q");
                    }
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
