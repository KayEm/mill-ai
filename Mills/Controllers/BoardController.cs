using Mills.Models;
using System.Windows;

namespace Mills.Controllers
{
    public class BoardController
    {
        BoardModel boardModel;
        
        public BoardController(BoardModel board)
        {
            boardModel = board;
        }
        
        public void AddNewPiece(Point position, PlayerModel player)
        {
            var pieceModel = new PieceModel() { Color = player.Color };
            var pointModel = boardModel.GetPointModelByPosition(position);
            boardModel.PlaceNewPiece(pieceModel, pointModel);
        }
        
        public void RemoveOpponentPiece(Point position, PlayerModel opponentPlayer)
        {
            var pointModel = boardModel.GetPointModelByPosition(position);
            boardModel.RemovePiece(pointModel);
        }
    }
}
