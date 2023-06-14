using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    public class ChessClient
    {
        public static void Start()
        {
            try
            {
                string ipAddress = "127.0.0.1";
                int port = 8080;

                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ipAddress), port);
                Console.WriteLine("Conectado al servidor.");

                NetworkStream stream = client.GetStream();

                while (true)
                {
                    // Recibir el mensaje del servidor (indicando el turno o el resultado del juego)
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.Contains("El juego ha terminado"))
                    {
                        break;
                    }

                    //Imprime el tablero
                    string[] messageParts = message.Split('/');
                    foreach (string part in messageParts)
                    {
                        Console.WriteLine($"{part}");
                    }

                    //Envía el movimiento para seleccionar una casilla
                    string selectTile = Console.ReadLine();
                    byte[] data = Encoding.UTF8.GetBytes(selectTile);
                    stream.Write(data, 0, data.Length);

                    bool correctSelectedTile = false;

                    do
                    {
                        buffer = new byte[1024];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageParts = message.Split('/');
                        foreach (string part in messageParts)
                        {
                            Console.WriteLine($"{part}");
                        }

                        if (string.IsNullOrEmpty(messageParts[1]))
                        {
                            correctSelectedTile = true;
                        }
                        else
                        {
                            selectTile = Console.ReadLine();
                            data = Encoding.UTF8.GetBytes(selectTile);
                            stream.Write(data, 0, data.Length);
                        }
                    } while (!correctSelectedTile);



                    // Recibir el mensaje del servidor con los posibles movimientos
                    buffer = new byte[1024];
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.Contains("El juego ha terminado"))
                    {
                        break;
                    }

                    //Imprime el tablero
                    messageParts = message.Split('/');
                    foreach (string part in messageParts)
                    {
                        Console.WriteLine($"{part}");
                    }

                    //Envía el movimiento para seleccionar una casilla
                    selectTile = Console.ReadLine();
                    data = Encoding.UTF8.GetBytes(selectTile);
                    stream.Write(data, 0, data.Length);

                    bool correctMoveTile = false;

                    do
                    {
                        buffer = new byte[1024];
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                        message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageParts = message.Split('/');
                        foreach (string part in messageParts)
                        {
                            Console.WriteLine($"{part}");
                        }

                        if (string.IsNullOrEmpty(messageParts[1]))
                        {
                            correctMoveTile = true;
                        }
                        else
                        {
                            selectTile = Console.ReadLine();
                            data = Encoding.UTF8.GetBytes(selectTile);
                            stream.Write(data, 0, data.Length);
                        }
                    } while (!correctMoveTile);



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
