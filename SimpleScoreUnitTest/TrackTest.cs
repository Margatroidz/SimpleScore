using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class TrackTest
    {
        Track track;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            track = new Track();
            target = new PrivateObject(track);
        }

        [TestMethod()]
        public void TestCreateNote()
        {
            Assert.AreEqual(target.GetFieldOrProperty("length"), 0);
            track.AddMessage(new Message(Message.Type.Voice, 10, 0x90, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 10);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Status, 0x90);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Data1, 0x2c);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Data2, 0x64);
            track.AddMessage(new Message(Message.Type.Voice, 100, 0x80, 0x2c, 0x54));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Status, 0x80);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Data1, 0x2c);
            Assert.AreEqual(((Message)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Data2, 0x54);
        }

        [TestMethod()]
        public void TestGetNote()
        {
            List<Message> list = (List<Message>)target.GetFieldOrProperty("messageList");
            list.Add(new Message(Message.Type.Voice, 10, 0x90, 0x2c, 0x64));
            list.Add(new Message(Message.Type.Voice, 100, 0x80, 0x2c, 0x54));
            target.SetFieldOrProperty("length", 100);
            Message[] test = track.GetMessages();
            Assert.AreEqual(test[0].Status, 0x90);
            Assert.AreEqual(test[0].Data1, 0x2c);
            Assert.AreEqual(test[0].Data2, 0x64);
            Assert.AreEqual(test[1].Status, 0x80);
            Assert.AreEqual(test[1].Data1, 0x2c);
            Assert.AreEqual(test[1].Data2, 0x54);
        }
    }
}