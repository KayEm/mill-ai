using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Models;
using System.Windows.Media;
using Mills.UnitTests.Eventing;

namespace Mills.UnitTests.Models
{
    [TestClass]
    public class RendererModelTests
    {
        [TestMethod]
        public void CurrentPlayerColor_EventSubscriber_PropertyChangedRaised()
        {
            // Arrange
            var boardViewModel = new BoardViewModel(null, null, null);
            
            var mockEventSubscriber = new MockEventSubscriber();
            boardViewModel.PropertyChanged += mockEventSubscriber.Handle;

            // Act
            boardViewModel.CurrentPlayerColor = new SolidColorBrush() { Color = Colors.Black };

            // Assert
            Assert.AreEqual(1, mockEventSubscriber.HitCount);
        }

        [TestMethod]
        public void UserMessage_EventSubscriber_PropertyChangedRaised()
        {
            // Arrange
            var boardViewModel = new BoardViewModel(null, null, null);

            var mockEventSubscriber = new MockEventSubscriber();
            boardViewModel.PropertyChanged += mockEventSubscriber.Handle;

            // Act
            boardViewModel.UserMessage = "Test message";

            // Assert
            Assert.AreEqual(1, mockEventSubscriber.HitCount);
        }
    }
}
