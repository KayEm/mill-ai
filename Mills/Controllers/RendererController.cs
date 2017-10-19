using Mills.Models;
using System.Linq;
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
        private Ellipse playerIndicator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvasControl">Canvas to draw on.</param>
        public RendererController(Canvas canvasControl, Ellipse currentPlayerIndicator)
        {
            canvas = canvasControl;
            playerIndicator = currentPlayerIndicator;
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

        public void DeletePiece(PointModel point)
        {
            Ellipse ellipse = FindEllipseInCanvas(point);

            if (ellipse == null)
            {
                return;
            }

            canvas.Children.Remove(ellipse);
            ellipse = null;
        }

        public void UpdatePlayerIndicator(PlayerModel currentPlayer)
        {
            playerIndicator.Fill = new SolidColorBrush() { Color = currentPlayer.Color };
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

        private Ellipse FindEllipseInCanvas(PointModel point)
        {
            return canvas.Children.OfType<Ellipse>()
                .Where(e => e.Margin.Top == point.Bounds.Y && e.Margin.Left == point.Bounds.X)
                .FirstOrDefault();            
        }
    }
}
