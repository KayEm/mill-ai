using Mills.Models;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    public class BoardController
    {
        BoardModel board;

        public BoardController(BoardModel boardModel)
        {
            board = boardModel;
        }

        public void StartGame()
        {
            board.CurrentPlayer = board.Players.First();
        }

        public void PlaceNewPiece(Point position)
        {
            var piece = new PieceModel() { Color = board.CurrentPlayer.Color, Position = position };
            board.PlaceNewPiece(piece);
        }

        public void TakeTurn()
        {
            if(board.CurrentPlayer == board.Players[0])
            {
                board.CurrentPlayer = board.Players[1];
            }
            else
            {
                board.CurrentPlayer = board.Players[0];
            }
        }
    }
}
