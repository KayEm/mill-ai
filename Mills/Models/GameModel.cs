using System;
using System.Collections.Generic;
using System.Linq;

namespace Mills.Models
{
    public class GameModel
    {
        private PlayerModel currentPlayer;
        
        public GameModel(BoardModel boardModel, List<PlayerModel> players)
        {
            BoardModel = boardModel;
            Players = players;
        }
        
        public event Action<PlayerModel> TurnTaken;

        public BoardModel BoardModel { get; private set; }
        
        public List<PlayerModel> Players { get; private set; }
        
        public PlayerModel CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
                TurnTaken(currentPlayer);
            }
        }

        public PlayerModel OpponentPlayer
        {
            get
            {
                return Players.Where(p => p != CurrentPlayer).FirstOrDefault();
            }
        }
    }
}
