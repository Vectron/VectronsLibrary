﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class TaskExtensionsTests
    {
        private readonly Action<ArgumentNullException> dummyAction = _ => { };

        [TestMethod]
        public void ActionIsCalledForFilteredException()
        {
            // Arrange
            Expression<Action<Action<InvalidOperationException>>> callback = m => m(It.IsAny<InvalidOperationException>());

            var logger = new Mock<ILogger>();
            var testMethod = new Mock<Action<InvalidOperationException>>();
            testMethod.Setup(callback).Verifiable();

            // Act
            Task.Run(() => throw new InvalidOperationException())
                .LogExceptionsAsync(logger.Object, testMethod.Object)
                .Wait();

            // Assert
            testMethod.Verify(callback, Times.Once);
        }

        [TestMethod]
        public void AllNonFilteredExceptionsGetLogged()
        {
            // Arrange
            var logger = new Mock<ILogger>();

            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            logger.Setup(expression).Verifiable();

            // Act
            Task.Factory.StartNew(
                () =>
                {
                    _ = Task.Factory.StartNew(() => throw new NotImplementedException(), TaskCreationOptions.AttachedToParent);
                    throw new InvalidOperationException();
                })
            .LogExceptionsAsync(logger.Object, dummyAction)
            .Wait();

            // Assert
            logger.Verify(expression, Times.Exactly(2));
        }

        [TestMethod]
        public void EveryChildExceptionIsLogged()
        {
            // Arrange
            var logger = new Mock<ILogger>();

            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            logger.Setup(expression).Verifiable();

            // Act
            Task.Factory.StartNew(
                () =>
                {
                    _ = Task.Factory.StartNew(() => throw new NotImplementedException(), TaskCreationOptions.AttachedToParent);
                    throw new InvalidOperationException();
                })
            .LogExceptionsAsync(logger.Object)
            .Wait();

            // Assert
            logger.Verify(expression, Times.Exactly(2));
        }

        [TestMethod]
        public void EveryChildExceptionIsLoggedInReverseOrder()
        {
            // Arrange
            var logger = new Mock<ILogger>(MockBehavior.Strict);
            var sequence = new MockSequence();
            Expression<Action<ILogger>> call1 = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<NotImplementedException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());
            Expression<Action<ILogger>> call2 = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            logger.InSequence(sequence).Setup(call1).Verifiable();
            logger.InSequence(sequence).Setup(call2).Verifiable();

            // Act
            Task.Factory.StartNew(
                () =>
                {
                    _ = Task.Factory.StartNew(() => throw new NotImplementedException(), TaskCreationOptions.AttachedToParent);
                    throw new InvalidOperationException();
                })
            .LogExceptionsAsync(logger.Object)
            .Wait();

            // Assert
            logger.Verify(call1, Times.Once);
            logger.Verify(call2, Times.Once);
        }

        [TestMethod]
        public void EveryChildExceptionIsLoggedInReverseOrderGeneric()
        {
            // Arrange
            var logger = new Mock<ILogger>(MockBehavior.Strict);
            var sequence = new MockSequence();
            Expression<Action<ILogger>> call1 = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<NotImplementedException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());
            Expression<Action<ILogger>> call2 = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            logger.InSequence(sequence).Setup(call1).Verifiable();
            logger.InSequence(sequence).Setup(call2).Verifiable();

            // Act
            Task.Factory.StartNew(
                () =>
                {
                    _ = Task.Factory.StartNew(() => throw new NotImplementedException(), TaskCreationOptions.AttachedToParent);
                    throw new InvalidOperationException();
                })
            .LogExceptionsAsync(logger.Object, dummyAction)
            .Wait();

            // Assert
            logger.Verify(call1, Times.Once);
            logger.Verify(call2, Times.Once);
        }

        [TestMethod]
        public void ExceptionsLoggedWithErrorLevel()
        {
            // Arrange
            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            var logger = new Mock<ILogger>();

            // Act
            Task.Run(() => throw new InvalidOperationException())
                .LogExceptionsAsync(logger.Object)
                .Wait();

            // Assert
            logger.Verify(expression, Times.Once);
        }

        [TestMethod]
        public void ExceptionsLoggedWithErrorLevelGeneric()
        {
            // Arrange
            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            var logger = new Mock<ILogger>();

            // Act
            Task.Run(() => throw new InvalidOperationException())
                .LogExceptionsAsync(logger.Object, dummyAction)
                .Wait();

            // Assert
            logger.Verify(expression, Times.Once);
        }

        [TestMethod]
        public void FilteredExceptionGoesToCallbackRestGetsLogged()
        {
            // Arrange
            var logger = new Mock<ILogger>();

            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            logger.Setup(expression).Verifiable();

            // Act
            Task.Factory.StartNew(
                () =>
                {
                    _ = Task.Factory.StartNew(() => throw new NotImplementedException(), TaskCreationOptions.AttachedToParent);
                    throw new InvalidOperationException();
                })
            .LogExceptionsAsync<NotImplementedException>(logger.Object, x => { })
            .Wait();

            // Assert
            logger.Verify(expression, Times.Once);
        }

        [TestMethod]
        public void ThrowIfActionIsNullGeneric()
        {
            // Arrange
            var logger = Mock.Of<ILogger>();

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync<Exception>(Task.CompletedTask, logger, null!));
        }

        [TestMethod]
        public void ThrowIfLoggerIsNull()
        {
            // Arrange

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(Task.CompletedTask, null!));
        }

        [TestMethod]
        public void ThrowIfLoggerIsNullGeneric()
        {
            // Arrange

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(Task.CompletedTask, null!, dummyAction));
        }

        [TestMethod]
        public void ThrowIfTaskIsNull()
        {
            // Arrange
            var logger = Mock.Of<ILogger>();

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(null!, logger));
        }

        [TestMethod]
        public void ThrowIfTaskIsNullGeneric()
        {
            // Arrange
            var logger = Mock.Of<ILogger>();

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(null!, logger, dummyAction));
        }

        [TestMethod]
        public void ThrownExceptionIsLogged()
        {
            // Arrange
            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            var logger = new Mock<ILogger>();

            // Act
            Task.Run(() => throw new InvalidOperationException())
                .LogExceptionsAsync(logger.Object)
                .Wait();

            // Assert
            logger.Verify(expression, Times.Once);
        }

        [TestMethod]
        public void ThrownExceptionIsLoggedGeneric()
        {
            // Arrange
            Expression<Action<ILogger>> expression = mockedTypeInstance
                => mockedTypeInstance.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<InvalidOperationException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>());

            var logger = new Mock<ILogger>();

            // Act
            Task.Run(() => throw new InvalidOperationException())
                .LogExceptionsAsync(logger.Object, dummyAction)
                .Wait();

            // Assert
            logger.Verify(expression, Times.Once);
        }
    }
}