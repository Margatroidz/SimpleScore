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
            track.AddMessage(new Voice(10, 0x90, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 10);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Status, 0x90);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Data1, 0x2c);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[0]).Data2, 0x64);
            track.AddMessage(new Voice(100, 0x80, 0x2c, 0x54));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Status, 0x80);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Data1, 0x2c);
            Assert.AreEqual(((Voice)((List<Message>)target.GetFieldOrProperty("messageList"))[1]).Data2, 0x54);
        }

        [TestMethod()]
        public void TestGetNote()
        {
            List<Message> list = (List<Message>)target.GetFieldOrProperty("messageList");
            list.Add(new Voice(10, 0x90, 0x2c, 0x64));
            list.Add(new Voice(100, 0x80, 0x2c, 0x54));
            target.SetFieldOrProperty("length", 100);
            Voice[] test = track.GetMessages();
            Assert.AreEqual(test[0].Status, 0x90);
            Assert.AreEqual(test[0].Data1, 0x2c);
            Assert.AreEqual(test[0].Data2, 0x64);
            Assert.AreEqual(test[1].Status, 0x80);
            Assert.AreEqual(test[1].Data1, 0x2c);
            Assert.AreEqual(test[1].Data2, 0x54);
        }

        [TestMethod()]
        public void TestPlay()
        {
            List<Message> list = (List<Message>)target.GetFieldOrProperty("messageList");
            list.Add(new Voice(10, 0x90, 0x2c, 0x64));
            list.Add(new Voice(100, 0x80, 0x2c, 0x54));
            target.SetFieldOrProperty("length", 100);
            Voice[] test = track.PlayIterator(0);
            Assert.AreEqual(test.Length, 0);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 0);
            test = track.PlayIterator(20);
            Assert.AreEqual(test[0].Status, 0x90);
            Assert.AreEqual(test[0].Data1, 0x2c);
            Assert.AreEqual(test[0].Data2, 0x64);
            Assert.AreEqual(test.Length, 1);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 1);
            test = track.PlayIterator(200);
            Assert.AreEqual(test[0].Status, 0x80);
            Assert.AreEqual(test[0].Data1, 0x2c);
            Assert.AreEqual(test[0].Data2, 0x54);
            Assert.AreEqual(test.Length, 1);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 2);
            track = new Track();
            Assert.AreEqual(track.Length, 0);
            target = new PrivateObject(track);
            list = (List<Message>)target.GetFieldOrProperty("messageList");
            list.Add(new Voice(10, 0x90, 0x2c, 0x64));
            list.Add(new Voice(50, 0x90, 0x2a, 0x64));
            list.Add(new Voice(100, 0x80, 0x2c, 0x54));
            list.Add(new Voice(200, 0x80, 0x2a, 0x54));
            target.SetFieldOrProperty("length", 200);
            Assert.AreEqual(track.Length, 200);
            test = track.PlayIterator(0);
            Assert.AreEqual(test.Length, 0);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 0);
            test = track.PlayIterator(300);
            Assert.AreEqual(test[0].Status, 0x90);
            Assert.AreEqual(test[0].Data1, 0x2c);
            Assert.AreEqual(test[0].Data2, 0x64);
            Assert.AreEqual(test[1].Status, 0x90);
            Assert.AreEqual(test[1].Data1, 0x2a);
            Assert.AreEqual(test[1].Data2, 0x64);
            Assert.AreEqual(test[2].Status, 0x80);
            Assert.AreEqual(test[2].Data1, 0x2c);
            Assert.AreEqual(test[2].Data2, 0x54);
            Assert.AreEqual(test[3].Status, 0x80);
            Assert.AreEqual(test[3].Data1, 0x2a);
            Assert.AreEqual(test[3].Data2, 0x54);
            Assert.AreEqual(test.Length, 4);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 4);
        }

        [TestMethod()]
        public void TestChangePosition()
        {
            List<Message> list = (List<Message>)target.GetFieldOrProperty("messageList");
            list.Add(new Voice(10, 0x90, 0x2c, 0x64));
            list.Add(new Voice(50, 0x90, 0x2a, 0x64));
            list.Add(new Voice(100, 0x80, 0x2c, 0x54));
            list.Add(new Voice(200, 0x80, 0x2a, 0x54));
            target.SetFieldOrProperty("length", 200);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 0);
            track.ChangePosition(25);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 1);
            track.ChangePosition(250);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 4);
            track.ChangePosition(40);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 1);
            track.ChangePosition(100);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 2);
            track.ChangePosition(10);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 0);
            track.ChangePosition(5);
            Assert.AreEqual(target.GetFieldOrProperty("position"), 0);
        }
    }
}