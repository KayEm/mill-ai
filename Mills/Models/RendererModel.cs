using System.Windows.Controls;
using System.Windows.Shapes;

namespace Mills.Models
{
    public class RendererModel
    {
        public RendererModel(Canvas canvas)
        {
            Canvas = canvas;
        }
        
        public Canvas Canvas { get; set; }
    }
}
