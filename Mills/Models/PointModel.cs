using System.Windows;

namespace Mills.Models
{
    /// <summary>
    /// Point or intersection on the board.
    /// </summary>
    public class PointModel
    {
        /// <summary>
        /// Position on the X coordinate, can be from a until g.
        /// </summary>
        public string X { get; set; }

        /// <summary>
        /// Position on thee Y coordinate, can be from 1 until 7.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Bounds of the point on the board canvas.
        /// </summary>
        public Rect Bounds { get; set; }

        /// <summary>
        /// Piece located on the point.
        /// </summary>
        public PieceModel Piece { get; set; }
    }
}
