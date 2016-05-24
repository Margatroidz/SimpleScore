using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class ModeTest
    {
        Mode mode;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            mode = new Mode(0, 0x90, 0x5c, 0x64);
            target = new PrivateObject(mode);
        }

        [TestMethod()]
        public void TestMode()
        {
            Assert.AreEqual(mode.Time, 0);
            Assert.AreEqual(mode.Status, 0x90);
            Assert.AreEqual(mode.Data1, 0x5c);
            Assert.AreEqual(mode.Data2, 0x64);
            mode = new Mode(54321, 0x80, 0x3b, 0x64);
            Assert.AreEqual(mode.Time, 54321);
            Assert.AreEqual(mode.Status, 0x80);
            Assert.AreEqual(mode.Data1, 0x3b);
            Assert.AreEqual(mode.Data2, 0x64);
        }
    }
}