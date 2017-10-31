using Mills.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Mills.Models
{
    public class BoardModel
    {        
        public BoardModel(IBoardService boardService)
        {
            var initialBoard = boardService.CreateInitialBoard();
            Points = initialBoard.Item1;
            Mills = initialBoard.Item2;
        }
        
        public List<PointModel> Points { get; private set; }

        public List<List<PointModel>> Mills { get; private set; }

        public event Action<PointModel> NewPieceAdded;
        
        public event Action<PointModel> PieceRemoved;

        public event Action<PointModel, bool> SelectionChanged;

        public event Action<PointModel, PointModel> PieceMoved;

        public void PlaceNewPiece(PieceModel piece, PointModel point)
        {
            point.Piece = piece;
            NewPieceAdded(point);
        }
        
        public void RemovePiece(PointModel point)
        {
            point.Piece = null;
            PieceRemoved(point);
        }

        public void ChangeSelection(PointModel point, bool isSelected)
        {
            point.Piece.IsSelected = isSelected;
            SelectionChanged(point, isSelected);
        }

        public void MovePiece(PointModel oldPoint, PointModel newPoint)
        {
            newPoint.Piece = oldPoint.Piece;
            oldPoint.Piece = null;
            PieceMoved(oldPoint, newPoint);
        }

        public bool IsAnyPieceSelected()
        {
            return Points.Any(p => p.Piece?.IsSelected == true);
        }

        public PointModel GetSelectedPoint()
        {
            return Points.Where(p => p.Piece?.IsSelected == true).FirstOrDefault();
        }

        public PointModel GetPointModelByPosition(Point position)
        {
            return Points.Where(p  => p.Bounds.Contains(position)).FirstOrDefault();
        }
    }
}
