using Mills.Controllers;
using Mills.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Mills
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardModel boardModel;

        private BoardController boardController;
        private RendererController rendererController;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Bootstrapper
            var points = CreateInitialPoints();
            var players = new List<PlayerModel>() { new PlayerModel() { Color = Colors.Red }, new PlayerModel() { Color = Colors.Blue } };
            boardModel = new BoardModel(players, points);

            boardController = new BoardController(boardModel);
            
            rendererController = new RendererController(BoardCanvas, CurrentPlayerIndicator);
            boardModel.NewPiecePlaced += rendererController.DrawNewPiece;
            boardModel.PieceRemoved += rendererController.DeletePiece;
            boardModel.TurnTaken += rendererController.UpdatePlayerIndicator;

            boardController.StartGame();
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(BoardCanvas);

            if (boardModel.IsMill)
            {
                if (boardController.RemoveOpponentPiece(currentPoint))
                {
                    boardController.TakeTurn();
                }

                return;
            }

            if (!boardController.PlaceNewPiece(currentPoint))
            {
                return;
            }

            if (boardController.IsNewMillFormed())
            {
                // todo

                return;
            }

            boardController.TakeTurn();
        }

        /// <summary>
        /// Creates initial points for the empty board.
        /// </summary>
        /// <returns>List of points for the empty board.</returns>
        private List<PointModel> CreateInitialPoints()
        {
            // Object pool pattern
            var a1 = new PointModel() { X = "a", Y = 1, Bounds = new Rect() { X = -10, Y = 290, Width = 15, Height = 15 } };
            var a4 = new PointModel() { X = "a", Y = 4, Bounds = new Rect() { X = -10, Y = 140, Width = 15, Height = 15 } };
            var a7 = new PointModel() { X = "a", Y = 7, Bounds = new Rect() { X = -10, Y = -10, Width = 15, Height = 15 } };

            var b2 = new PointModel() { X = "b", Y = 2, Bounds = new Rect() { X = 40, Y = 240, Width = 15, Height = 15 } };
            var b4 = new PointModel() { X = "b", Y = 4, Bounds = new Rect() { X = 40, Y = 140, Width = 15, Height = 15 } };
            var b6 = new PointModel() { X = "b", Y = 6, Bounds = new Rect() { X = 40, Y = 40, Width = 15, Height = 15 } };

            var c3 = new PointModel() { X = "c", Y = 3, Bounds = new Rect() { X = 90, Y = 190, Width = 15, Height = 15 } };
            var c4 = new PointModel() { X = "c", Y = 4, Bounds = new Rect() { X = 90, Y = 140, Width = 15, Height = 15 } };
            var c5 = new PointModel() { X = "c", Y = 5, Bounds = new Rect() { X = 90, Y = 90, Width = 15, Height = 15 } };

            var d1 = new PointModel() { X = "d", Y = 1, Bounds = new Rect() { X = 140, Y = 290, Width = 15, Height = 15 } };
            var d2 = new PointModel() { X = "d", Y = 2, Bounds = new Rect() { X = 140, Y = 240, Width = 15, Height = 15 } };
            var d3 = new PointModel() { X = "d", Y = 3, Bounds = new Rect() { X = 140, Y = 190, Width = 15, Height = 15 } };
            var d5 = new PointModel() { X = "d", Y = 5, Bounds = new Rect() { X = 140, Y = 90, Width = 15, Height = 15 } };
            var d6 = new PointModel() { X = "d", Y = 6, Bounds = new Rect() { X = 140, Y = 40, Width = 15, Height = 15 } };
            var d7 = new PointModel() { X = "d", Y = 7, Bounds = new Rect() { X = 140, Y = -10, Width = 15, Height = 15 } };

            var e3 = new PointModel() { X = "e", Y = 3, Bounds = new Rect() { X = 190, Y = 190, Width = 15, Height = 15 } };
            var e4 = new PointModel() { X = "e", Y = 4, Bounds = new Rect() { X = 190, Y = 140, Width = 15, Height = 15 } };
            var e5 = new PointModel() { X = "e", Y = 5, Bounds = new Rect() { X = 190, Y = 90, Width = 15, Height = 15 } };

            var f2 = new PointModel() { X = "f", Y = 2, Bounds = new Rect() { X = 240, Y = 240, Width = 15, Height = 15 } };
            var f4 = new PointModel() { X = "f", Y = 4, Bounds = new Rect() { X = 240, Y = 140, Width = 15, Height = 15 } };
            var f6 = new PointModel() { X = "f", Y = 6, Bounds = new Rect() { X = 240, Y = 40, Width = 15, Height = 15 } };

            var g1 = new PointModel() { X = "g", Y = 1, Bounds = new Rect() { X = 290, Y = 290, Width = 15, Height = 15 } };
            var g4 = new PointModel() { X = "g", Y = 4, Bounds = new Rect() { X = 290, Y = 140, Width = 15, Height = 15 } };
            var g7 = new PointModel() { X = "g", Y = 7, Bounds = new Rect() { X = 290, Y = -10, Width = 15, Height = 15 } };

            return new List<PointModel> { a1, a4, a7, b2, b4, b6, c3, c4, c5, d1, d2, d3, d5, d6, d7, e3, e4, e5, f2, f4, f6, g1, g4, g7 };
        }
    }
}
