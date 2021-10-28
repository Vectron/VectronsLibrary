using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

[TestClass]
public class ObjectExtensionTests
{
    [TestMethod]
    public void DontThrowIfClassObjectIsNotNull()
    {
        // Arrange
        var obj = string.Empty;

        // Act
        obj!.ThrowIfNull(nameof(obj));

        // Assert
    }

    [TestMethod]
    public void DontThrowIfStructObjectIsNotNull()
    {
        // Arrange
        int? obj = 1;

        // Act
        obj.ThrowIfNull(nameof(obj));

        // Assert
    }

    [TestMethod]
    public void ThrowsIfClassObjectIsNull()
    {
        // Arrange
        string? value = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => value!.ThrowIfNull("Test object"));
    }

    [TestMethod]
    public void ThrowsIfStructObjectIsNull()
    {
        // Arrange
        int? value = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => value.ThrowIfNull("Test object"));
    }
}