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
        public void TestMethod1()
        {
            MainWindow mw = new MainWindow();

            World w = mw.getWorld();
            Player p = w.getPlayer();
            p.setLocation(new Point(1, 1));
            
            p.move(w, new Point(0, 1));
            Assert.AreEqual(new Point(0, 1), p.getLocation());
        }
    }
}
