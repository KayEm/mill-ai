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
            var pointModel = boardModel.GetPointModelByPosition(position);
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
            var pointModel = boardModel.GetPointModelByPosition(position);
            if (pointModel?.Piece == null)
            {
                return;
            }

            boardModel.RemovePiece(pointModel);
        }

        public void ChangeSelection(Point point, PlayerModel player, bool isSelected)
        {
            var selectedPoint = boardModel.GetPointModelByPosition(point);
            if (selectedPoint?.Piece == null)
            {
                return;
            }

            if (boardModel.IsAnyPieceSelected())
            {
                boardModel.ChangeSelection(boardModel.GetSelectedPoint(), false);
            }

            boardModel.ChangeSelection(selectedPoint, isSelected);
        }

        public PieceModel MoveSelectedPiece(Point currentPoint)
        {
            var selectedPoint = boardModel.GetSelectedPoint();
            if (selectedPoint?.Piece == null)
            {
                return null;
            }

            var newPoint = boardModel.GetPointModelByPosition(currentPoint);
            if (newPoint == null)
            {
                return null;
            }

            if (newPoint.Piece != null)
            {
                return null;
            }

            boardModel.MovePiece(selectedPoint, newPoint);

            return newPoint.Piece;
        }

        public void CheckNewMill(PlayerModel player, PieceModel newPiece)
        {
            player.HasMill = IsPieceInMill(newPiece);
        }

        public bool IsPieceInMill(PieceModel piece)
        {
            var possibleMills = boardModel.Mills.Where(m => m.Any(p => p.Piece == piece));
            return possibleMills.Any(m => m.All(p => p.Piece != null && p.Piece.Color == piece.Color));
        }
    }
}
