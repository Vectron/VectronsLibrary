using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="CollectionExtensions"/> class.
/// </summary>
[TestClass]
public class ICollectionExtensionTests
{
    /// <summary>
    /// Test if add range adds all the items to the collection.
    /// </summary>
    [TestMethod]
    public void AddRangeAddsAllItems()
    {
        // Arrange
        ICollection<int> collection = new List<int>();
        var items = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // Act
        collection.AddRange(items);

        // Assert
        CollectionAssert.AreEqual(items, (System.Collections.ICollection)collection);
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="ICollection{T}"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowsIfCollectionIsNull()
    {
        // Arrange
        var items = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => ICollectionExtension.AddRange(null!, items));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="IEnumerable{T}"/> is null.
    /// </summary>
    [TestMethod]
    public void ThrowsIfEnumerableIsNull()
    {
        // Arrange
        ICollection<int>? collection = new List<int>();

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => ICollectionExtension.AddRange(collection, null!));
    }
}
