using System;
using System.Collections.Generic;

namespace Mills.Models
{
    public class BoardModel
    {
        public BoardModel(List<PlayerModel> players, List<PieceModel> pieces)
        {
            Players = players;
            Pieces = pieces;
        }

        public List<PieceModel> Pieces { get; private set; }

        public List<PlayerModel> Players { get; private set; }

        public PlayerModel CurrentPlayer { get; set; }

        public event Action<PieceModel> NewPiecePlaced;

        public void PlaceNewPiece(PieceModel piece)
        {
            Pieces.Add(piece);
            NewPiecePlaced(piece);
        }
    }
}
