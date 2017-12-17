using Mills.Models;
using System.Linq;

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
    }
}
