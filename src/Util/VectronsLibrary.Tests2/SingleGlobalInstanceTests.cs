using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Guid("2B0D126E-027B-43CE-8A27-DAFC51C03BD5")]

namespace VectronsLibrary.Tests2;

/// <summary>
/// Tests for the <see cref="SingleGlobalInstance"/> class.
/// </summary>
[TestClass]
public class SingleGlobalInstanceTests
{
    /// <summary>
    /// Test if the assembly guid is returned.
    /// </summary>
    [TestMethod]
    public void ApplicationGuidIsReadProperly()
    {
        var guid = SingleGlobalInstance.GetApplicationGui();
        Assert.AreEqual("2B0D126E-027B-43CE-8A27-DAFC51C03BD5", guid);
    }
}