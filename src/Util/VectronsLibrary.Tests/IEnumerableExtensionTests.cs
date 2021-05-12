using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ForEachThrowsExceptionIfActionIsNull()
        {
            // Arrange
            IEnumerable<int> items = new List<int>();

            // Act
            items.ForEach(null!);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ForEachThrowsExceptionIfEnumarableIsNull()
        {
            // Arrange
            IEnumerable<int>? items = null;

            // Act
            items!.ForEach(x => { });

            // Assert
        }

        [TestMethod]
        public void ToCSV()
        {
            // Arrange
            var items = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expectedResult = "0,1,2,3,4,5,6,7,8,9";

#pragma warning disable 612, 618

            // Act
            var result = items.ToCSV();
#pragma warning restore 612, 618

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ToCSVWithSepperator()
        {
            // Arrange
            var items = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expectedResult = "0;1;2;3;4;5;6;7;8;9";

#pragma warning disable 612, 618

            // Act
            var result = items.ToCSV(';');
#pragma warning restore 612, 618

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}