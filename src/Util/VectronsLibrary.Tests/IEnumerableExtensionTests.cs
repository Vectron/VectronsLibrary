using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

[TestClass]
public class IEnumerableExtensionTests
{
    [TestMethod]
    public void ForEach()
    {
        // Arrange
        var items = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var itemsLooped = 0;

        // Act
        items.ForEach(x => itemsLooped++);

        // Assert
        Assert.AreEqual(items.Length, itemsLooped);
    }

    [TestMethod]
    public void ForEachThrowsExceptionIfActionIsNull()
    {
        // Arrange
        IEnumerable<int> items = new List<int>();

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => IEnumerableExtension.ForEach(items, null!));
    }

    [TestMethod]
    public void ForEachThrowsExceptionIfEnumarableIsNull()
    {
        // Arrange
        IEnumerable<int>? enumerable = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => enumerable!.ForEach(_ => { }));
    }

    [Obsolete("The ToCSV function is marked as obsolete, so test is also obsolete")]
    [TestMethod]
    public void ToCSV()
    {
        // Arrange
        var items = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var expectedResult = "0,1,2,3,4,5,6,7,8,9";

        // Act
        var result = items.ToCSV();

        // Assert
        Assert.AreEqual(expectedResult, result);
    }

    [Obsolete("The ToCSV function is marked as obsolete, so test is also obsolete")]
    [TestMethod]
    public void ToCSVWithSepperator()
    {
        // Arrange
        var items = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var expectedResult = "0;1;2;3;4;5;6;7;8;9";

        // Act
        var result = items.ToCSV(';');

        // Assert
        Assert.AreEqual(expectedResult, result);
    }
}