using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace VectronsLibrary.DI.Tests
{
    [TestClass]
    public class AssemblyResolverTests
    {
        [TestMethod]
        public void ConstructorThrowsArgumentNullException()
        {
            _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(null!));
            _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), null!));
            _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), Array.Empty<string>(), null!));
        }

        [TestMethod]
        public void EmptySearchDirIsSkipped()
        {
            _ = new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), Array.Empty<string>(), new string[] { string.Empty, null! });

            var result = Assembly.Load("VectronsLibrary.DI.TestsAssembly");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InvallidAssembliesShouldBeSkipped()
        {
            _ = new AssemblyResolver();

            var assemblyToLoad = new AssemblyName("test.XmlSerializers, version=1.0.0.0, culture=neutral, publicKeyToken=null")
            {
                Name = string.Empty,
            };

            _ = Assert.ThrowsException<FileLoadException>(() => Assembly.Load(assemblyToLoad));
        }

        [TestMethod]
        [DataRow("test.XmlSerializers, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("test.resources, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("test.xmlserializers, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("test.Resources, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("MyAssembly, version=1.0.0.0, culture=en-us, publicKeyToken=null")]
        [DataRow("test1, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("test2, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("test3, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        [DataRow("Test1, version=1.0.0.0, culture=neutral, publicKeyToken=null")]
        public void SerializerResourcesAndIgnoredAssembliesShouldNotBeFound(string assemblyName)
        {
            _ = new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), new[] { "test1", "test2", "test3" });

            _ = Assert.ThrowsException<FileNotFoundException>(() => Assembly.Load(assemblyName));
        }

        [TestMethod]
        public void TryResolve()
        {
            _ = new AssemblyResolver();

            var result = Assembly.Load("VectronsLibrary.DI.TestsAssembly");

            Assert.IsNotNull(result);
        }
    }
}