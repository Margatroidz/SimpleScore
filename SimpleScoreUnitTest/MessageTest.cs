using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class MessageTest
    {
        Message message;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            message = new Message(Message.Type.Voice, 0, 0x80, 0x5c, 0x64);
            target = new PrivateObject(message);
        }

        [TestMethod()]
        public void TestMessage()
        {
            Assert.AreEqual(message.Time, 0);
            Assert.AreEqual(message.Status, 0x80);
            Assert.AreEqual(message.Data1, 0x5c);
            Assert.AreEqual(message.Data2, 0x64);
            message = new Message(Message.Type.Meta, 12345, 0x90, 0x6a, 0x0);
            Assert.AreEqual(message.Time, 12345);
            Assert.AreEqual(message.Status, 0x90);
            Assert.AreEqual(message.Data1, 0x6a);
            Assert.AreEqual(message.Data2, 0x0);
        }
    }
}
