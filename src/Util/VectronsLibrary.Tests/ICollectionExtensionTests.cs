using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

[TestClass]
public class ICollectionExtensionTests
{
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

    [TestMethod]
    public void ThrowsIfCollectionIsNull()
    {
        // Arrange
        var items = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => ICollectionExtension.AddRange(null!, items));
    }

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