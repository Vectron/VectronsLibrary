using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfCollectionIsNull()
        {
            // Arrange
            ICollection<int>? collection = null;
            var items = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Act
            collection!.AddRange(items);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfEnumerableIsNull()
        {
            // Arrange
            ICollection<int>? collection = new List<int>();

            // Act
            collection.AddRange(null!);

            // Assert
        }
    }
}