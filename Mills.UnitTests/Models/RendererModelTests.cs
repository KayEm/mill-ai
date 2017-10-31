using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mills.Models;
using System.ComponentModel;
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
            var rendererModel = new RendererModel(null);
            
            var mockEventSubscriber = new MockEventSubscriber();
            rendererModel.PropertyChanged += mockEventSubscriber.Handle;

            // Act
            rendererModel.CurrentPlayerColor = new SolidColorBrush() { Color = Colors.Black };

            // Assert
            Assert.AreEqual(1, mockEventSubscriber.HitCount);
        }

        [TestMethod]
        public void UserMessage_EventSubscriber_PropertyChangedRaised()
        {
            // Arrange
            var rendererModel = new RendererModel(null);

            var mockEventSubscriber = new MockEventSubscriber();
            rendererModel.PropertyChanged += mockEventSubscriber.Handle;

            // Act
            rendererModel.UserMessage = "Test message";

            // Assert
            Assert.AreEqual(1, mockEventSubscriber.HitCount);
        }
    }
}
