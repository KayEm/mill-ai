using Mills.Models;
using System.Linq;

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

        public bool CanRemovePiece(PointModel pointModel)
        {
            if (!HasMill)
            {
                return false;
            }

            if (pointModel?.Piece?.Color != gameModel.OpponentPlayer.Color)
            {
                return false;
            }

            return true;
        }

        public bool CanAddNewPiece()
        {
            if (HasMill)
            {
                return false;
            }

            if (AllPiecesAdded())
            {
                return false;
            }

            return true;
        }

        public bool CanSelectPiece(PointModel pointModel)
        {
            if (HasMill)
            {
                return false;
            }

            if (!AllPiecesAdded())
            {
                return false;
            }

            return pointModel?.Piece?.Color == gameModel.CurrentPlayer.Color;
        }

        public bool CanMovePiece()
        {
            if (!AllPiecesAdded())
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
