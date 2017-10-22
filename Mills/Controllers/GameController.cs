using Mills.Models;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    public class GameController
    {
        private GameModel gameModel;

        public GameController(GameModel game)
        {
            gameModel = game;
        }
        
        public void StartGame()
        {
            gameModel.CurrentPlayer = gameModel.Players.First();
        }
        
        public void TakeTurn()
        {
            if (gameModel.CurrentPlayer == gameModel.Players[0])
            {
                gameModel.CurrentPlayer = gameModel.Players[1];
            }
            else
            {
                gameModel.CurrentPlayer = gameModel.Players[0];
            }
        }

        public void CheckAllPiecesAdded()
        {
            var player = gameModel.CurrentPlayer;
            if (player.AllPiecesAdded)
            {
                return;
            }

            player.AllPiecesAdded = gameModel.BoardModel.Points.Count(p => p.Piece?.Color == player.Color) == 9;
        }

        public bool CanTakeTurn(bool isMill)
        {
            if (isMill)
            {
                return false;
            }

            return true;
        }

        public bool CanRemovePiece(Point position, bool isMill)
        {
            if (!isMill)
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

        public bool CannAddNewPiece(Point position)
        {
            if (gameModel.CurrentPlayer.AllPiecesAdded)
            {
                return false;
            }

            var pointModel = gameModel.BoardModel.GetPointModelByPosition(position);
            if (pointModel == null)
            {
                return false;
            }

            return true;
        }
    }
}
