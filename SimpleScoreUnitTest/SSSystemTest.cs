using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class SSSystemTest
    {
        SSSystem system;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            system = new SSSystem();
            target = new PrivateObject(system);
        }

        [TestMethod()]
        public void TestSystem()
        {
            system = new SSSystem();
            Assert.IsNotNull(target.GetFieldOrProperty("player"));
            Assert.IsNotNull(target.GetFieldOrProperty("player"));
            Assert.AreEqual(target.GetFieldOrProperty("loadStyle"), SSSystem.LoadStyle.Single);
            Assert.IsNotNull(target.GetFieldOrProperty("file"));
        }

        [TestMethod()]
        public void TestCreateScore()
        {
            Assert.IsNull(target.GetFieldOrProperty("score"));
            target.Invoke("CreateScore");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, string.Empty);
            ((Score)target.GetFieldOrProperty("score")).Name = "test";
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "test");
            target.Invoke("CreateScore");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, string.Empty);
        }

        [TestMethod()]
        public void TestLoad()
        {
            system.Load(@"../../TestData/01.mid");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "01");
            system.Load(@"../../TestData/03.mid");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "03");
        }

        [TestMethod()]
        public void TestLoadSequential()
        {
            system.Load(@"../../TestData/01.mid");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "01");
            system.LoadSequential(1);
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "02");
            system.LoadSequential(2);
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "01");
            system.LoadSequential(-1);
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Name, "03");
        }

        [TestMethod()]
        public void TestChangeLoadStyle()
        {
            system.ChangeLoadStyle(0);
            Assert.AreEqual(target.GetFieldOrProperty("loadStyle"), SSSystem.LoadStyle.Single);
            system.ChangeLoadStyle(1);
            Assert.AreEqual(target.GetFieldOrProperty("loadStyle"), SSSystem.LoadStyle.Loop);
            system.ChangeLoadStyle(2);
            Assert.AreEqual(target.GetFieldOrProperty("loadStyle"), SSSystem.LoadStyle.Random);
            system.ChangeLoadStyle(3);
            Assert.AreEqual(target.GetFieldOrProperty("loadStyle"), SSSystem.LoadStyle.Sequential);
        }

        [TestMethod()]
        public void TestGetVoiceByTrack()
        {
            target.Invoke("CreateScore");
            Score score = (Score)target.GetFieldOrProperty("score");
            score.CreateTrack();
            score.CreateTrack();
            score.CreateTrack();
            score.CreateNote(0, new Voice(0, 0x90, 0x2c, 0x64));
            score.CreateNote(0, new Voice(0, 0x80, 0x2c, 0x64));
            score.CreateNote(0, new Voice(100, 0x90, 0x1a, 0x64));
            score.CreateNote(0, new Voice(1000, 0x80, 0x1a, 0x64));
            score.CreateNote(1, new Voice(0, 0x90, 0x20, 0x64));
            score.CreateNote(1, new Voice(10, 0x80, 0x20, 0x64));
            Voice[] voiceList = system.GetVoiceByTrack(0);
            Assert.AreEqual(voiceList[0].Time, 0);
            Assert.AreEqual(voiceList[0].Status, 0x90);
            Assert.AreEqual(voiceList[0].Data1, 0x2c);
            Assert.AreEqual(voiceList[0].Data2, 0x64);
            Assert.AreEqual(voiceList[3].Time, 1000);
            Assert.AreEqual(voiceList[3].Status, 0x80);
            Assert.AreEqual(voiceList[3].Data1, 0x1a);
            Assert.AreEqual(voiceList[3].Data2, 0x64);
            Assert.AreEqual(voiceList.Length, 4);
            voiceList = system.GetVoiceByTrack(1);
            Assert.AreEqual(voiceList[0].Time, 0);
            Assert.AreEqual(voiceList[0].Status, 0x90);
            Assert.AreEqual(voiceList[0].Data1, 0x20);
            Assert.AreEqual(voiceList[0].Data2, 0x64);
            Assert.AreEqual(voiceList[1].Time, 10);
            Assert.AreEqual(voiceList[1].Status, 0x80);
            Assert.AreEqual(voiceList[1].Data1, 0x20);
            Assert.AreEqual(voiceList[1].Data2, 0x64);
            Assert.AreEqual(voiceList.Length, 2);
        }

        [TestMethod()]
        public void TestStop()
        {
            Player player = (Player)target.GetFieldOrProperty("player");
            system.Stop();
            //沒有辦法測試是否成功wait或set
            //score還沒有丟進去player先按stop，單純測試防呆
            PrivateObject playerPO = new PrivateObject(player);
            system.Load(@"../../TestData/02.mid");
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Clock, 0);
            system.ChangeClock(0.01f);
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Clock, 3868);
            system.Stop();
            Assert.AreEqual(((Score)target.GetFieldOrProperty("score")).Clock, 0);
            playerPO.Invoke("CloseMidiDevice");
        }

        [TestMethod()]
        public void TestPlayOrPause()
        {
            Player player = (Player)target.GetFieldOrProperty("player");
            Assert.AreEqual(player.IsPlay, false);
            //沒有辦法測試是否成功wait或set
            //score還沒有丟進去player先按play或pause，原本就可以按，順便測試防呆
            system.PlayOrPause();
            Assert.AreEqual(player.IsPlay, true);
            system.PlayOrPause();
            Assert.AreEqual(player.IsPlay, false);
            PrivateObject playerPO = new PrivateObject(player);
            playerPO.Invoke("CloseMidiDevice");
        }
    }
}