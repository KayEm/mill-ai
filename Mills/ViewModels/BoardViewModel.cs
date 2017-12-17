using Mills.Communication;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Linq;

namespace Mills.Models
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        private const string newMillMessage = "Player {0} has formed a mill. Remove opponent's piece.";
        private const string cannotRemovePieceMessage = "That piece is in a mill and cannot be removed.";
        private const string gameOverMessage = "Game over! Player {0} has won the game.";

        private const int minimumPieceCount = 2;
        private const int maximumPieceCount = 9;

        private GameModel gameModel;
        private BoardModel boardModel;

        public event Action<string> NotifyUser;
        public event Action<PointModel, PlayerModel, bool> SelectPiece;
        public event Action<PointModel> RemovePiece;
        public event Action TakeTurn;

        public event Action<PointModel> MoveSelectedPiece;
        public event Action<PointModel, PlayerModel> AddPiece;

        public BoardViewModel(Canvas canvas, GameModel gameModel, BoardModel boardModel)
        {
            Canvas = canvas;
            this.gameModel = gameModel;
            this.boardModel = boardModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Canvas Canvas { get; set; }

        public bool HasMill => gameModel.CurrentPlayer.HasMill;

        private string userMessage;
        public string UserMessage
        {
            get
            {
                return userMessage;
            }

            set
            {
                userMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserMessage"));
            }
        }

        private SolidColorBrush currentPlayerColor;
        public SolidColorBrush CurrentPlayerColor
        {
            get
            {
                return currentPlayerColor;
            }

            set
            {
                currentPlayerColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPlayerColor"));
            }
        }

        RelayCommand canvasMouseLeftClickCommand;
        public ICommand CanvasMouseLeftClickCommand
        {
            get
            {
                if (canvasMouseLeftClickCommand == null)
                {
                    canvasMouseLeftClickCommand = new RelayCommand(param => HandleCanvasMouseLeftClick(),
                        param => CanCanvasMouseLeftClick());
                }
                return canvasMouseLeftClickCommand;
            }
        }

        private bool CanCanvasMouseLeftClick()
        {
            return true;
        }

        private void HandleCanvasMouseLeftClick()
        {
            if (IsGameOver())
            {
                return;
            }

            var currentPoint = Mouse.GetPosition(Canvas);
            var pointModel = GetPointModelByPosition(currentPoint);
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
            if (!CanSelectPiece(pointModel, currentPoint))
            {
                return false;
            }

            SelectPiece(pointModel, gameModel.CurrentPlayer, true);
            return true;
        }

        private bool HandleRemovePiece(PointModel pointModel, Point currentPoint)
        {
            if (!CanRemovePiece(pointModel))
            {
                return false;
            }

            if (IsPieceInMill(pointModel.Piece))
            {
                NotifyUser(cannotRemovePieceMessage);
                return true;
            }

            RemovePiece(pointModel);

            if (IsGameOver())
            {
                NotifyUser(string.Format(gameOverMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            TakeTurn();
            return true;
        }

        private bool HandleMovePiece(Point currentPoint)
        {
            if (!CanMovePiece())
            {
                return false;
            }

            var newPoint = GetPointModelByPosition(currentPoint);
            if (newPoint == null)
            {
                return false;
            }

            MoveSelectedPiece(newPoint);
            if (newPoint.Piece == null)
            {
                return false;
            }

            SelectPiece(newPoint, gameModel.CurrentPlayer, false);

            gameModel.CurrentPlayer.HasMill = IsPieceInMill(newPoint.Piece);

            if (HasMill)
            {
                NotifyUser(string.Format(newMillMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            TakeTurn();
            return true;
        }

        private bool HandleAddNewPiece(Point currentPoint)
        {
            if (!CanAddNewPiece())
            {
                return false;
            }

            var pointModel = GetPointModelByPosition(currentPoint);
            AddPiece(pointModel, gameModel.CurrentPlayer);
            if (pointModel.Piece == null)
            {
                return false;
            }

            gameModel.CurrentPlayer.HasMill = IsPieceInMill(pointModel.Piece);

            if (HasMill)
            {
                NotifyUser(string.Format(newMillMessage, gameModel.CurrentPlayer.Number));
                return true;
            }

            TakeTurn();
            return true;
        }

        public bool IsGameOver()
        {
            if (!AllPiecesAdded())
            {
                return false;
            }

            return gameModel.OpponentPlayer.CurrentPieceCount == minimumPieceCount;
        }

        private bool AllPiecesAdded()
        {
            return gameModel.CurrentPlayer.TotalPieceCount == maximumPieceCount;
        }

        public bool CanRemovePiece(PointModel pointModel)
        {
            if (!HasMill || pointModel?.Piece?.Color != gameModel.OpponentPlayer.Color)
            {
                return false;
            }

            return true;
        }

        public bool CanAddNewPiece()
        {
            if (HasMill || AllPiecesAdded())
            {
                return false;
            }

            return true;
        }

        public bool CanSelectPiece(PointModel pointModel, Point currentPoint)
        {
            if (HasMill || !AllPiecesAdded())
            {
                return false;
            }

            var newSelectedPoint = GetPointModelByPosition(currentPoint);
            if (newSelectedPoint?.Piece == null)
            {
                return false;
            }

            return pointModel?.Piece?.Color == gameModel.CurrentPlayer.Color;
        }

        public bool CanMovePiece()
        {
            return AllPiecesAdded();
        }

        public bool IsPieceInMill(PieceModel piece)
        {
            if (piece == null)
            {
                return false;
            }

            var possibleMills = boardModel.Mills.Where(m => m.Any(p => p.Piece == piece));
            return possibleMills.Any(m => m.All(p => p.Piece != null && p.Piece.Color == piece.Color));
        }

        public PointModel GetPointModelByPosition(Point position)
        {
            return boardModel.Points.Where(p => p.Bounds.Contains(position)).FirstOrDefault();
        }
    }
}
