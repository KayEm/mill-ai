using Mills.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mills.Controllers
{
    /// <summary>
    /// Rendering of the board.
    /// </summary>
    public class RendererController
    {
        private Canvas canvas;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvasControl">Canvas to draw on.</param>
        public RendererController(Canvas canvasControl)
        {
            canvas = canvasControl;
        }

        /// <summary>
        /// Draws new piece on the given point.
        /// </summary>
        /// <param name="point">Point to draw the piece on.</param>
        public void DrawNewPiece(PointModel point)
        {
            Ellipse ellipse = CreatePiece(point.Piece.Color, point.Bounds);
            canvas.Children.Add(ellipse);
        }

        /// <summary>
        /// Create ellipse for the piece.
        /// </summary>
        /// <param name="color">Color of the ellipse.</param>
        /// <param name="bounds">Bounds to place ellipse on.</param>
        /// <returns>Ellipse for the corresponding piece in the given bounds.</returns>
        private Ellipse CreatePiece(Color color, Rect bounds)
        {
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = color };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };

            return new Ellipse()
            {
                Height = 20,
                Width = 20,
                Margin = new Thickness(bounds.X, bounds.Y, 25, 25),
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }
    }
}
