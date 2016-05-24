using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class VoiceTest
    {
        Voice voice;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            voice = new Voice(0, 0x80, 0x5c, 0x64);
            target = new PrivateObject(voice);
        }

        [TestMethod()]
        public void TestVoice()
        {
            Assert.AreEqual(voice.Time, 0);
            Assert.AreEqual(voice.Status, 0x80);
            Assert.AreEqual(voice.Data1, 0x5c);
            Assert.AreEqual(voice.Data2, 0x64);
            voice = new Voice(12345, 0x90, 0x6a, 0x0);
            Assert.AreEqual(voice.Time, 12345);
            Assert.AreEqual(voice.Status, 0x90);
            Assert.AreEqual(voice.Data1, 0x6a);
            Assert.AreEqual(voice.Data2, 0x0);
        }
    }
}
