using Chess.Models.Enumerations;
using Chess.Core;
using Chess.Utils;


namespace Chess;
public class Program
{
    public static void Main(string[] args)
    {
        StartMenu();
    }

    public static void StartMenu()
    {
        string option = "0";
        bool exit = false;
        do
        {
            Console.Clear();
            Console.WriteLine("MENU\n");
            Console.WriteLine("1 - Jugar contra IA\n");
            Console.WriteLine("2 - Jugar en local 1 vs 1\n");
            Console.WriteLine("3 - IA vs IA\n");
            Console.WriteLine("4 - Iniciar servidor de ajedrez (Beta)\n");
            Console.WriteLine("5 - Conectar a servidor de ajedrez (Beta)\n");
            Console.WriteLine("6 - Salir\n");
            Console.Write("\nIntroduce una opción (1-6): ");
            option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    StartAIGame();
                    break;
                case "2":
                    StartLocalGame();
                    break;
                case "3":
                    ChessAIVSAIGame.Start();
                    Console.ReadKey();
                    break;
                case "4":
                    ChessServer.Start();
                    break;
                case "5":
                    ChessClient.Start();
                    break;
                case "6":
                    exit = true;
                    break;
            }

        } while (!exit);
    }

    public static void StartLocalGame()
    {
        bool startWithFen = false;
        bool startWithoutFen = false;
        string fen = "STARTING FEN";
        string turn = "TURN STRING";
        Team team = Team.NONE;
        do
        {
            Console.Clear();
            Console.Write("Pulsa ENTER para comenzar una nueva partida o introduce una cadena FEN: ");
            fen = Console.ReadLine();
            if (FenConverter.IsFenValid(fen))
            {
                startWithFen = true;
            }
            else if (string.IsNullOrEmpty(fen))
            {
                startWithoutFen = true;
            }
        } while (!startWithFen && !startWithoutFen);

        if (startWithFen)
        {
            do
            {
                Console.Clear();
                Console.Write("Introduce el equipo que empezará jugando (Blancas: b | Negras: n): ");
                turn = Console.ReadLine();
                turn = turn.ToLower();
                if (turn == "b")
                {
                    team = Team.WHITE;
                }
                else if (turn == "n")
                {
                    team = Team.BLACK;
                }

            } while (team == Team.NONE);
        }

        ChessLocalGame.Start(fen, team);
    }

    public static void StartAIGame()
    {
        bool startWithFen = false;
        bool startWithoutFen = false;
        string fen = "STARTING FEN";
        string turn = "TURN STRING";
        Team team = Team.NONE;
        Team playerTeam = Team.NONE;
        do
        {
            Console.Clear();
            Console.Write("Pulsa ENTER para comenzar una nueva partida o introduce una cadena FEN: ");
            fen = Console.ReadLine();
            if (FenConverter.IsFenValid(fen))
            {
                startWithFen = true;
            }
            else if (string.IsNullOrEmpty(fen))
            {
                startWithoutFen = true;
            }
        } while (!startWithFen && !startWithoutFen);

        if (startWithFen)
        {
            do
            {
                Console.Clear();
                Console.Write("Introduce el equipo que empezará jugando (Blancas: b | Negras: n): ");
                turn = Console.ReadLine();
                turn = turn.ToLower();
                if (turn == "b")
                {
                    team = Team.WHITE;
                }
                else if (turn == "n")
                {
                    team = Team.BLACK;
                }

            } while (team == Team.NONE);
        }
        while (playerTeam == Team.NONE)
        {
            Console.Clear();
            Console.Write("Introduce el equipo con el que vas a jugar (Blancas: b | Negras: n): ");
            turn = Console.ReadLine();
            turn = turn.ToLower();
            if (turn == "b")
            {
                playerTeam = Team.WHITE;
            }
            else if (turn == "n")
            {
                playerTeam = Team.BLACK;
            }
        }
        ChessAIGame.Start(playerTeam, fen, team);
    }
}