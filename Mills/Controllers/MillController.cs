using Mills.Models;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    public class MillController
    {
        private MillModel millModel;

        public MillController(MillModel mill)
        {
            millModel = mill;
        }

        public bool IsMill => millModel.IsMill;
        
        public void CheckNewMill(PlayerModel player)
        {
            millModel.IsMill = millModel.Mills.Any(m => m.All(p => p.Piece?.Color == player.Color));

            if (millModel.IsMill)
            {
                MessageBox.Show("You have a mill. Remove one piece from the opponent.");
            }
        }

        public void SetIsMillToFalse()
        {
            millModel.IsMill = false;
        }
    }
}
