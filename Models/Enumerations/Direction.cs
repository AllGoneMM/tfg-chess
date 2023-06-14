namespace Chess.Models.Enumerations
{
    public class Direction
    {
        public const int Down = +8;

        public const int Up = -8;

        public const int Right = +1;

        public const int Left = -1;

        public const int DownRight = Down + Right;

        public const int DownLeft = Down + Left;

        public const int UpRight = Up + Right;

        public const int UpLeft = Up + Left;
    }
}
