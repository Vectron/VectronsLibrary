using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectronsLibrary.Extensions;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void DontThrowIfClassObjectIsNull()
        {
            // Arrange
            string? obj = string.Empty;

            // Act
            obj!.ThrowIfNull(nameof(obj));

            // Assert
        }

        [TestMethod]
        public void DontThrowIfStructObjectIsNull()
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

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => ObjectExtension.ThrowIfNull<string>(null!, "Test object"));
        }

        [TestMethod]
        public void ThrowsIfStructObjectIsNull()
        {
            // Arrange

            // Act

            // Assert
            _ = Assert.ThrowsException<ArgumentNullException>(() => ObjectExtension.ThrowIfNull<int>(null, "Test object"));
        }
    }
}