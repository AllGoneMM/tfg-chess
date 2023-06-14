using Chess.Models.Enumerations;
using Type = Chess.Models.Enumerations.Type;

namespace Chess.Models
{
    public class Piece
    {
        public Team Team { get; set; }

        public Type Type { get; set; }

        public Piece(Team team, Type type)
        {
            this.Team = team;
            this.Type = type;
            this.HasMoved = false;
            this.IsAvailableForEnPassant = false;
            this.NeedsPromotions = false;
        }

        public bool HasMoved { get; set; }

        public bool NeedsPromotions { get; set; }

        public bool IsAvailableForEnPassant { get; set; }

        public string GetFenRepresentation()
        {
            string fenRepresentation = string.Empty;
            if (this.Team == Team.WHITE)
            {
                switch (this.Type)
                {
                    case Type.BISHOP: fenRepresentation = "♗"; break;
                    case Type.KNIGHT: fenRepresentation = "♘"; break;
                    case Type.PAWN: fenRepresentation = "♙"; break;
                    case Type.KING: fenRepresentation = "♔"; break;
                    case Type.QUEEN: fenRepresentation = "♕"; break;
                    case Type.ROOK: fenRepresentation = "♖"; break;
                }
            }
            else
            {
                switch (this.Type)
                {
                    case Type.BISHOP: fenRepresentation = "♝"; break;
                    case Type.KNIGHT: fenRepresentation = "♞"; break;
                    case Type.PAWN: fenRepresentation = "♟"; break;
                    case Type.KING: fenRepresentation = "♚"; break;
                    case Type.QUEEN: fenRepresentation = "♛"; break;
                    case Type.ROOK: fenRepresentation = "♜"; break;
                }
            }

            return fenRepresentation;
        }
    }

}
