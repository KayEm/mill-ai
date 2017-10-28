using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Mills.Models
{
    public class RendererModel : INotifyPropertyChanged
    {
        public RendererModel(Canvas canvas)
        {
            Canvas = canvas;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Canvas Canvas { get; set; }

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
    }
}
