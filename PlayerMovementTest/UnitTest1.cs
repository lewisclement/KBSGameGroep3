using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlayerMovementTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PlayerMove()
        {
            MainWindow mw = new MainWindow();

            World w = mw.getWorld();
            Player p = w.getPlayer();
            p.setLocation(new Point(1, 1));
            
            // Move is relative to current position
            // Move left
            p.move(w, new Point(-1, 0));
            Assert.AreEqual(new Point(0, 1), p.getLocation());

            // Move right
            p.move(w, new Point(1, 0));
            Assert.AreEqual(new Point(0, 1), p.getLocation());

            // Move down
            p.move(w, new Point(0, 1));
            Assert.AreEqual(new Point(0, 1), p.getLocation());

            // Move up
            p.move(w, new Point(0, -1));
            Assert.AreEqual(new Point(0, 1), p.getLocation());
        }

        [TestMethod]
        public void PlayerMoveToWater()
        {
            World w = new World(5, 5);
            
            Player p = w.getPlayer();
            p.setLocation(new Point(1, 1));

            // Move is relative to current position
            p.move(w, new Point(-1, 0));
            Assert.AreEqual(new Point(0, 1), p.getLocation());
        }
    }
}
