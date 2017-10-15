using Mills.Models;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    /// <summary>
    /// Board handling.
    /// </summary>
    public class BoardController
    {
        BoardModel board;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="board">The board model.</param>
        public BoardController(BoardModel board)
        {
            this.board = board;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            board.CurrentPlayer = board.Players.First();
        }

        /// <summary>
        /// Place new piece on the given position.
        /// </summary>
        /// <param name="position">The position where the piece should be placed.</param>
        /// <returns> True, if the piece was placed; otherwise false.</returns>
        public bool PlaceNewPiece(Point position)
        {
            if (!board.CanPlaceNewPiece())
            {
                return false;
            }

            var isPointEmpty = board.IsPointEmpty(position);
            if (!isPointEmpty.Item1)
            {
                return false;
            }

            var piece = new PieceModel() { Color = board.CurrentPlayer.Color };
            board.PlaceNewPiece(piece, isPointEmpty.Item2);

            return true;
        }

        /// <summary>
        /// Take turn between players.
        /// </summary>
        public void TakeTurn()
        {
            if(board.CurrentPlayer == board.Players[0])
            {
                board.CurrentPlayer = board.Players[1];
            }
            else
            {
                board.CurrentPlayer = board.Players[0];
            }
        }
    }
}
