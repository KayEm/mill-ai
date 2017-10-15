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
            var board = new BoardModel(players, points);

            boardController = new BoardController(board);
            boardController.StartGame();

            rendererController = new RendererController(BoardCanvas);
            board.NewPiecePlaced += rendererController.DrawNewPiece;
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(BoardCanvas);

            if (boardController.PlaceNewPiece(currentPoint))
            {
                boardController.TakeTurn();
            }
        }

        /// <summary>
        /// Creates initial points for the empty board.
        /// </summary>
        /// <returns>List of points for the empty board.</returns>
        private List<PointModel> CreateInitialPoints()
        {
            // Object pool pattern
            var points = new List<PointModel>()
            {
                new PointModel() { PositionX = "a", PositionY = 1, Bounds = new Rect() { X = -10, Y = 290, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "a", PositionY = 4, Bounds = new Rect() { X = -10, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "a", PositionY = 7, Bounds = new Rect() { X = -10, Y = -10, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "b", PositionY = 2, Bounds = new Rect() { X = 40, Y = 240, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "b", PositionY = 4, Bounds = new Rect() { X = 40, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "b", PositionY = 6, Bounds = new Rect() { X = 40, Y = 40, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "c", PositionY = 3, Bounds = new Rect() { X = 90, Y = 190, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "c", PositionY = 4, Bounds = new Rect() { X = 90, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "c", PositionY = 5, Bounds = new Rect() { X = 90, Y = 90, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "d", PositionY = 1, Bounds = new Rect() { X = 140, Y = 290, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "d", PositionY = 2, Bounds = new Rect() { X = 140, Y = 240, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "d", PositionY = 3, Bounds = new Rect() { X = 140, Y = 190, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "d", PositionY = 5, Bounds = new Rect() { X = 140, Y = 90, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "d", PositionY = 6, Bounds = new Rect() { X = 140, Y = 40, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "d", PositionY = 7, Bounds = new Rect() { X = 140, Y = -10, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "e", PositionY = 3, Bounds = new Rect() { X = 190, Y = 190, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "e", PositionY = 4, Bounds = new Rect() { X = 190, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "e", PositionY = 5, Bounds = new Rect() { X = 190, Y = 90, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "f", PositionY = 2, Bounds = new Rect() { X = 240, Y = 240, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "f", PositionY = 4, Bounds = new Rect() { X = 240, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "f", PositionY = 6, Bounds = new Rect() { X = 240, Y = 40, Width = 15, Height = 15  } },

                new PointModel() { PositionX = "g", PositionY = 1, Bounds = new Rect() { X = 290, Y = 290, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "g", PositionY = 4, Bounds = new Rect() { X = 290, Y = 140, Width = 15, Height = 15  } },
                new PointModel() { PositionX = "g", PositionY = 7, Bounds = new Rect() { X = 290, Y = -10, Width = 15, Height = 15  } },
            };

            return points;
        }
    }
}
