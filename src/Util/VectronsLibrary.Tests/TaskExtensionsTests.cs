using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="Extensions.TaskExtensions"/> class.
/// </summary>
[TestClass]
public class TaskExtensionsTests
{
    private readonly Action<ArgumentNullException> dummyAction = _ => { };

    /// <summary>
    /// Test if the action is called for the requested exceptions.
    /// </summary>
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

    /// <summary>
    /// Test if all other exceptions are logged.
    /// </summary>
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
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

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

    /// <summary>
    /// Test if inherited exceptions from the filter exception are logged.
    /// </summary>
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
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

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

    /// <summary>
    /// Test if inherited exceptions from the filter exception are logged in reversed order.
    /// </summary>
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
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());
        Expression<Action<ILogger>> call2 = mockedTypeInstance
            => mockedTypeInstance.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<InvalidOperationException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

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

    /// <summary>
    /// Test if inherited exceptions from the filter exception are logged in reversed order.
    /// </summary>
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
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());
        Expression<Action<ILogger>> call2 = mockedTypeInstance
            => mockedTypeInstance.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<InvalidOperationException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

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

    /// <summary>
    /// Test if exceptions are logged with the <see cref="LogLevel.Error"/> level.
    /// </summary>
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
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

        var logger = new Mock<ILogger>();

        // Act
        Task.Run(() => throw new InvalidOperationException())
            .LogExceptionsAsync(logger.Object)
            .Wait();

        // Assert
        logger.Verify(expression, Times.Once);
    }

    /// <summary>
    /// Test if exceptions are logged with the <see cref="LogLevel.Error"/> level.
    /// </summary>
    [TestMethod]
    public void ExceptionsLoggedWithErrorLevelGeneric()
    {
        // Arrange
        Expression<Action<ILogger>> expression = mockedTypeInstance
            => mockedTypeInstance.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

        var logger = new Mock<ILogger>();

        // Act
        Task.Run(() => throw new InvalidOperationException())
            .LogExceptionsAsync(logger.Object, dummyAction)
            .Wait();

        // Assert
        logger.Verify(expression, Times.Once);
    }

    /// <summary>
    /// Test if the filtered exception calls the callback, rest gets logged.
    /// </summary>
    [TestMethod]
    public void FilteredExceptionGoesToCallBackRestGetsLogged()
    {
        // Arrange
        var logger = new Mock<ILogger>();

        Expression<Action<ILogger>> expression = mockedTypeInstance
            => mockedTypeInstance.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

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

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="Action"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowIfActionIsNullGeneric()
    {
        // Arrange
        var logger = Mock.Of<ILogger>();

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync<Exception>(Task.CompletedTask, logger, null!));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="ILogger"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowIfLoggerIsNull()
    {
        // Arrange
        ILogger? logger = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(Task.CompletedTask, logger!));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="ILogger"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowIfLoggerIsNullGeneric()
    {
        // Arrange
        ILogger? logger = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(Task.CompletedTask, logger!, dummyAction));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="Task"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowIfTaskIsNull()
    {
        // Arrange
        var logger = Mock.Of<ILogger>();

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(null!, logger));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="Task"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowIfTaskIsNullGeneric()
    {
        // Arrange
        var logger = Mock.Of<ILogger>();

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => Extensions.TaskExtensions.LogExceptionsAsync(null!, logger, dummyAction));
    }

    /// <summary>
    /// Test if exceptions are logged.
    /// </summary>
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
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

        var logger = new Mock<ILogger>();

        // Act
        Task.Run(() => throw new InvalidOperationException())
            .LogExceptionsAsync(logger.Object)
            .Wait();

        // Assert
        logger.Verify(expression, Times.Once);
    }

    /// <summary>
    /// Test if exceptions are logged.
    /// </summary>
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
                It.IsAny<Func<It.IsAnyType, Exception?, string>>());

        var logger = new Mock<ILogger>();

        // Act
        Task.Run(() => throw new InvalidOperationException())
            .LogExceptionsAsync(logger.Object, dummyAction)
            .Wait();

        // Assert
        logger.Verify(expression, Times.Once);
    }
}
