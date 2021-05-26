using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Guid("2B0D126E-027B-43CE-8A27-DAFC51C03BD5")]

namespace VectronsLibrary.Tests2
{
    [TestClass]
    public class SingleGlobalInstanceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var guid = SingleGlobalInstance.GetApplicationGui();
            Assert.AreEqual("2B0D126E-027B-43CE-8A27-DAFC51C03BD5", guid);
        }
    }
}