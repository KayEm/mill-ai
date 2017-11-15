using Mills.Models;
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
        
        public PieceModel AddNewPiece(Point position, PlayerModel player)
        {
            var pointModel = GetPointModelByPosition(position);
            if (pointModel == null || pointModel.Piece != null)
            {
                return null;
            }

            var pieceModel = new PieceModel() { Color = player.Color };
            boardModel.PlaceNewPiece(pieceModel, pointModel);

            return pieceModel;
        }
        
        public void RemoveOpponentPiece(Point position, PlayerModel opponentPlayer)
        {
            var pointModel = GetPointModelByPosition(position);
            if (pointModel?.Piece == null)
            {
                return;
            }

            boardModel.RemovePiece(pointModel);
        }

        public void ChangeSelection(Point point, PlayerModel player, bool isSelected)
        {
            var newSelectedPoint = GetPointModelByPosition(point);
            if (newSelectedPoint?.Piece == null)
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

        public PieceModel MoveSelectedPiece(Point currentPoint)
        {
            var selectedPoint = GetSelectedPoint();
            if (selectedPoint?.Piece == null)
            {
                return null;
            }

            var newPoint = GetPointModelByPosition(currentPoint);
            if (newPoint == null)
            {
                return null;
            }

            if (newPoint.Piece != null)
            {
                return null;
            }

            if (!newPoint.IsNeighbor(selectedPoint))
            {
                return null;
            }

            boardModel.MovePiece(selectedPoint, newPoint);

            return newPoint.Piece;
        }
        
        public bool IsPieceInMill(PieceModel piece)
        {
            if (piece == null)
            {
                return false;
            }

            var possibleMills = boardModel.Mills.Where(m => m.Any(p => p.Piece == piece));
            return possibleMills.Any(m => m.All(p => p.Piece != null && p.Piece.Color == piece.Color));
        }

        public PointModel GetSelectedPoint()
        {
            return boardModel.Points.Where(p => p.Piece?.IsSelected == true).FirstOrDefault();
        }

        public PointModel GetPointModelByPosition(Point position)
        {
            return boardModel.Points.Where(p => p.Bounds.Contains(position)).FirstOrDefault();
        }
    }
}
