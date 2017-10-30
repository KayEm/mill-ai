using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Models;
using System.Collections.Generic;
using NSubstitute;
using Mills.Controllers;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using Mills.UnitTests.Eventing;

namespace Mills.UnitTests.Controllers
{
    [TestClass]
    public class BoardControllerTests
    {
        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, 1, DisplayName = "Point at position.")]
        [DataRow(5, 150, 150, 35, 35, 0, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, 0, DisplayName = "Piece on point at position.")]
        public void AddNewPiece_Position_NewPieceAddedRaised(
            int pointCount, int x, int y, int stubX, int stubY, int expectedCallCount)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            var newPosition = new Point(x, y);
            var newPiece = boardController.AddNewPiece(newPosition, stubPlayer);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 100, 100, 15, 15, DisplayName = "No point at position.")]
        [DataRow(5, 15, 15, 15, 15, DisplayName = "Piece on point at position.")]
        public void AddNewPiece_InvalidPosition_ReturnsNull(
            int pieceCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pieceCount);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            // Act
            var newPosition = new Point(x, y);
            var newPiece = boardController.AddNewPiece(newPosition, stubPlayer);

            // Assert
            Assert.IsNull(newPiece);
        }

        [DataTestMethod]
        [DataRow(5, 100, 100, 15, 15, DisplayName = "No point at position.")]
        [DataRow(5, 15, 15, 15, 15, DisplayName = "Piece on point at position.")]
        public void AddNewPiece_InvalidPosition_NoPieceIsAdded(
            int pieceCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pieceCount);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            // Act
            var newPosition = new Point(x, y);
            boardController.AddNewPiece(newPosition, stubPlayer);

            // Assert
            Assert.AreEqual(1, mockBoardModel.Points.Count(p => p.Piece != null));
        }

        [TestMethod]
        public void AddNewPiece_EmptyPointAtPosition_ReturnsNewPieceAndIsNotNull()
        {
            // Arrange
            var stubPoints = CreatePoints(5);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);

            // Act
            var position = new Point(5, 5);
            var newPiece = boardController.AddNewPiece(position, stubPlayer);

            // Assert
            Assert.IsNotNull(newPiece);
        }

        [TestMethod]
        public void AddNewPiece_EmptyPointAtPosition_NewPieceIsAddedToCorrectPoint()
        {
            // Arrange
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);

            // Act
            var position = new Point(25, 25);
            var newPiece = boardController.AddNewPiece(position, stubPlayer);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(position)).First();
            Assert.AreEqual(newPiece, expectedPoint.Piece);
        }
        
        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, 1, DisplayName = "Point at position.")]
        [DataRow(5, 150, 150, 35, 35, 0, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, 0, DisplayName = "Piece on point at position.")]
        public void MoveSelectedPiece_SelectedPoint_PieceMovedRaised(
            int pointCount, int x, int y, int stubX, int stubY, int expectedCallCount)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.SelectionChanged += mockEventSubscriber.Handle;
            stubBoardModel.PieceMoved += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            boardController.ChangeSelection(stubPosition, stubPlayer, true);
            mockEventSubscriber.Reset();

            // Act
            var newPosition = new Point(x, y);
            boardController.MoveSelectedPiece(newPosition);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 150, 150, 35, 35, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, DisplayName = "Piece on point at position.")]
        public void MoveSelectedPiece_InvalidNewPoint_ReturnsNull(
            int pointCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;
            mockBoardModel.PieceMoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            boardController.ChangeSelection(stubPosition, stubPlayer, true);

            // Act
            var newPosition = new Point(x, y);
            var piece = boardController.MoveSelectedPiece(newPosition);

            // Assert
            Assert.IsNull(piece);
        }
        
        [DataTestMethod]
        [DataRow(5, 150, 150, 35, 35, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, DisplayName = "Piece on point at position.")]
        public void MoveSelectedPiece_InvalidNewPoint_PieceIsNotMoved(
            int pointCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;
            mockBoardModel.PieceMoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            boardController.ChangeSelection(stubPosition, stubPlayer, true);

            // Act
            var newPosition = new Point(x, y);
            var piece = boardController.MoveSelectedPiece(newPosition);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPosition)).First();
            Assert.IsNotNull(expectedPoint.Piece);
        }

        [TestMethod]
        public void MoveSelectedPiece_ValidSelectedPoint_PieceIsMovedToCorrectPoint()
        {
            // Arrange
            var stubPosition = new Point(15, 15);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;
            mockBoardModel.PieceMoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            boardController.ChangeSelection(stubPosition, stubPlayer, true);

            // Act
            var newPosition = new Point(25, 25);
            var piece = boardController.MoveSelectedPiece(newPosition);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(newPosition)).First();
            Assert.AreEqual(piece, expectedPoint.Piece);
        }
        
        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, 0, DisplayName = "No piece on point at position.")]
        [DataRow(5, 150, 150, 35, 35, 0, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, 1, DisplayName = "Piece on point at position.")]
        public void RemoveOpponentPiece_Position_PieceRemovedRaised(
            int pointCount, int x, int y, int stubX, int stubY, int expectedCallCount)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.PieceRemoved += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            var newPosition = new Point(x, y);
            boardController.RemoveOpponentPiece(newPosition, stubPlayer);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, DisplayName = "No piece on point at position.")]
        [DataRow(5, 150, 150, 35, 35, DisplayName = "No point at position.")]
        public void RemoveOpponentPiece_InvalidPosition_PieceIsNotRemoved(
            int pointCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.PieceRemoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            // Act
            var newPosition = new Point(x, y);
            boardController.RemoveOpponentPiece(newPosition, stubPlayer);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPosition)).First();
            Assert.IsNotNull(expectedPoint.Piece);
        }

        [TestMethod]
        public void RemoveOpponentPiece_ValidPosition_PieceIsRemoved()
        {
            // Arrange
            var position = new Point(15, 15);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.PieceRemoved += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(position, stubPlayer);

            // Act
            boardController.RemoveOpponentPiece(position, stubPlayer);

            // Assert
            var expectedPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(position)).First();
            Assert.IsNull(expectedPoint.Piece);
        }

        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, 0, DisplayName = "No piece on point at position.")]
        [DataRow(5, 150, 150, 35, 35, 0, DisplayName = "No point at position.")]
        [DataRow(5, 25, 25, 25, 25, 1, DisplayName = "Piece on point at position.")]
        public void ChangeSelection_Position_SelectionChangedRaised(
            int pointCount, int x, int y, int stubX, int stubY, int expectedCallCount)
        {
            // Arrange
            var position = new Point(x, y);
            var stubPoints = CreatePoints(pointCount);
            var stubBoardModel = Substitute.For<BoardModel>(stubPoints);

            var mockEventSubscriber = new MockEventSubscriber();
            stubBoardModel.NewPieceAdded += mockEventSubscriber.Handle;
            stubBoardModel.SelectionChanged += mockEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(stubBoardModel);
            boardController.AddNewPiece(new Point(stubX, stubY), stubPlayer);
            mockEventSubscriber.Reset();

            // Act
            boardController.ChangeSelection(position, stubPlayer, true);

            // Assert
            Assert.AreEqual(expectedCallCount, mockEventSubscriber.HitCount);
        }

        [DataTestMethod]
        [DataRow(5, 15, 15, 35, 35, DisplayName = "No piece on point at position.")]
        [DataRow(5, 150, 150, 35, 35, DisplayName = "No point at position.")]
        public void ChangeSelection_InvalidPosition_NoPieceSelected(
            int pointCount, int x, int y, int stubX, int stubY)
        {
            // Arrange
            var stubPosition = new Point(stubX, stubY);
            var stubPoints = CreatePoints(pointCount);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            // Act
            var position = new Point(x, y);
            boardController.ChangeSelection(position, stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(0, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_ValidPoint_PieceIsSelected()
        {
            // Arrange
            var position = new Point(25, 25);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(position, stubPlayer);

            // Act
            boardController.ChangeSelection(position, stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(1, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_OnlyOnePieceIsSelected()
        {
            // Arrange
            var stubPosition = new Point(25, 25);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            var newPosition = new Point(45, 45);
            boardController.AddNewPiece(newPosition, stubPlayer);

            // Act
            boardController.ChangeSelection(newPosition, stubPlayer, true);

            // Assert
            var selectedCount = mockBoardModel.Points.Count(p => p.Piece != null && p.Piece.IsSelected);
            Assert.AreEqual(1, selectedCount);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_NewPieceIsSelected()
        {
            // Arrange
            var stubPosition = new Point(25, 25);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            var newPosition = new Point(45, 45);
            boardController.AddNewPiece(newPosition, stubPlayer);

            // Act
            boardController.ChangeSelection(newPosition, stubPlayer, true);

            // Assert
            var newPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(newPosition)).First();
            Assert.IsTrue(newPoint.Piece.IsSelected);
        }

        [TestMethod]
        public void ChangeSelection_PieceSelected_OldPieceIsNotSelected()
        {
            // Arrange
            var stubPosition = new Point(25, 25);
            var stubPoints = CreatePoints(5);
            var mockBoardModel = Substitute.For<BoardModel>(stubPoints);

            var stubEventSubscriber = new MockEventSubscriber();
            mockBoardModel.NewPieceAdded += stubEventSubscriber.Handle;
            mockBoardModel.SelectionChanged += stubEventSubscriber.Handle;

            var stubPlayer = Substitute.For<PlayerModel>(1, Colors.Yellow);

            var boardController = new BoardController(mockBoardModel);
            boardController.AddNewPiece(stubPosition, stubPlayer);

            var newPosition = new Point(45, 45);
            boardController.AddNewPiece(newPosition, stubPlayer);

            // Act
            boardController.ChangeSelection(newPosition, stubPlayer, true);

            // Assert
            var oldPoint = mockBoardModel.Points.Where(p => p.Bounds.Contains(stubPosition)).First();
            Assert.IsFalse(oldPoint.Piece.IsSelected);
        }

        private List<PointModel> CreatePoints(int count)
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
