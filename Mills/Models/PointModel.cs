using System.Windows;

namespace Mills.Models
{
    public class PointModel
    {        
        public string X { get; set; }

        public int Y { get; set; }
        
        public Rect Bounds { get; set; }
        
        public PieceModel Piece { get; set; }
    }
}
