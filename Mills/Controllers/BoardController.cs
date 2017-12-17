using Mills.Models;
using System;
using System.Linq;
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
        
        public void AddNewPiece(PointModel pointModel, PlayerModel player)
        {
            if (pointModel == null || pointModel.Piece != null)
            {
                return;
            }

            var pieceModel = new PieceModel() { Color = player.Color };
            boardModel.PlaceNewPiece(pieceModel, pointModel);
        }
        
        public void RemovePiece(PointModel pointModel)
        {
            if (pointModel?.Piece == null)
            {
                return;
            }

            boardModel.RemovePiece(pointModel);
        }

        public void ChangeSelection(PointModel newSelectedPoint, PlayerModel player, bool isSelected)
        {
            if (newSelectedPoint.Piece == null)
            {
                return;
            }

            var oldSelectedPoint = GetSelectedPoint();
            if (oldSelectedPoint != null)
            {
                boardModel.ChangeSelection(oldSelectedPoint, false);
            }
            
            boardModel.ChangeSelection(newSelectedPoint, isSelected);
        }

        public void MoveSelectedPiece(PointModel newPoint)
        {
            var selectedPoint = GetSelectedPoint();
            if (selectedPoint?.Piece == null || newPoint.Piece != null || !newPoint.IsNeighbor(selectedPoint))
            {
                return;
            }

            boardModel.MovePiece(selectedPoint, newPoint);
        }

        public PointModel GetSelectedPoint()
        {
            return boardModel.Points.Where(p => p.Piece?.IsSelected == true).FirstOrDefault();
        }
    }
}
