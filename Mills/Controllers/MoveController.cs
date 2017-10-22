using Mills.Models;
using System.Windows;

namespace Mills.Controllers
{
    public class MoveController
    {
        private BoardModel boardModel;

        public MoveController(BoardModel board)
        {
            boardModel = board;
        }

        public void MovePiece(Point currentPoint)
        {
            if (boardModel.IsAnyPieceSelected())
            {

            }
        }
    }
}
