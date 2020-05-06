using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
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
        [ExpectedException(typeof(System.NullReferenceException))]
        public void ForEachThrowsExceptionIfActionIsNull()
        {
            // Arrange
            List<int> items = null;

            // Act
            items.ForEach(x => { });

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void ForEachThrowsExceptionIfEnumarableIsNull()
        {
            // Arrange
            List<int> items = null;

            // Act
            items.ForEach(null);

            // Assert
        }

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
}