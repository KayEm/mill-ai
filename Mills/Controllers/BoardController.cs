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
            if (board.AllPiecesPlaced())
            {
                return false;
            }

            var isPointEmpty = board.IsPointEmpty(position);
            if (isPointEmpty.Item1 || isPointEmpty.Item2.Piece != null)
            {
                return false;
            }

            var piece = new PieceModel() { Color = board.CurrentPlayer.Color };
            board.PlaceNewPiece(piece, isPointEmpty.Item2);

            return true;
        }

        /// <summary>
        /// Remove opponent's piece from the given position.
        /// </summary>
        /// <param name="position">The position where the piece should be removed.</param>
        /// <returns>True is the piece was removed; otherwise false</returns>
        public bool RemoveOpponentPiece(Point position)
        {
            var isPointEmpty = board.IsPointEmpty(position);
            if (isPointEmpty.Item1)
            {
                return false;
            }

            if (isPointEmpty.Item2.Piece.Color == board.CurrentPlayer.Color)
            {
                return false;
            }

            board.RemovePiece(isPointEmpty.Item2);

            return true;
        }

        /// <summary>
        /// Check if player placed three of his pieces on contiguous points in a straight line, vertically or horizontally.
        /// </summary>
        /// <returns>true, if new mill is formed; otherwise false</returns>
        public bool IsNewMillFormed()
        {
            // todo

            return false;
        }

        /// <summary>
        /// Take turn between players.
        /// </summary>
        public void TakeTurn()
        {
            if (board.CurrentPlayer == board.Players[0])
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
