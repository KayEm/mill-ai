using Mills.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mills.Controllers
{
    public class RendererController
    {
        private const string nextPlayerMessage = "Player {0}'s move!";
        private BoardViewModel boardViewModel;

        public event Action<string> NotifyUser;

        public RendererController(BoardViewModel boardViewModel)
        {
            this.boardViewModel = boardViewModel;
        }
        
        public void DrawNewPiece(PointModel point)
        {
            Ellipse ellipse = CreatePiece(point.Piece.Color, point.Bounds);
            boardViewModel.Canvas.Children.Add(ellipse);
        }

        public void DeletePiece(PointModel point)
        {
            Ellipse ellipse = FindEllipseInCanvas(point);

            if (ellipse == null)
            {
                return;
            }

            boardViewModel.Canvas.Children.Remove(ellipse);
            ellipse = null;
        }

        public void ChangeSelection(PointModel point, bool isSelected)
        {
            Ellipse ellipse = FindEllipseInCanvas(point);

            if (ellipse == null)
            {
                return;
            }

            var color = isSelected ? Colors.Red : point.Piece.Color;
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = color };
            ellipse.Fill = fillBrush;
        }

        public void MovePiece(PointModel oldPoint, PointModel newPoint)
        {
            Ellipse ellipse = FindEllipseInCanvas(oldPoint);
            MovePiece(ellipse, newPoint.Bounds);
        }

        public void UpdateRendererModel(PlayerModel currentPlayer)
        {
            boardViewModel.CurrentPlayerColor = new SolidColorBrush() { Color = currentPlayer.Color };
            NotifyUser(string.Format(nextPlayerMessage, currentPlayer.Number));
        }

        private Ellipse CreatePiece(Color color, Rect bounds)
        {
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = color };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };

            return new Ellipse()
            {
                Height = 50,
                Width = 50,
                Margin = new Thickness(bounds.X, bounds.Y, 50, 50),
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }

        private void MovePiece(Ellipse ellipse, Rect newBounds)
        {
            ellipse.Margin = new Thickness(newBounds.X, newBounds.Y, 50, 50);
        }

        private Ellipse FindEllipseInCanvas(PointModel point)
        {
            return boardViewModel.Canvas.Children.OfType<Ellipse>()
                .Where(e => e.Margin.Top == point.Bounds.Y && e.Margin.Left == point.Bounds.X)
                .FirstOrDefault();            
        }
    }
}
