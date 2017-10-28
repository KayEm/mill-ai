using Mills.Models;
using System.Linq;

namespace Mills.Controllers
{
    public class MillController
    {
        private MillModel millModel;

        public MillController(MillModel mill)
        {
            millModel = mill;
        }
                
        public void CheckNewMill(PlayerModel player, PieceModel newPiece)
        {
            player.HasMill = IsPieceInMill(newPiece);
        }

        public bool IsPieceInMill(PieceModel piece)
        {
            var possibleMills = millModel.Mills.Where(m => m.Any(p => p.Piece == piece));
            return possibleMills.Any(m => m.All(p => p.Piece != null && p.Piece.Color == piece.Color));
        }
    }
}
