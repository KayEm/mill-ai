using Mills.Models;
using NSubstitute;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Mills.UnitTests.Helpers
{
    public class TestHelper
    {
        public static GameModel CreateGameModel(List<PointModel> points, List<PlayerModel> players)
        {
            var stubBoardModel = Substitute.For<BoardModel>(points);

            return Substitute.For<GameModel>(stubBoardModel, players);
        }

        public static List<PlayerModel> CreateDefaultPlayers()
        {
            var stubPlayer1 = Substitute.For<PlayerModel>(1, Colors.Blue);
            var stubPlayer2 = Substitute.For<PlayerModel>(2, Colors.Green);
            return new List<PlayerModel> { stubPlayer1, stubPlayer2 };
        }

        public static List<PointModel> CreatePoints(int count, Color color)
        {
            var stubPoints = new List<PointModel>();
            for (int i = 0; i < count; i++)
            {
                var stubpieceModel = Substitute.For<PieceModel>();
                stubpieceModel.Color = color;
                var stubPointModel = Substitute.For<PointModel>();
                stubPointModel.Piece = stubpieceModel;

                stubPoints.Add(stubPointModel);
            }

            return stubPoints;
        }

        public static List<PointModel> CreatePoints(int count)
        {
            var points = new List<PointModel>();
            for (int i = 0; i < count; i++)
            {
                var pointModel = Substitute.For<PointModel>();
                pointModel.Bounds = new Rect(new Point(i * 10, i * 10), new Size(10, 10));

                points.Add(pointModel);
            }

            return points;
        }
    }
}
