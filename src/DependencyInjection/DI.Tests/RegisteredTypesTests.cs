using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.DI.Tests;

[TestClass]
public class RegisteredTypesTests
{
    private interface ITestInterface
    {
    }

    private interface ITestInterface2 : ITestInterface
    {
    }

    [TestMethod]
    public void ReturnsAllRequestedTypes()
    {
        // Arrange
        var provider = new ServiceCollection()
            .AddSingleton<ITestInterface, TestClass1>()
            .AddSingleton<ITestInterface2, TestClass1>()
            .AddSingleton<ITestInterface, TestClass2>()
            .AddSingleton<ITestInterface, TestClass3>()
            .AddSingleton<ITestInterface, TestClass3>()
            .AddSingleton<ITestInterface, TestClass3>()
            .AddRegisteredTypes()
            .BuildServiceProvider();

        // Act
        var implementations = provider.GetRequiredService<IRegisteredTypes<ITestInterface>>();

        // Assert
        Assert.AreEqual(3, implementations.Items.Count());
    }

    private class TestClass1 : ITestInterface2
    {
        [ExcludeFromCodeCoverage]
        public TestClass1()
            => Assert.Fail("Constructor should not be called");
    }

    private class TestClass2 : ITestInterface
    {
        [ExcludeFromCodeCoverage]
        public TestClass2()
            => Assert.Fail("Constructor should not be called");
    }

    private class TestClass3 : ITestInterface
    {
        [ExcludeFromCodeCoverage]
        public TestClass3()
            => Assert.Fail("Constructor should not be called");
    }
}