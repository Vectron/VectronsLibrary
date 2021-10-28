using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.DI.Tests;

[TestClass]
public class HelperTests
{
    [TestMethod]
    public void AssemblyDirectoryReturnsValidPath()
    {
        // Arrange

        // Act
        var excists = Directory.Exists(Helper.AssemblyDirectory);

        // Assert
        Assert.IsTrue(excists);
    }

    [TestMethod]
    public void LoadTypesFromAssemblySafeReturnsEmptyArray()
    {
        // Arrange

        // Act
        var types = Helper.LoadTypesFromAssemblySafe(string.Empty);

        // Assert
        Assert.IsTrue(types.Length == 0);
    }

    [TestMethod]
    public void LoadTypesFromAssemblySafeReturnsTypes()
    {
        // Arrange
        var currentAssembly = Assembly.GetExecutingAssembly();

        // Act
        var types = Helper.LoadTypesFromAssemblySafe(currentAssembly.GetName().Name + ".dll");

        // Assert
        Assert.IsTrue(types.Length > 0);
    }
}