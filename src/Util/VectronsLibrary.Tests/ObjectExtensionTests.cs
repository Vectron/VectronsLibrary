using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="ObjectExtension"/> class.
/// </summary>
[TestClass]
public class ObjectExtensionTests
{
    /// <summary>
    /// Test that <see cref="ArgumentNullException"/> is thrown when the object is not <see langword="null"/>.
    /// </summary>
    [TestMethod]
    public void DoNotThrowIfClassObjectIsNotNull()
    {
        // Arrange
        var obj = string.Empty;

        // Act
        obj!.ThrowIfNull(nameof(obj));

        // Assert
    }

    /// <summary>
    /// Test that <see cref="ArgumentNullException"/> is thrown when the struct is not <see langword="null"/>.
    /// </summary>
    [TestMethod]
    public void DoNotThrowIfStructObjectIsNotNull()
    {
        // Arrange
        int? obj = 1;

        // Act
        obj.ThrowIfNull(nameof(obj));

        // Assert
    }

    /// <summary>
    /// Test that <see cref="ArgumentNullException"/> is thrown when the object is <see langword="null"/>.
    /// </summary>
    [TestMethod]
    public void ThrowsIfClassObjectIsNull()
    {
        // Arrange
        string? value = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => value!.ThrowIfNull("Test object"));
    }

    /// <summary>
    /// Test that <see cref="ArgumentNullException"/> is thrown when the struct is <see langword="null"/>.
    /// </summary>
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