using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Services;
using System.Linq;

namespace Mills.UnitTests.Services
{
    [TestClass]
    public class BoardServiceTests
    {
        [TestMethod]
        public void CreatePlayers__CheckPlayerCount()
        {
            // Arrange
            var boardService = new BoardService();

            // Act
            var players = boardService.CreatePlayers();

            // Assert
            Assert.AreEqual(2, players.Count);
        }

        [TestMethod]
        public void CreateInitialBoard__CheckPossibleMillCount()
        {
            // Arrange
            var boardService = new BoardService();

            // Act
            var initialBoard = boardService.CreateInitialBoard();
            var mills = initialBoard.Item2;

            // Assert
            Assert.AreEqual(16, mills.Count);
        }

        [TestMethod]
        public void CreateInitialBoard__CheckPointCount()
        {
            // Arrange
            var boardService = new BoardService();

            // Act
            var initialBoard = boardService.CreateInitialBoard();
            var points = initialBoard.Item1;

            // Assert
            Assert.AreEqual(24, points.Count);
        }

        [DataTestMethod]
        [DataRow("a", 1, 2, DisplayName = "A1")]
        [DataRow("a", 4, 3, DisplayName = "A4")]
        [DataRow("a", 7, 2, DisplayName = "A7")]
        [DataRow("b", 2, 2, DisplayName = "B2")]
        [DataRow("b", 4, 4, DisplayName = "B4")]
        [DataRow("b", 6, 2, DisplayName = "B6")]
        [DataRow("c", 3, 2, DisplayName = "C3")]
        [DataRow("c", 4, 3, DisplayName = "C4")]
        [DataRow("c", 5, 2, DisplayName = "C5")]
        [DataRow("d", 1, 3, DisplayName = "D1")]
        [DataRow("d", 2, 4, DisplayName = "D2")]
        [DataRow("d", 3, 3, DisplayName = "D3")]
        [DataRow("d", 5, 3, DisplayName = "D5")]
        [DataRow("d", 6, 4, DisplayName = "D6")]
        [DataRow("d", 7, 3, DisplayName = "D7")]
        [DataRow("e", 3, 2, DisplayName = "E3")]
        [DataRow("e", 4, 3, DisplayName = "E4")]
        [DataRow("e", 5, 2, DisplayName = "E5")]
        [DataRow("f", 2, 2, DisplayName = "F2")]
        [DataRow("f", 4, 4, DisplayName = "F4")]
        [DataRow("f", 6, 2, DisplayName = "F6")]
        [DataRow("g", 1, 2, DisplayName = "G1")]
        [DataRow("g", 4, 3, DisplayName = "G4")]
        [DataRow("g", 7, 2, DisplayName = "G7")]
        public void CreateInitialBoard__CheckNeighborCount(string x, int y, int expectedCount)
        {
            // Arrange
            var boardService = new BoardService();

            // Act
            var initialBoard = boardService.CreateInitialBoard();
            var points = initialBoard.Item1;

            // Assert
            Assert.AreEqual(expectedCount, points.Where(p => p.X == x && p.Y == y).First().Neighbors.Count);
        }
    }
}
