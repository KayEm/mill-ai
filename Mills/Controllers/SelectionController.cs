using Mills.Models;
using System.Windows;

namespace Mills.Controllers
{
    public class SelectionController
    {
        private BoardModel boardModel;

        public SelectionController(BoardModel board)
        {
            boardModel = board;
        }

        public void SelectPiece(Point point, PlayerModel player)
        {
            var selectedPoint = boardModel.GetPointModelByPosition(point);
            if (selectedPoint.Piece?.Color == player.Color)
            {
                selectedPoint.Piece.IsSelected = true;
            }
        }
    }
}
