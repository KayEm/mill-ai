using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Controllers;
using Mills.Models;
using Mills.Services;
using Mills.UnitTests.Eventing;
using Mills.UnitTests.Helpers;
using NSubstitute;

namespace Mills.UnitTests
{
    [TestClass]
    public class GameControllerTests
    {
        [TestMethod]
        public void StartGame_EmptyBoard_RaisesTurnTaken()
        {
            // Arrange
            var stubPlayers = TestHelper.CreateDefaultPlayers();

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreatePlayers().Returns(stubPlayers);
            
            var stubGameModel = Substitute.For<GameModel>(stubBoardService);

            var mockEventSubscriber = new MockEventSubscriber();
            stubGameModel.TurnTaken += mockEventSubscriber.Handle;

            var gameController = new GameController(stubGameModel);

            // Act
            gameController.StartGame();

            // Assert
            Assert.AreEqual(1, mockEventSubscriber.HitCount);
        }

        [TestMethod]
        public void StartGame_EmptyBoard_CurrentPlayerIsSet()
        {
            // Arrange
            var stubPlayers = TestHelper.CreateDefaultPlayers();

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreatePlayers().Returns(stubPlayers);

            var mockGameModel = Substitute.For<GameModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockGameModel.TurnTaken += stubEventSubscriber.Handle;

            var gameController = new GameController(mockGameModel);

            // Act
            gameController.StartGame();

            // Assert
            Assert.IsNotNull(mockGameModel.CurrentPlayer);
        }
    }
}
