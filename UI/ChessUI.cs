using Chess.Models;
using Chess.Models.Enumerations;
using System.Text;

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
                Console.Write(errorMessage + "\n");
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

        public string GetSelectionLayout(Tile[] layout, Team turn, string errorMessage = "")
        {
            StringBuilder builder = new StringBuilder();
            string turnString;
            if (turn == Team.WHITE)
            {
                turnString = "Turno: ♙";
            }
            else
            {
                turnString = "Turno: ♟";
            }
            builder.Append("    A   B   C   D   E   F   G   H\n");
            builder.Append("  +===============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    builder.Append($"{rowNumber} ");
                    rowNumber--;
                }
                builder.Append("|");
                if (layout[i].Piece != null)
                {
                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        builder.Append(" " + layout[i].Piece.GetFenRepresentation());
                    }
                    else
                    {
                        builder.Append(" " + layout[i].Piece.GetFenRepresentation() + " ");
                    }

                }
                else
                {
                    builder.Append("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    builder.Append($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    builder.Append($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            builder.Append($"    A   B   C   D   E   F   G   H\n{turnString}");

            builder.Append("/" + errorMessage);
            builder.Append("/Introduce la casilla que quieras seleccionar ({1-8}{A-H}): ");
            return builder.ToString();
        }

        public string GetMoveLayout(Tile[] layout, Team turn, List<Move> moves, string errorMessage = "")
        {
            StringBuilder builder = new StringBuilder();
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
            builder.Append("    A   B   C   D   E   F   G   H\n");
            builder.Append("  +==============================+\n");
            int rowNumber = 8;
            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    builder.Append($"{rowNumber} ");
                    rowNumber--;
                }
                builder.Append("|");
                if (layout[i].Piece != null)
                {

                    if (layout[i].Piece.Team == Team.BLACK && layout[i].Piece.Type == Chess.Models.Enumerations.Type.PAWN)
                    {
                        if (legalMoves.Contains(i))
                        {
                            builder.Append(" X ");
                        }
                        else
                        {
                            builder.Append(" " + layout[i].Piece.GetFenRepresentation());
                        }

                    }
                    else
                    {
                        if (legalMoves.Contains(i))
                        {
                            builder.Append(" X ");
                        }
                        else
                        {
                            builder.Append(" " + layout[i].Piece.GetFenRepresentation() + " ");
                        }
                    }
                }
                else if (legalMoves.Contains(i))
                {
                    builder.Append(" ⏹");
                }
                else
                {
                    builder.Append("   ");
                }
                if (i % 8 == 7 && i != 63)
                {
                    builder.Append($"| {rowNumber + 1}\n  +---+---+---+---+---+---+---+---+\n");
                }
                if (i == 63)
                {
                    builder.Append($"| {rowNumber + 1}\n  +===============================+\n");
                }
            }
            builder.Append($"    A   B   C   D   E   F   G   H\n\n{turnString}\n");
            builder.Append("/" + errorMessage);

            builder.Append("/Introduce la casilla a la que te quieras desplazar ({1-8}{A-H}) o \"X\" para deseleccionar: ");
            return builder.ToString();
        }
    }
}
