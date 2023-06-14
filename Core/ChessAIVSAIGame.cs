using Chess.UI;
using Stockfish.NET;

namespace Chess.Core
{
    public static class ChessAIVSAIGame
    {
        public static void Start()
        {
            IStockfish stockfish = new Stockfish.NET.Stockfish(@"stockfish.exe");
            Random random = new Random();
            int depth = random.Next(1, 20);
            stockfish.Depth = depth;
            ChessGame chess = new ChessGame();
            ChessUI chessUI = new ChessUI();
            while (chess.Status == Chess.Models.Enumerations.Status.IN_PROGRESS)
            {
                chessUI.ShowLayout(chess.Board.GetTileLayout());
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
            chessUI.ShowLayout(chess.Board.GetTileLayout());
            if (chess.Status == Chess.Models.Enumerations.Status.WIN_BLACK)
            {
                Console.WriteLine("VICTORIA NEGRAS");
            }
            else
            {
                Console.WriteLine("VICTORIA BLANCAS");
            }
        }
    }
}
