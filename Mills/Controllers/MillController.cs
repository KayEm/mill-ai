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
                
        public void CheckNewMill(PlayerModel player)
        {
            player.HasMill = millModel.Mills.Any(m => m.All(p => p.Piece?.Color == player.Color));

            if (player.HasMill)
            {
                MessageBox.Show("You have a mill. Remove one piece from the opponent.");
            }
        }
    }
}
