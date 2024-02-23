using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace VectronsLibrary.DI.Tests;

/// <summary>
/// Test methods for <see cref="AssemblyResolver"/>.
/// </summary>
[TestClass]
public class AssemblyResolverTests
{
    private static readonly string[] IgnoredAssemblies = ["test1", "test2", "test3"];

    /// <summary>
    /// Test if we get <see cref="ArgumentNullException"/> when we pass <see langword="null"/> to the constructor.
    /// </summary>
    [TestMethod]
    public void ConstructorThrowsArgumentNullException()
    {
        _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(null!));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), null!));
        _ = Assert.ThrowsException<ArgumentNullException>(() => new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), [], null!));
    }

    /// <summary>
    /// Check if empty search directories are skipped.
    /// </summary>
    [TestMethod]
    public void EmptySearchDirIsSkipped()
    {
        using var assemblyResolver = new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), [], [string.Empty, null!]);

        var result = Assembly.Load("VectronsLibrary.DI.TestsAssembly");

        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Check if <see cref="FileNotFoundException"/> or <see cref="FileLoadException"/> is thrown when loading unknown assembly.
    /// </summary>
    [TestMethod]
    public void InvalidAssembliesThrowException()
    {
        using var assemblyResolver = new AssemblyResolver();

        var assemblyToLoad = new AssemblyName("test.XmlSerializers, version=1.0.0.0, culture=neutral, publicKeyToken=null")
        {
            Name = string.Empty,
        };
#if NET6_0_OR_GREATER
        _ = Assert.ThrowsException<FileNotFoundException>(() => Assembly.Load(assemblyToLoad));
#else
        _ = Assert.ThrowsException<FileLoadException>(() => Assembly.Load(assemblyToLoad));
#endif
    }

    /// <summary>
    /// Check if <see cref="FileNotFoundException"/> or <see cref="FileLoadException"/> is thrown when loading Serializer or resource assembly.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly to load.</param>
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
    public void SerializerResourcesAndIgnoredAssembliesThrowException(string assemblyName)
    {
        using var assemblyResolver = new AssemblyResolver(Mock.Of<ILogger<AssemblyResolver>>(), IgnoredAssemblies);

        _ = Assert.ThrowsException<FileNotFoundException>(() => Assembly.Load(assemblyName));
    }

    /// <summary>
    /// Check if a assembly is resolved properly.
    /// </summary>
    [TestMethod]
    public void TryResolve()
    {
        using var assemblyResolver = new AssemblyResolver();

        var result = Assembly.Load("VectronsLibrary.DI.TestsAssembly");

        Assert.IsNotNull(result);
    }
}
