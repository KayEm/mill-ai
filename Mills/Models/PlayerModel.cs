using System.Windows.Media;

namespace Mills.Models
{
    public class PlayerModel : NotifyPropertyModel
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
                NotifyPropertyChanged("Color");
            }
        }

        public bool AllPiecesAdded { get; set; }
    }
}
