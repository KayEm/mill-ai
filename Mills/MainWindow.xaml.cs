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

        private GameModel gameModel;

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
            
            gameModel = new GameModel(boardService);
            gameController = new GameController(gameModel);

            var boardModel = new BoardModel(boardService);
            boardController = new BoardController(boardModel);
            
            var rendererModel = new RendererModel(BoardCanvas);
            rendererController = new RendererController(rendererModel);

            boardModel.NewPieceAdded += rendererController.DrawNewPiece;
            boardModel.NewPieceAdded += gameController.IncreasePieceCount;
            boardModel.PieceRemoved += rendererController.DeletePiece;
            boardModel.PieceRemoved += gameController.DecreaseOpponentPieceCount;
            boardModel.PieceMoved += rendererController.MovePiece;
            boardModel.SelectionChanged += rendererController.ChangeSelection;
            gameModel.TurnTaken += rendererController.UpdateRendererModel;

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
            var pointModel = boardController.GetPointModelByPosition(currentPoint);
            if (pointModel == null)
            {
                return;
            }

            if (pointModel.Piece == null)
            {
                var addHandled = HandleAddNewPiece(currentPoint);
                if (addHandled)
                {
                    return;
                }

                var moveHandled = HandleMovePiece(currentPoint);
                if (moveHandled)
                {
                    return;
                }
            }
            else
            {
                var removeHandled = HandleRemovePiece(pointModel, currentPoint);
                if (removeHandled)
                {
                    return;
                }

                var changeSelectionHandled = HandleChangeSelection(pointModel, currentPoint);
                if (removeHandled)
                {
                    return;
                }
            }
        }

        private bool HandleChangeSelection(PointModel pointModel, Point currentPoint)
        {
            if (!gameController.CanSelectPiece(pointModel))
            {
                return false;
            }

            boardController.ChangeSelection(currentPoint, gameModel.CurrentPlayer, true);
            return true;
        }

        private bool HandleRemovePiece(PointModel pointModel, Point currentPoint)
        {
            if (!gameController.CanRemovePiece(pointModel))
            {
                return false;
            }

            if (boardController.IsPieceInMill(pointModel.Piece))
            {
                rendererController.ShowMessage(cannotRemovePieceMessage);
                return true;
            }

            boardController.RemoveOpponentPiece(currentPoint, gameModel.OpponentPlayer);

            if (gameController.IsGameOver())
            {
                rendererController.ShowMessage(string.Format(gameOverMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            gameController.TakeTurn();
            return true;
        }

        private bool HandleMovePiece(Point currentPoint)
        {
            if (!gameController.CanMovePiece())
            {
                return false;
            }

            var newPiece = boardController.MoveSelectedPiece(currentPoint);
            if (newPiece == null)
            {
                return false;
            }

            boardController.ChangeSelection(currentPoint, gameModel.CurrentPlayer, false);

            gameModel.CurrentPlayer.HasMill = boardController.IsPieceInMill(newPiece);

            if (gameController.HasMill)
            {
                rendererController.ShowMessage(string.Format(newMillMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            gameController.TakeTurn();
            return true;
        }

        private bool HandleAddNewPiece(Point currentPoint)
        {
            if (!gameController.CanAddNewPiece())
            {
                return false;
            }

            var newPiece = boardController.AddNewPiece(currentPoint, gameModel.CurrentPlayer);
            if (newPiece == null)
            {
                return false;
            }

            gameModel.CurrentPlayer.HasMill = boardController.IsPieceInMill(newPiece);

            if (gameController.HasMill)
            {
                rendererController.ShowMessage(
                    string.Format(newMillMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            gameController.TakeTurn();
            return true;
        }
    }
}
