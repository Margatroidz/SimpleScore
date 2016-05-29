using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class ScoreTest
    {
        int testEventCount;
        Score score;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            testEventCount = 0;
            score = new Score();
            target = new PrivateObject(score);
        }

        [TestMethod()]
        public void TestScore()
        {
            score = new Score();
            Assert.AreEqual(target.GetFieldOrProperty("Name"), string.Empty);
            Assert.AreEqual(score.Tick, 0);
            Assert.AreEqual(score.Length, 0);
            Assert.AreEqual(score.Semiquaver, 0f);
            Assert.AreEqual(score.TrackCount, 0);
            score.Name = "test";
            Assert.AreEqual(score.Name, "test");
        }

        [TestMethod()]
        public void TestClear()
        {
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 0);
            ((List<Track>)target.GetFieldOrProperty("trackList")).Add(new Track());
            ((List<Track>)target.GetFieldOrProperty("trackList")).Add(new Track());
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 2);
            score.Clear();
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 0);
        }

        [TestMethod()]
        public void TestCreateTrack()
        {
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 0);
            score.CreateTrack();
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 1);
            score.CreateTrack();
            score.CreateTrack();
            Assert.AreEqual(((List<Track>)target.GetFieldOrProperty("trackList")).Count, 3);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception), "音軌超出範圍")]
        public void TestCreateNote()
        {
            List<Track> trackList = ((List<Track>)target.GetFieldOrProperty("trackList"));
            trackList.Add(new Track());
            score.CreateMessage(0, new Message(Message.Type.Voice,0, 0x90, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 0);
            Assert.AreEqual(trackList[0].GetMessages().Length, 1);
            score.CreateMessage(0, new Message(Message.Type.Voice,100, 0x80, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(trackList[0].GetMessages().Length, 2);
            score.CreateMessage(0, new Message(Message.Type.Voice,0, 0x90, 0x1a, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(trackList[0].GetMessages().Length, 3);
            score.CreateMessage(0, new Message(Message.Type.Voice,1000, 0x80, 0x1a, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 1000);
            Assert.AreEqual(trackList[0].GetMessages().Length, 4);
            score.CreateMessage(1, new Message(Message.Type.Voice,0, 0x80, 0x1a, 0x64));
        }

        [TestMethod()]
        public void TestGetTrack()
        {
            List<Track> trackList = ((List<Track>)target.GetFieldOrProperty("trackList"));
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList[0].AddMessage(new Message(Message.Type.Voice,0, 0x90, 0x2c, 0x64));
            trackList[0].AddMessage(new Message(Message.Type.Voice,0, 0x80, 0x2c, 0x64));
            trackList[0].AddMessage(new Message(Message.Type.Voice,100, 0x90, 0x1a, 0x64));
            trackList[0].AddMessage(new Message(Message.Type.Voice,1000, 0x80, 0x1a, 0x64));
            trackList[1].AddMessage(new Message(Message.Type.Voice,0, 0x90, 0x20, 0x64));
            trackList[1].AddMessage(new Message(Message.Type.Voice,10, 0x80, 0x20, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,0, 0x90, 0x1c, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,0, 0x90, 0x1d, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,100, 0x80, 0x1c, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,100, 0x90, 0x25, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,500, 0x80, 0x25, 0x64));
            trackList[2].AddMessage(new Message(Message.Type.Voice,500, 0x80, 0x1d, 0x64));

            Message[] result = score.GetTrack(0);
            Assert.AreEqual(result.Length, 4);
            Assert.AreEqual(result[0].Status, 0x90);
            Assert.AreEqual(result[0].Data1, 0x2c);
            Assert.AreEqual(result[0].Data2, 0x64);
            Assert.AreEqual(result[3].Status, 0x80);
            Assert.AreEqual(result[3].Data1, 0x1a);
            Assert.AreEqual(result[3].Data2, 0x64);

            result = score.GetTrack(1);
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Status, 0x90);
            Assert.AreEqual(result[0].Data1, 0x20);
            Assert.AreEqual(result[0].Data2, 0x64);
            Assert.AreEqual(result[1].Status, 0x80);
            Assert.AreEqual(result[1].Data1, 0x20);
            Assert.AreEqual(result[1].Data2, 0x64);

            result = score.GetTrack(2);
            Assert.AreEqual(result.Length, 6);
            Assert.AreEqual(result[0].Status, 0x90);
            Assert.AreEqual(result[0].Data1, 0x1c);
            Assert.AreEqual(result[0].Data2, 0x64);
            Assert.AreEqual(result[5].Status, 0x80);
            Assert.AreEqual(result[5].Data1, 0x1d);
            Assert.AreEqual(result[5].Data2, 0x64);
        }

        [TestMethod()]
        public void TestProperty()
        {

        }

        private void TestEvent()
        {
            testEventCount++;
        }
    }
}