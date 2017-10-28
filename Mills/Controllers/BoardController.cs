using Mills.Models;
using System.Windows;

namespace Mills.Controllers
{
    public class BoardController
    {
        BoardModel boardModel;
        
        public BoardController(BoardModel board)
        {
            boardModel = board;
        }
        
        public PieceModel AddNewPiece(Point position, PlayerModel player)
        {
            var pieceModel = new PieceModel() { Color = player.Color };
            var pointModel = boardModel.GetPointModelByPosition(position);
            boardModel.PlaceNewPiece(pieceModel, pointModel);

            return pieceModel;
        }
        
        public void RemoveOpponentPiece(Point position, PlayerModel opponentPlayer)
        {
            var pointModel = boardModel.GetPointModelByPosition(position);
            boardModel.RemovePiece(pointModel);
        }

        public void ChangeSelection(Point point, PlayerModel player, bool isSelected)
        {
            var selectedPoint = boardModel.GetPointModelByPosition(point);
            boardModel.ChangeSelection(selectedPoint, isSelected);
        }

        public PieceModel MovePiece(Point currentPoint)
        {
            var selectedPoint = boardModel.GetSelectedPoint();
            var newPoint = boardModel.GetPointModelByPosition(currentPoint);

            boardModel.MovePiece(selectedPoint, newPoint);

            return newPoint.Piece;
        }
    }
}
