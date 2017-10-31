using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Controllers;
using Mills.Models;
using Mills.Services;
using Mills.UnitTests.Eventing;
using Mills.UnitTests.Helpers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Mills.UnitTests
{
    [TestClass]
    public class GameControllerTests
    {
        [TestMethod]
        public void StartGame_EmptyBoard_RaisesTurnTaken()
        {
            // Arrange
            var stubPoints = new List<PointModel>();
            var stubPlayers = TestHelper.CreateDefaultPlayers();

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
            stubBoardService.CreatePlayers().Returns(stubPlayers);
            
            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
            var stubGameModel = Substitute.For<GameModel>(stubBoardModel, stubBoardService);

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
            var stubPoints = new List<PointModel>();
            var stubPlayers = TestHelper.CreateDefaultPlayers();

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
            stubBoardService.CreatePlayers().Returns(stubPlayers);

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
            var mockGameModel = Substitute.For<GameModel>(stubBoardModel, stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockGameModel.TurnTaken += stubEventSubscriber.Handle;

            var gameController = new GameController(mockGameModel);

            // Act
            gameController.StartGame();

            // Assert
            Assert.IsNotNull(mockGameModel.CurrentPlayer);
        }

        [TestMethod]
        public void IsGameOver_EmptyBoard_ReturnsFalse()
        {
            // Arrange
            var stubPoints = new List<PointModel>();
            var stubPlayers = TestHelper.CreateDefaultPlayers();

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
            stubBoardService.CreatePlayers().Returns(stubPlayers);

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
            var stubGameModel = Substitute.For<GameModel>(stubBoardModel, stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            stubGameModel.TurnTaken += stubEventSubscriber.Handle;

            var gameController = new GameController(stubGameModel);
            gameController.StartGame();

            // Act
            var isGameOver = gameController.IsGameOver();

            // Assert
            Assert.IsFalse(isGameOver);
        }

        [TestMethod]
        public void IsGameOver_OpponentHas3Pieces_ReturnsFalse()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithPiece(3, Colors.Green);

            var stubPlayer1 = Substitute.For<PlayerModel>(1, Colors.Blue);
            stubPlayer1.TotalPieceCount = 9;
            var stubPlayer2 = Substitute.For<PlayerModel>(2, Colors.Green);
            stubPlayer2.CurrentPieceCount = 3;
            stubPlayer2.TotalPieceCount = 9;
            var stubPlayers = new List<PlayerModel> { stubPlayer1, stubPlayer2 };

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
            stubBoardService.CreatePlayers().Returns(stubPlayers);

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
            var stubGameModel = Substitute.For<GameModel>(stubBoardModel, stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            stubGameModel.TurnTaken += stubEventSubscriber.Handle;

            var gameController = new GameController(stubGameModel);
            gameController.StartGame();

            // Act
            var isGameOver = gameController.IsGameOver();

            // Assert
            Assert.IsFalse(isGameOver);
        }

        [TestMethod]
        public void IsGameOver_OpponentHas2Pieces_ReturnsTrue()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithPiece(2, Colors.Green);

            var stubPlayer1 = Substitute.For<PlayerModel>(1, Colors.Blue);
            stubPlayer1.TotalPieceCount = 9;
            var stubPlayer2 = Substitute.For<PlayerModel>(2, Colors.Green);
            stubPlayer2.CurrentPieceCount = 2;
            stubPlayer2.TotalPieceCount = 9;
            var stubPlayers = new List<PlayerModel> { stubPlayer1, stubPlayer2 };

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
            stubBoardService.CreatePlayers().Returns(stubPlayers);

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
            var stubGameModel = Substitute.For<GameModel>(stubBoardModel, stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            stubGameModel.TurnTaken += stubEventSubscriber.Handle;

            var gameController = new GameController(stubGameModel);
            gameController.StartGame();

            // Act
            var isGameOver = gameController.IsGameOver();

            // Assert
            Assert.IsTrue(isGameOver);
        }
    }
}
