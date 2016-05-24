using System;
using SimpleScore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleScoreUnitTest
{
    [TestClass]
    public class PlayerTest
    {
        Player player;
        PrivateObject target;

        [TestInitialize()]
        [DeploymentItem("SimpleScore.exe")]
        public void Initialize()
        {
            player = new Player();
            target = new PrivateObject(player);
        }

        [TestMethod()]
        public void TestPlayer()
        {
            
        }
    }
}