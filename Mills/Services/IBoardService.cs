using Mills.Models;
using System;
using System.Collections.Generic;

namespace Mills.Services
{
    public interface IBoardService
    {
        Tuple<List<PointModel>, List<List<PointModel>>> CreateInitialBoard();

        List<PlayerModel> CreatePlayers();
    }
}
