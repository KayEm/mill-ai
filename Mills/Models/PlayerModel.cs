using System.Windows.Media;

namespace Mills.Models
{
    public class PlayerModel
    {        
        public Color Color { get; set; }

        public int TotalPieceCount { get; set; }

        public int CurrentPieceCount { get; set; }
        
        public bool HasMill { get; set; }

        public int Number { get; set; }
    }
}
