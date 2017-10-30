using System.Windows.Media;

namespace Mills.Models
{
    public class PlayerModel
    {
        public PlayerModel(int number, Color color)
        {
            Number = number;
            Color = color;
        }

        public int Number { get; private set; }

        public Color Color { get; private set; }

        public int TotalPieceCount { get; set; }

        public int CurrentPieceCount { get; set; }
        
        public bool HasMill { get; set; }
    }
}
