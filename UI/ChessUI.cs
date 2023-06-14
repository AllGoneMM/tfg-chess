using Chess.Models;
using Chess.Models.Enumerations;

namespace Chess.UI
{
    public class ChessUI
    {
        public void ShowSelectionLayout(Tile[] layout, Team turn, string errorMessage = "")
        {
            Console.Clear();
            string turnString;
            if (turn == Team.WHITE)
            {
                turnString = "Turno: ♙";
            }
            else
            {
                turnString = "Turno: ♟";
            }
            Console.Write("    A   B   C   D   E   F   G   H\n");
            Console.Write("  +===============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    Console.Write($"{rowNumber} ");
                    rowNumber--;
                }
                Console.Write("|");
                if (layout[i].Piece != null)
                {
                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation());
                    }
                    else
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation() + " ");
                    }

                }
                else
                {
                    Console.Write("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            Console.Write($"    A   B   C   D   E   F   G   H\n\n{turnString}\n");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.Write(errorMessage + "\n");
            }
            Console.Write("Introduce la casilla que quieras seleccionar ({1-8}{A-H}): ");
        }

        public void ShowMoveLayout(Tile[] layout, Team turn, List<Move> moves, string errorMessage = "")
        {
            Console.Clear();
            List<int> legalMoves = new List<int>();
            foreach (Move move in moves)
            {
                legalMoves.Add(move.MoveTile);
            }
            string turnString;
            if (turn == Team.WHITE)
            {
                turnString = "Turno: ♙";
            }
            else
            {
                turnString = "Turno: ♟";
            }
            Console.Write("    A   B   C   D   E   F   G   H\n");
            Console.Write("  +==============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    Console.Write($"{rowNumber} ");
                    rowNumber--;
                }
                Console.Write("|");
                if (layout[i].Piece != null)
                {

                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        if (legalMoves.Contains(i))
                        {
                            Console.Write(" X ");
                        }
                        else
                        {
                            Console.Write(" " + layout[i].Piece.GetFenRepresentation());
                        }

                    }
                    else
                    {
                        if (legalMoves.Contains(i))
                        {
                            Console.Write(" X ");
                        }
                        else
                        {
                            Console.Write(" " + layout[i].Piece.GetFenRepresentation() + " ");
                        }
                    }
                }
                else if (legalMoves.Contains(i))
                {
                    Console.Write(" ⏹");
                }
                else
                {
                    Console.Write("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            Console.Write($"    A   B   C   D   E   F   G   H\n\n{turnString}\n");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.Write(errorMessage+"\n");
            }
            Console.Write("Introduce la casilla a la que te quieras desplazar ({1-8}{A-H}) o \"X\" para deseleccionar: ");
        }
        public void ShowPromotion(Tile[] layout, string errorMessage = "")
        {
            Console.Clear();
            Console.Write("    A   B   C   D   E   F   G   H\n");
            Console.Write("  +===============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    Console.Write($"{rowNumber} ");
                    rowNumber--;
                }
                Console.Write("|");
                if (layout[i].Piece != null)
                {
                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation());
                    }
                    else
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation() + " ");
                    }

                }
                else
                {
                    Console.Write("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            Console.Write($"    A   B   C   D   E   F   G   H\n");
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.Write(errorMessage + "\n");
            }
            Console.Write("Introduce la pieza a la que quieres promover (Reina: Q | Torre: R | Alfil: B | Caballo: N): ");
        }

        public void ShowLayout(Tile[] layout)
        {
            Console.Clear();
            Console.Write("    A   B   C   D   E   F   G   H\n");
            Console.Write("  +===============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    Console.Write($"{rowNumber} ");
                    rowNumber--;
                }
                Console.Write("|");
                if (layout[i].Piece != null)
                {
                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation());
                    }
                    else
                    {
                        Console.Write(" " + layout[i].Piece.GetFenRepresentation() + " ");
                    }

                }
                else
                {
                    Console.Write("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    Console.Write($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            Console.Write($"    A   B   C   D   E   F   G   H\n");
        }
    }
}
