using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests;

/// <summary>
/// Tests for the <see cref="IEnumerableExtension"/> class.
/// </summary>
[TestClass]
public class IEnumerableExtensionTests
{
    /// <summary>
    /// Test if the action is run on every item.
    /// </summary>
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

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="Action"/> is null.
    /// </summary>
    [TestMethod]
    public void ForEachThrowsExceptionIfActionIsNull()
    {
        // Arrange
        IEnumerable<int> items = [];

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => IEnumerableExtension.ForEach(items, null!));
    }

    /// <summary>
    /// Test if <see cref="ArgumentNullException"/> is thrown when the <see cref="IEnumerable{T}"/> is null.
    /// </summary>
    [TestMethod]
    public void ForEachThrowsExceptionIfIEnumerableIsNull()
    {
        // Arrange
        IEnumerable<int>? enumerable = null;

        // Act

        // Assert
        _ = Assert.ThrowsException<ArgumentNullException>(() => enumerable!.ForEach(_ => { }));
    }

    /// <summary>
    /// Test if the items are converted to a csv format string.
    /// </summary>
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

    /// <summary>
    /// Test if the items are converted to a csv format string with a custom separator.
    /// </summary>
    [Obsolete("The ToCSV function is marked as obsolete, so test is also obsolete")]
    [TestMethod]
    public void ToCSVWithSeparator()
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
