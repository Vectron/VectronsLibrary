using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.DI.Tests;

/// <summary>
/// Test for the <see cref="AssemblyTypeLoader"/> class.
/// </summary>
[TestClass]
public class AssemblyTypeLoaderTests
{
    /// <summary>
    /// Check if the returned Assembly Directory is valid.
    /// </summary>
    [TestMethod]
    public void AssemblyDirectoryReturnsValidPath()
    {
        // Arrange

        // Act
        var exists = Directory.Exists(AssemblyTypeLoader.AssemblyDirectory);

        // Assert
        Assert.IsTrue(exists);
    }

    /// <summary>
    /// Check if LoadTypesFromAssemblySafe returns empty array when nothing is found.
    /// </summary>
    [TestMethod]
    public void LoadTypesFromAssemblySafeReturnsEmptyArray()
    {
        // Arrange

        // Act
        var types = AssemblyTypeLoader.LoadTypesFromAssemblySafe(string.Empty);

        // Assert
        Assert.IsTrue(types.Length == 0);
    }

    /// <summary>
    /// Check if LoadTypesFromAssemblySafe returns the containing types.
    /// </summary>
    [TestMethod]
    public void LoadTypesFromAssemblySafeReturnsTypes()
    {
        // Arrange
        var currentAssembly = Assembly.GetExecutingAssembly();

        // Act
        var types = AssemblyTypeLoader.LoadTypesFromAssemblySafe(currentAssembly.GetName().Name + ".dll");

        // Assert
        Assert.IsTrue(types.Length > 0);
    }
}
