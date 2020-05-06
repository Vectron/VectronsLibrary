using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class TaskExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void DoesNotThrowException()
        {
            //Arrange
            var logger = GetLoggerMock<TaskExtensionsTests>();

            //Act
            Task.Run(() => throw new NullReferenceException())
                 //.LogExceptionsAsync(logger.Object)
                 .Wait();

            //Assert
            logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void RunsActionOnSpecificException()
        {
            //Arrange
            var logger = GetLoggerMock<TaskExtensionsTests>();

            //Act
            Task.Run(() => throw new NullReferenceException())
                 .LogExceptionsAsync<NullReferenceException>(logger.Object, x => Assert.IsInstanceOfType(x, typeof(NullReferenceException)))
                 .Wait();

            //Assert
            logger.VerifyAll();
        }

        protected static Mock<ILogger<T>> GetLoggerMock<T>()
        {
            var mock = new Mock<ILogger<T>>();
            mock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()))
                .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((l, e, s, ex, f) =>
                {
                    if (ex != null)
                    {
                        Console.WriteLine($"{Enum.GetName(typeof(LogLevel), l)}: {f(s, ex)}\n{ex?.StackTrace}");
                    }
                    else
                    {
                        Console.WriteLine($"{Enum.GetName(typeof(LogLevel), l)}: {f(s, ex)}");
                    }
                });
            return mock;
        }
    }
}