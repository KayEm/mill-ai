using Mills.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mills.Controllers
{
    public class RendererController
    {
        private Canvas canvas;

        public RendererController(Canvas canvasControl)
        {
            canvas = canvasControl;
        }

        public void DrawNewPiece(PieceModel piece)
        {
            Ellipse ellipse = CreateBoardPiece(piece.Color, piece.Position.X, piece.Position.Y);
            canvas.Children.Add(ellipse);
        }

        private Ellipse CreateBoardPiece(Color color, double x, double y)
        {
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = color };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };

            return new Ellipse()
            {
                Height = 20,
                Width = 20,
                Margin = new Thickness(x, y, 25, 25),
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }
    }
}
