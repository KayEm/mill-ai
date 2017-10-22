using System.Windows.Media;

namespace Mills.Models
{
    public class PlayerModel
    {
        private Color color;
        
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public bool AllPiecesAdded { get; set; }

        public bool HasMill { get; set; }
    }
}
