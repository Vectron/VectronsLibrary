using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfClassObjectIsNull()
        {
            //Arrange
            string obj = null;

            //Act
            obj.ThrowIfNull(nameof(obj));

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsIfStructObjectIsNull()
        {
            //Arrange
            int? obj = null;

            //Act
            obj.ThrowIfNull(nameof(obj));

            //Assert
        }
    }
}