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
            var pieces = new List<PieceModel>();
            var players = new List<PlayerModel>() { new PlayerModel() { Color = Colors.Red }, new PlayerModel() { Color = Colors.Blue } };
            var board = new BoardModel(players, pieces);

            boardController = new BoardController(board);
            boardController.StartGame();

            rendererController = new RendererController(BoardCanvas);
            board.NewPiecePlaced += rendererController.DrawNewPiece;
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentPoint = Mouse.GetPosition(BoardCanvas);
            boardController.PlaceNewPiece(currentPoint);
            boardController.TakeTurn();
        }
    }
}
