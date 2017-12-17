using System.Windows.Controls;
using Mills.Controllers;
using Mills.Models;
using Mills.Services;
using System.Windows;
using System.Windows.Input;

namespace Mills.Views
{
    /// <summary>
    /// Interaction logic for BoardControl.xaml
    /// </summary>
    public partial class BoardControl : UserControl
    {
        public BoardControl()
        {
            InitializeComponent();
        }

        internal void InitializeBoard()
        {
            // Bootstrapper
            var boardService = new BoardService();

            var gameModel = new GameModel(boardService);
            var gameController = new GameController(gameModel);

            var boardModel = new BoardModel(boardService);
            var boardController = new BoardController(boardModel);

            var boardViewModel = new BoardViewModel(BoardCanvas, gameModel, boardModel);

            var userMessageController = new UserMessageController(boardViewModel);
            var rendererController = new RendererController(boardViewModel);

            boardModel.NewPieceAdded += rendererController.DrawNewPiece;
            boardModel.NewPieceAdded += gameController.IncreasePieceCount;
            boardModel.PieceRemoved += rendererController.DeletePiece;
            boardModel.PieceRemoved += gameController.DecreaseOpponentPieceCount;
            boardModel.PieceMoved += rendererController.MovePiece;
            boardModel.SelectionChanged += rendererController.ChangeSelection;
            gameModel.TurnTaken += rendererController.UpdateRendererModel;

            rendererController.NotifyUser += userMessageController.SetUserMessage;
            boardViewModel.NotifyUser += userMessageController.SetUserMessage;
            boardViewModel.SelectPiece += boardController.ChangeSelection;
            boardViewModel.RemovePiece += boardController.RemovePiece;
            boardViewModel.TakeTurn += gameController.TakeTurn;
            boardViewModel.AddPiece += boardController.AddNewPiece;
            boardViewModel.MoveSelectedPiece += boardController.MoveSelectedPiece;

            DataContext = boardViewModel;
            gameController.StartGame();
        }
    }
}
