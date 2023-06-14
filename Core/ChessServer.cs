using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Enumerations;
using Chess.UI;
using System.IO;
using Chess.Exceptions;
using Chess.Models;

namespace Chess.Core
{
    public class ChessServer
    {
        public static void Start()
        {
            {
                TcpListener listener = null;

                try
                {
                    string ipAddress = "127.0.0.1";
                    int port = 8080;

                    listener = new TcpListener(IPAddress.Parse(ipAddress), port);
                    listener.Start();
                    Console.WriteLine("Servidor iniciado. Esperando conexiones...");

                    TcpClient player1 = listener.AcceptTcpClient();
                    Console.WriteLine("Jugador 1 conectado.");
                    TcpClient player2 = listener.AcceptTcpClient();
                    Console.WriteLine("Jugador 2 conectado.");

                    // Crear una instancia de la clase Chess para representar el juego de ajedrez
                    ChessGame chess = new ChessGame();

                    // Obtener los flujos de red de los jugadores
                    NetworkStream player1Stream = player1.GetStream();
                    NetworkStream player2Stream = player2.GetStream();

                    // Bucle principal del juego
                    while (true)
                    {
                        // Turno del jugador 1
                        Console.WriteLine("Es el turno del Jugador 1.");
                        SendTurnMessage(player1Stream, chess);
                        List<Move> moves = ReceiveTileSelection(player1Stream, chess);
                        SendMoveMessage(player1Stream, chess, moves);
                        RecieveMoveSelection(player1Stream, chess, moves);


                        // Verificar el estado del juego después del movimiento del Jugador 1
                        if (IsGameOver(chess))
                        {
                            string winner = "";
                            if (chess.Turn == Team.WHITE)
                            {
                                winner = "VICTORIA BLANCAS";
                            }
                            else
                            {
                                winner = "VICTORIA NEGRAS";
                            }
                            SendGameEndMessage(player1Stream, player2Stream, winner);
                        }

                        // Turno del jugador 2
                        Console.WriteLine("Es el turno del Jugador 2.");
                        SendTurnMessage(player2Stream, chess);
                        moves = ReceiveTileSelection(player2Stream, chess);
                        SendMoveMessage(player2Stream, chess, moves);
                        RecieveMoveSelection(player2Stream, chess, moves);
                        // Verificar el estado del juego después del movimiento del Jugador 2
                        if (IsGameOver(chess))
                        {
                            string winner = "";
                            if (chess.Turn == Team.WHITE)
                            {
                                winner = "VICTORIA BLANCAS";
                            }
                            else
                            {
                                winner = "VICTORIA NEGRAS";
                            }
                            SendGameEndMessage(player1Stream, player2Stream, winner);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    listener?.Stop();
                }
            }

            static void SendTurnMessage(NetworkStream stream, ChessGame chess, string errorMessage = "")
            {
                ChessUI chessUI = new ChessUI();

                string message = chessUI.GetSelectionLayout(chess.Board.GetTileLayout(), chess.Turn, errorMessage);
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            static void SendMoveMessage(NetworkStream stream, ChessGame chess, List<Move> moves, string errorMessage = "")
            {
                ChessUI chessUI = new ChessUI();

                string message = chessUI.GetMoveLayout(chess.Board.GetTileLayout(), chess.Turn, moves, errorMessage);
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Mensaje con posibles movimientos enviado");
            }

            static List<Move> ReceiveTileSelection(NetworkStream stream, ChessGame chess)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Seleccionar casilla: " + selectedTile);

                string errorMessage = "";
                bool correctSelectedTile = false;
                List<Move> moves = new List<Move>();
                do
                {
                    try
                    {

                        moves = chess.SelectTile(selectedTile);
                        Console.WriteLine("Selección correcta");
                        correctSelectedTile = true;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Selección incorrecta, mandando mensaje de error");
                        errorMessage = $"La casilla \"{selectedTile}\" no existe";
                        SendTurnMessage(stream, chess, errorMessage);
                        buffer = new byte[1024];
                        Console.WriteLine("Leyendo nueva respuesta");
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Respuesta recibida:" + selectedTile);
                    }
                    catch (IllegalTileException)
                    {
                        Console.WriteLine("Selección incorrecta, mandando mensaje de error");
                        errorMessage = $"La casilla \"{selectedTile}\" está vacía o contiene una pieza enemiga";
                        SendTurnMessage(stream, chess, errorMessage);
                        buffer = new byte[1024];
                        Console.WriteLine("Leyendo nueva respuesta");
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Respuesta recibida:" + selectedTile);
                    }
                    catch (NoLegalMovesException)
                    {
                        Console.WriteLine("Selección incorrecta, mandando mensaje de error");
                        errorMessage = $"No hay ningún movimiento disponible en la casilla \"{selectedTile}\"";
                        SendTurnMessage(stream, chess, errorMessage);
                        buffer = new byte[1024];
                        Console.WriteLine("Leyendo nueva respuesta");
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Respuesta recibida:" + selectedTile);
                    }
                } while (!correctSelectedTile);
                Console.WriteLine("Mandando mensaje de confirmación");
                SendTurnMessage(stream, chess);
                return moves;

            }

            static bool IsGameOver(ChessGame chess)
            {
                if (chess.Status == Status.IN_PROGRESS)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            static void SendGameEndMessage(NetworkStream stream1, NetworkStream stream2, string winner)
            {
                string message = "El juego ha terminado. Ganador: " + winner;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream1.Write(data, 0, data.Length);
                stream2.Write(data, 0, data.Length);
            }

            static void RecieveMoveSelection(NetworkStream player1Stream, ChessGame chess, List<Move> moves)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = player1Stream.Read(buffer, 0, buffer.Length);
                string selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Seleccionar casilla a MOVER: " + selectedTile);

                string errorMessage = "";
                bool correctSelectedTile = false;
                do
                {
                    try
                    {
                        chess.MovePiece(selectedTile);
                        Console.WriteLine("Selección correcta");
                        correctSelectedTile = true;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Selección incorrecta, mandando mensaje de error");
                        errorMessage = $"La casilla \"{selectedTile}\" no existe";
                        SendMoveMessage(player1Stream, chess, moves, errorMessage);
                        buffer = new byte[1024];
                        Console.WriteLine("Leyendo nueva respuesta");
                        bytesRead = player1Stream.Read(buffer, 0, buffer.Length);
                        selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Respuesta recibida:" + selectedTile);
                    }

                    catch (IllegalMoveException)
                    {
                        Console.WriteLine("Selección incorrecta, mandando mensaje de error");
                        errorMessage = $"No se puede realizar un movimiento a \"{selectedTile}\"";
                        SendMoveMessage(player1Stream, chess, moves, errorMessage);
                        buffer = new byte[1024];
                        Console.WriteLine("Leyendo nueva respuesta");
                        bytesRead = player1Stream.Read(buffer, 0, buffer.Length);
                        selectedTile = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Respuesta recibida:" + selectedTile);
                    }
                } while (!correctSelectedTile);
                Console.WriteLine("Movimiento correcto, mandando mensaje de confirmación");
                SendTurnMessage(player1Stream, chess);
            }
        }

    }

}


