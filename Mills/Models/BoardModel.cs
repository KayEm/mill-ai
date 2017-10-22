using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Mills.Models
{
    public class BoardModel
    {        
        public BoardModel(List<PointModel> points)
        {
            Points = points;
        }
        
        public List<PointModel> Points { get; private set; }
        
        public event Action<PointModel> NewPieceAdded;
        
        public event Action<PointModel> PieceRemoved;
        
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
        
        public bool IsAnyPieceSelected()
        {
            return Points.Any(p => p.Piece?.IsSelected == true);
        }
        
        public PointModel GetPointModelByPosition(Point position)
        {
            return Points.Where(p  => p.Bounds.Contains(position)).FirstOrDefault();
        }
    }
}
