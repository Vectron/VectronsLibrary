using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace VectronsLibrary.DI.Tests
{
    [TestClass]
    public class RegisteredTypesTests
    {
        private interface ITestInterface
        {
        }

        [TestMethod]
        public void ReturnsAllRequestedTypes()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddSingleton<ITestInterface, TestClass1>()
                .AddSingleton<ITestInterface, TestClass2>()
                .AddSingleton<ITestInterface, TestClass2>()
                .AddRegisteredTypes()
                .BuildServiceProvider();

            // Act
            var implementations = provider.GetService<IRegisteredTypes<ITestInterface>>();

            // Assert
            Assert.AreEqual(3, implementations.Items.Count());
        }

        private class TestClass1 : ITestInterface
        {
            public TestClass1()
            {
                Assert.Fail("Constructor should not be called");
            }
        }

        private class TestClass2 : ITestInterface
        {
            public TestClass2()
            {
                Assert.Fail("Constructor should not be called");
            }
        }

        private class TestClass3 : ITestInterface
        {
            public TestClass3()
            {
                Assert.Fail("Constructor should not be called");
            }
        }
    }
}