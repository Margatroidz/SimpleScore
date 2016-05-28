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
            Assert.AreEqual(target.GetFieldOrProperty("beatPerMilliSecond"), 0.5f);
            Assert.AreEqual(((List<TimeData>)target.GetFieldOrProperty("beatList"))[0].Clock, 0);
            Assert.AreEqual(((List<TimeData>)target.GetFieldOrProperty("beatList"))[0].Data, 0.5f);
            Assert.AreEqual(target.GetFieldOrProperty("currentBeat"), 0);
            Assert.AreEqual(score.Tick, 0);
            Assert.AreEqual(score.Clock, 0);
            Assert.AreEqual(score.Length, 0);
            Assert.AreEqual(score.IsEnd, false);
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
            score.CreateMessage(0, new Voice(0, 0x90, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 0);
            Assert.AreEqual(trackList[0].GetMessages().Length, 1);
            score.CreateMessage(0, new Voice(100, 0x80, 0x2c, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(trackList[0].GetMessages().Length, 2);
            score.CreateMessage(0, new Voice(0, 0x90, 0x1a, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 100);
            Assert.AreEqual(trackList[0].GetMessages().Length, 3);
            score.CreateMessage(0, new Voice(1000, 0x80, 0x1a, 0x64));
            Assert.AreEqual(target.GetFieldOrProperty("length"), 1000);
            Assert.AreEqual(trackList[0].GetMessages().Length, 4);
            Assert.AreEqual(score.ProgressPercentage, 0d);
            score.CreateMessage(1, new Voice(0, 0x80, 0x1a, 0x64));
        }

        [TestMethod()]
        public void TestGetTrack()
        {
            List<Track> trackList = ((List<Track>)target.GetFieldOrProperty("trackList"));
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList[0].AddMessage(new Voice(0, 0x90, 0x2c, 0x64));
            trackList[0].AddMessage(new Voice(0, 0x80, 0x2c, 0x64));
            trackList[0].AddMessage(new Voice(100, 0x90, 0x1a, 0x64));
            trackList[0].AddMessage(new Voice(1000, 0x80, 0x1a, 0x64));
            trackList[1].AddMessage(new Voice(0, 0x90, 0x20, 0x64));
            trackList[1].AddMessage(new Voice(10, 0x80, 0x20, 0x64));
            trackList[2].AddMessage(new Voice(0, 0x90, 0x1c, 0x64));
            trackList[2].AddMessage(new Voice(0, 0x90, 0x1d, 0x64));
            trackList[2].AddMessage(new Voice(100, 0x80, 0x1c, 0x64));
            trackList[2].AddMessage(new Voice(100, 0x90, 0x25, 0x64));
            trackList[2].AddMessage(new Voice(500, 0x80, 0x25, 0x64));
            trackList[2].AddMessage(new Voice(500, 0x80, 0x1d, 0x64));

            Voice[] result = score.GetTrack(0);
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
        public void TestPlay()
        {
            List<Track> trackList = ((List<Track>)target.GetFieldOrProperty("trackList"));
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList.Add(new Track());
            trackList[0].AddMessage(new Voice(0, 0x90, 0x2c, 0x64));
            trackList[0].AddMessage(new Voice(0, 0x80, 0x2c, 0x64));
            trackList[0].AddMessage(new Voice(100, 0x90, 0x1a, 0x64));
            trackList[0].AddMessage(new Voice(1000, 0x80, 0x1a, 0x64));
            trackList[1].AddMessage(new Voice(0, 0x90, 0x20, 0x64));
            trackList[1].AddMessage(new Voice(10, 0x80, 0x20, 0x64));
            trackList[2].AddMessage(new Voice(0, 0x90, 0x1c, 0x64));
            trackList[2].AddMessage(new Voice(0, 0x90, 0x1d, 0x64));
            trackList[2].AddMessage(new Voice(100, 0x80, 0x1c, 0x64));
            trackList[2].AddMessage(new Voice(100, 0x90, 0x25, 0x64));
            trackList[2].AddMessage(new Voice(500, 0x80, 0x25, 0x64));
            trackList[2].AddMessage(new Voice(500, 0x80, 0x1d, 0x64));

            Voice[] result = score.Play();
            Assert.AreEqual(result.Length, 5);
            target.SetFieldOrProperty("clock", 50);
            result = score.Play();
            Assert.AreEqual(result.Length, 1);
            target.SetFieldOrProperty("clock", 100);
            result = score.Play();
            Assert.AreEqual(result.Length, 3);
            target.SetFieldOrProperty("clock", 499);
            result = score.Play();
            Assert.AreEqual(result.Length, 0);
            target.SetFieldOrProperty("clock", 1000);
            result = score.Play();
            Assert.AreEqual(result.Length, 3);
        }

        [TestMethod()]
        public void TestAddBeatTime()
        {
            List<TimeData> beat = (List<TimeData>)target.GetFieldOrProperty("beatList");

            Assert.AreEqual(beat.Count, 1);
            score.AddBeatTime(10, 0.2f);
            score.AddBeatTime(1000, 0.5f);
            Assert.AreEqual(beat.Count, 3);
            Assert.AreEqual(beat[1].Clock, 10);
            Assert.AreEqual(beat[1].Data, 0.2f);
            Assert.AreEqual(beat[2].Clock, 1000);
            Assert.AreEqual(beat[2].Data, 0.5f);
        }

        [TestMethod()]
        public void TestIncreaseClock()
        {
            float semiquaver = (float)target.GetFieldOrProperty("semiquaver");
            List<TimeData> beat = (List<TimeData>)target.GetFieldOrProperty("beatList");
            score.playProgressChanged += TestEvent;
            beat.Add(new TimeData(5, 0.4f));
            beat.Add(new TimeData(10, 0.6f));
            score.Tick = 160;
            Assert.AreEqual(score.Clock, 0);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.5f);
            Assert.AreEqual(testEventCount, 0);
            score.IncreaseClock();
            Assert.AreEqual(score.Clock, 10);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.4f);
            Assert.AreEqual(testEventCount, 0);
            score.IncreaseClock();
            Assert.AreEqual(score.Clock, 20);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.6f);
            Assert.AreEqual(testEventCount, 0);
            score.IncreaseClock();
            Assert.AreEqual(score.Clock, 30);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.6f);
            Assert.AreEqual(testEventCount, 0);
            score.IncreaseClock();
            Assert.AreEqual(score.Clock, 40);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.6f);
            Assert.AreEqual(testEventCount, 1);
            score.playProgressChanged -= TestEvent;
        }

        [TestMethod()]
        public void TestChangeClock()
        {
            List<Track> trackList = ((List<Track>)target.GetFieldOrProperty("trackList"));
            trackList.Add(new Track());
            score.CreateMessage(0, new Voice(0, 0x90, 0x27, 0x64));
            score.CreateMessage(0, new Voice(100, 0x90, 0x29, 0x64));
            score.CreateMessage(0, new Voice(200, 0x80, 0x27, 0x64));
            score.CreateMessage(0, new Voice(500, 0x80, 0x29, 0x64));
            score.AddBeatTime(50, 0.7f);
            score.AddBeatTime(200, 0.6f);

            score.ChangeClock(0.6f);
            Assert.AreEqual(score.Clock, 300);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.6f);
            score.ChangeClock(0.1f);
            Assert.AreEqual(score.Clock, 50);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.5f);
            score.ChangeClock(0.09f);
            Assert.AreEqual(score.Clock, 45);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.5f);
            score.ChangeClock(0.4f);
            Assert.AreEqual(score.Clock, 200);
            Assert.AreEqual(score.BeatPerMilliSecond, 0.7f);
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