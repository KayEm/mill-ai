using Mills.Controllers;
using Mills.Models;
using Mills.Services;
using System.Windows;
using System.Windows.Input;

namespace Mills
{
    public partial class MainWindow : Window
    {
        private const string newMillMessage = "Player {0} has formed a mill. Remove one piece from the opponent.";
        private const string cannotRemovePieceMessage = "That piece is in a mill and cannot be removed.";
        private const string gameOverMessage = "Game over! Player {0} has won the game.";

        public GameModel GameModel;

        private GameController gameController;
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
            var boardService = new BoardService();
            var boardModel = new BoardModel(boardService);
            
            GameModel = new GameModel(boardModel, boardService);
            gameController = new GameController(GameModel);

            boardController = new BoardController(boardModel);
            
            var rendererModel = new RendererModel(BoardCanvas);
            rendererController = new RendererController(rendererModel);

            boardModel.NewPieceAdded += rendererController.DrawNewPiece;
            boardModel.NewPieceAdded += gameController.IncreasePieceCount;
            boardModel.PieceRemoved += rendererController.DeletePiece;
            boardModel.PieceRemoved += gameController.DecreaseOpponentPieceCount;
            boardModel.PieceMoved += rendererController.MovePiece;
            boardModel.SelectionChanged += rendererController.ChangeSelection;
            GameModel.TurnTaken += rendererController.UpdateRendererModel;

            DataContext = rendererModel;
            gameController.StartGame();
        }

        private void BoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (gameController.IsGameOver())
            {
                return;
            }

            var currentPoint = Mouse.GetPosition(BoardCanvas);

            if (gameController.CanRemovePiece(currentPoint))
            {
                var pointModel = GameModel.BoardModel.GetPointModelByPosition(currentPoint);
                if (boardController.IsPieceInMill(pointModel.Piece))
                {
                    rendererController.ShowMessage(cannotRemovePieceMessage);
                    return;
                }

                boardController.RemoveOpponentPiece(currentPoint, GameModel.OpponentPlayer);

                if (gameController.IsGameOver())
                {
                    rendererController.ShowMessage(string.Format(gameOverMessage, GameModel.CurrentPlayer.Number));
                    return;
                }

                gameController.TakeTurn();
                return;
            }

            if (gameController.CanAddNewPiece(currentPoint))
            {
                var newPiece = boardController.AddNewPiece(currentPoint, GameModel.CurrentPlayer);
                boardController.CheckNewMill(GameModel.CurrentPlayer, newPiece);

                if (gameController.HasMill)
                {
                    rendererController.ShowMessage(string.Format(newMillMessage, GameModel.CurrentPlayer.Number));
                    return;
                }

                gameController.TakeTurn();
                return;
            }

            if (gameController.CanSelectPiece(currentPoint))
            {
                boardController.ChangeSelection(currentPoint, GameModel.CurrentPlayer, true);
                return;
            }

            if (gameController.CanMovePiece(currentPoint))
            {
                var newPiece = boardController.MoveSelectedPiece(currentPoint);
                boardController.ChangeSelection(currentPoint, GameModel.CurrentPlayer, false);

                boardController.CheckNewMill(GameModel.CurrentPlayer, newPiece);

                if (gameController.HasMill)
                {
                    rendererController.ShowMessage(string.Format(newMillMessage, GameModel.CurrentPlayer.Number));
                    return;
                }

                gameController.TakeTurn();
            }
        }
    }
}
