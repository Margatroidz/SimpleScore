using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class FileTest
    {
        File file;
        MidiParser parser;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            file = new File();
            parser = new MidiParser();
            target = new PrivateObject(file);
        }

        [TestMethod()]
        public void TestParser()
        {
            File file = new File();
            Score score = new Score();

            parser.Parse(file.Load(@"../../TestData/01.mid"), score);
            Assert.AreEqual(score.TrackCount, 1);
            score = new Score();

            parser.Parse(file.Load(@"../../TestData/02.mid"), score);
            Assert.AreEqual(score.TrackCount, 2);
            score = new Score();

            parser.Parse(file.Load(@"../../TestData/03.mid"), score);
            Assert.AreEqual(score.TrackCount, 2);
        }
    }
}