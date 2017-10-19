using Mills.Models;
using System;
using System.Linq;
using System.Windows;

namespace Mills.Controllers
{
    /// <summary>
    /// Board handling.
    /// </summary>
    public class BoardController
    {
        BoardModel boardModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="board">The board model.</param>
        public BoardController(BoardModel board)
        {
            boardModel = board;
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void StartGame()
        {
            boardModel.CurrentPlayer = boardModel.Players.First();
        }

        /// <summary>
        /// Place new piece on the given position.
        /// </summary>
        /// <param name="position">The position where the piece should be placed.</param>
        /// <returns> The new piece, if the piece was placed; otherwise null.</returns>
        public PieceModel PlaceNewPiece(Point position)
        {
            if (boardModel.AllPiecesPlaced())
            {
                return null;
            }

            var pointModel = boardModel.GetPointModelByPosition(position);
            if (pointModel == null)
            {
                return null;
            }

            var pieceModel = new PieceModel() { Color = boardModel.CurrentPlayer.Color };
            boardModel.PlaceNewPiece(pieceModel, pointModel);

            return pieceModel;
        }

        /// <summary>
        /// Remove opponent's piece from the given position.
        /// </summary>
        /// <param name="position">The position where the piece should be removed.</param>
        /// <returns>True is the piece was removed; otherwise false</returns>
        public bool RemoveOpponentPiece(Point position)
        {
            var pointModel = boardModel.GetPointModelByPosition(position);
            if (pointModel == null)
            {
                return false;
            }

            if (pointModel.Piece?.Color == boardModel.CurrentPlayer.Color)
            {
                return false;
            }

            boardModel.RemovePiece(pointModel);

            return true;
        }

        /// <summary>
        /// Check if player placed three of his pieces on contiguous points in a straight line, vertically or horizontally.
        /// </summary>
        /// <returns>true, if new mill is formed; otherwise false</returns>
        public bool IsNewMillFormed(PieceModel addedPiece)
        {
            var mills = boardModel.Mills.Where(m => m.Any(p => p.Piece == addedPiece));
            
            if (mills == null)
            {
                return false;
            }

            boardModel.IsMill = mills.Any(m => m.All(p => p.Piece?.Color == addedPiece.Color));

            return boardModel.IsMill;
        }

        /// <summary>
        /// Take turn between players.
        /// </summary>
        public void TakeTurn()
        {
            boardModel.IsMill = false;

            if (boardModel.CurrentPlayer == boardModel.Players[0])
            {
                boardModel.CurrentPlayer = boardModel.Players[1];
            }
            else
            {
                boardModel.CurrentPlayer = boardModel.Players[0];
            }
        }
    }
}
