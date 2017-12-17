using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Models;
using NSubstitute;
using Mills.Controllers;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using Mills.UnitTests.Eventing;
using Mills.UnitTests.Helpers;
using System.Collections.Generic;
using System;
using Mills.Services;

namespace Mills.UnitTests.Controllers
{
    [TestClass]
    public class BoardControllerTests
    {
        [DataTestMethod]
        [DataRow(5, 1, 2, 1, DisplayName = "Point at position.")]
        [DataRow(5, 2, 2, 0, DisplayName = "Piece on point at position.")]
        public void AddNewPiece_Position_NewPieceAddedRaised(
            int pointCount, int newPointModelIndex, int stubPointModelIndex, int expectedCallCount)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);
 
            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));
 
            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);
 
            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            boardController.AddNewPiece(stubPoints[newPointModelIndex], stubPlayer);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }
        
        [DataTestMethod]
        [DataRow(5, 1, 1, DisplayName = "Piece on point at position.")]
        public void AddNewPiece_InvalidPosition_NoPieceIsAdded(
            int pieceCount, int newPointModelIndex, int stubPointModelIndex)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pieceCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);

            // Act
            boardController.AddNewPiece(stubPoints[newPointModelIndex], stubPlayer);

            // Assert
            Assert.AreEqual(1, mockBoardModel.Points.Count(p => p.Piece != null));
        }

        [TestMethod]
        public void AddNewPiece_EmptyPointAtPosition_ReturnsNewPieceAndIsNotNull()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);

            // Act
            boardController.AddNewPiece(stubPoints[0], stubPlayer);

            // Assert
            Assert.IsNotNull(stubPoints[0].Piece);
        }

        [TestMethod]
        public void AddNewPiece_EmptyPointAtPosition_NewPieceIsAddedToCorrectPoint()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);

            // Act
            boardController.AddNewPiece(stubPoints[2], stubPlayer);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[2].Bounds)).First();
            Assert.AreEqual(stubPoints[2].Piece, expectedPoint.Piece);
        }
        
        [DataTestMethod]
        [DataRow(5, 1, 3, 1, 3, 1, DisplayName = "Point at position.")]
        [DataRow(5, 2, 2, 3, 4, 0, DisplayName = "Piece on point at position.")]
        public void MoveSelectedPiece_SelectedPoint_PieceMovedRaised(
            int pointCount, int newPointModelIndex, int stubPointModelIndex, int pointIndex, int neightborIndex, int expectedCallCount)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);
            stubPoints[pointIndex].Neighbors = new List<PointModel> { stubPoints[neightborIndex] };

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.SelectionChanged += mockEventSubscriber.Handle;
            stubBoardModel.PieceMoved += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);
            boardController.ChangeSelection(stubPoints[stubPointModelIndex], stubPlayer, true);
            mockEventSubscriber.Reset();

            // Act
            boardController.MoveSelectedPiece(stubPoints[newPointModelIndex]);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }
                
        [DataTestMethod]
        [DataRow(5, 2, 2, DisplayName = "Piece on point at position.")]
        public void MoveSelectedPiece_InvalidNewPoint_PieceIsNotMoved(
            int pointCount, int newPointModelIndex, int stubPointModelIndex)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;
            mockBoardModel.PieceMoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);
            boardController.ChangeSelection(stubPoints[stubPointModelIndex], stubPlayer, true);

            // Act
            boardController.MoveSelectedPiece(stubPoints[newPointModelIndex]);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[stubPointModelIndex].Bounds)).First();
            Assert.IsNotNull(expectedPoint.Piece);
        }

        [TestMethod]
        public void MoveSelectedPiece_ValidSelectedPoint_PieceIsMovedToCorrectPoint()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);
            stubPoints[2].Neighbors = new List<PointModel> { stubPoints[3] };

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;
            mockBoardModel.PieceMoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[1], stubPlayer);
            boardController.ChangeSelection(stubPoints[1], stubPlayer, true);

            // Act
            boardController.MoveSelectedPiece(stubPoints[2]);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[2].Bounds)).First();
            Assert.AreEqual(stubPoints[2].Piece, expectedPoint.Piece);
        }
        
        [DataTestMethod]
        [DataRow(5, 1, 3, 0, DisplayName = "No piece on point at position.")]
        [DataRow(5, 2, 2, 1, DisplayName = "Piece on point at position.")]
        public void RemovePiece_Position_PieceRemovedRaised(
            int pointCount, int newPointModelIndex, int stubPointModelIndex, int expectedCallCount)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.PieceRemoved += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            boardController.RemovePiece(stubPoints[newPointModelIndex]);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 1, 3, DisplayName = "No piece on point at position.")]
        public void RemovePiece_InvalidPosition_PieceIsNotRemoved(
            int pointCount, int newPointModelIndex, int stubPointModelIndex)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.PieceRemoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);

            // Act
            boardController.RemovePiece(stubPoints[newPointModelIndex]);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[stubPointModelIndex].Bounds)).First();
            Assert.IsNotNull(expectedPoint.Piece);
        }

        [TestMethod]
        public void RemovePiece_ValidPosition_PieceIsRemoved()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.PieceRemoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[1], stubPlayer);

            // Act
            boardController.RemovePiece(stubPoints[1]);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[1].Bounds)).First();
            Assert.IsNull(expectedPoint.Piece);
        }

        [DataTestMethod]
        [DataRow(5, 1, 3, 0, DisplayName = "No piece on point at position.")]
        [DataRow(5, 2, 2, 1, DisplayName = "Piece on point at position.")]
        public void ChangeSelection_Position_SelectionChangedRaised(
            int pointCount, int newPointModelIndex, int stubPointModelIndex, int expectedCallCount)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var stubBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.SelectionChanged += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            boardController.ChangeSelection(stubPoints[newPointModelIndex], stubPlayer, true);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 1, 3, DisplayName = "No piece on point at position.")]
        public void ChangeSelection_InvalidPosition_NoPieceSelected(
            int pointCount, int newPointModelIndex, int stubPointModelIndex)
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(pointCount);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[stubPointModelIndex], stubPlayer);

            // Act
            boardController.ChangeSelection(stubPoints[newPointModelIndex], stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(0, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_ValidPoint_PieceIsSelected()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[2], stubPlayer);

            // Act
            boardController.ChangeSelection(stubPoints[2], stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(1, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_OnlyOnePieceIsSelected()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[2], stubPlayer);

            boardController.AddNewPiece(stubPoints[4], stubPlayer);

            // Act
            boardController.ChangeSelection(stubPoints[4], stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(1, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_NewPieceIsSelected()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[2], stubPlayer);

            boardController.AddNewPiece(stubPoints[4], stubPlayer);

            // Act
            boardController.ChangeSelection(stubPoints[4], stubPlayer, true);

            // Assert
            var newPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[4].Bounds)).First();
            Assert.IsTrue(newPoint.Piece.IsSelected);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_OldPieceIsNotSelected()
        {
            // Arrange
            var stubPoints = TestHelper.CreatePointsWithBounds(5);

            var stubBoardService = Substitute.For<IBoardService>();
            stubBoardService.CreateInitialBoard()
                .Returns(new Tuple<List<PointModel>, List<List<PointModel>>>(stubPoints, null));

            var mockBoardModel = Substitute.For<BoardModel>(stubBoardService);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPoints[2], stubPlayer);

            boardController.AddNewPiece(stubPoints[4], stubPlayer);

            // Act
            boardController.ChangeSelection(stubPoints[4], stubPlayer, true);

            // Assert
            var oldPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPoints[2].Bounds)).First();
            Assert.IsFalse(oldPoint.Piece.IsSelected);
        }
    }
}
