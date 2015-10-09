using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlayerMovementTest
{
    [TestClass]
    public class PlayerMoveToWater
    {
        private World w;
        private Player p;

        public PlayerMoveToWater()
        {
            w = new World(3, 3);
            SetupWorld();
        }

        private void SetupWorld()
        {
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
            p.setLocation(new Point(0, 1));
        }

        [TestMethod]
        public void MoveToWater()
        {
            // Move is relative to current position
            // Move one grid down to grass tile
            p.move(w, new Point(0, 0));
            Assert.AreEqual(new Point(0, 1), p.getLocation());

            // Try to move one grid down to water tile
            p.move(w, new Point(0, 1));
            // Check if player didn't move
            Assert.AreEqual(new Point(0, 1), p.getLocation());

            // Add lilypad on location 0,2
            Entity lilypad = new Plant(new Point(0, 2), (int)SPRITES.waterlily, 50, false, 0, 0);
            w.addEntity(lilypad);
            // Try to move one grid down to water tile with lilypad
            p.move(w, new Point(0, 1));
            Assert.AreEqual(new Point(0, 2), p.getLocation());
        }
    }
}
