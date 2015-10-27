using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlayerInventoryTest
{
    [TestClass]
    public class PlayerInventory
    {

        private World w;
        private Player p;

        public PlayerInventory()
        {
            this.w = new World(3, 3, null);
            this.p = w.getPlayer();
            /*
             * Grid simulation setup:
             * G = Grass, W = Water
             * |G|G|G|
             * |G|G|G|
             * |W|W|W|
             */
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    w.setTerraintile(new Point(i, j), (int)SPRITES.grass);
                }
                w.setTerraintile(new Point(i, 2), (int)SPRITES.water);
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
