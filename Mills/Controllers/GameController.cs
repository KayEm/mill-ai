using Mills.Models;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    public class GameController
    {
        private const int minimumPieceCount = 2;
        private const int maximumPieceCount = 9;
        private GameModel gameModel;

        public GameController(GameModel game)
        {
            gameModel = game;
        }

        public bool HasMill => gameModel.CurrentPlayer.HasMill;

        public bool IsGameOver()
        {
            if (!AllPiecesAdded())
            {
                return false;
            }

            return gameModel.OpponentPlayer.CurrentPieceCount == minimumPieceCount;
        }
        
        public void StartGame()
        {
            gameModel.CurrentPlayer = gameModel.Players.First();
        }
        
        public void TakeTurn()
        {
            gameModel.CurrentPlayer.HasMill = false;

            if (gameModel.CurrentPlayer == gameModel.Players[0])
            {
                gameModel.CurrentPlayer = gameModel.Players[1];
            }
            else
            {
                gameModel.CurrentPlayer = gameModel.Players[0];
            }
        }

        public void IncreasePieceCount(PointModel point)
        {
            gameModel.CurrentPlayer.TotalPieceCount++;
            gameModel.CurrentPlayer.CurrentPieceCount++;
        }

        public void DecreaseOpponentPieceCount(PointModel point)
        {
            gameModel.OpponentPlayer.CurrentPieceCount--;
        }

        public bool CanRemovePiece(Point position)
        {
            if (!HasMill)
            {
                return false;
            }

            var pointModel = gameModel.BoardModel.GetPointModelByPosition(position);
            if (pointModel == null)
            {
                return false;
            }

            if (pointModel.Piece?.Color != gameModel.OpponentPlayer.Color)
            {
                return false;
            }

            return true;
        }

        public bool CanAddNewPiece(Point position)
        {
            if (HasMill)
            {
                return false;
            }

            if (AllPiecesAdded())
            {
                return false;
            }

            var pointModel = gameModel.BoardModel.GetPointModelByPosition(position);
            if (pointModel == null || pointModel.Piece != null)
            {
                return false;
            }

            return true;
        }

        public bool CanSelectPiece(Point position)
        {
            if (HasMill)
            {
                return false;
            }

            if (!AllPiecesAdded())
            {
                return false;
            }

            var selectedPoint = gameModel.BoardModel.GetPointModelByPosition(position);
            return selectedPoint?.Piece?.Color == gameModel.CurrentPlayer.Color;
        }

        public bool CanMovePiece(Point position)
        {
            if (!AllPiecesAdded())
            {
                return false;
            }

            if (!gameModel.BoardModel.IsAnyPieceSelected())
            {
                return false;
            }

            return true;
        }

        private bool AllPiecesAdded()
        {
            return gameModel.CurrentPlayer.TotalPieceCount == maximumPieceCount;
        }
    }
}
