using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlayerMovementTest
{
    [TestClass]
    public class PlayerMoveToEntity
    {
        private World w;
        private Player p;

        public PlayerMoveToEntity()
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
        }

        [TestMethod]
        public void MoveToWater()
        {
            // Set Player location
            p.setLocation(new Point(0, 1));
            // Try to move one grid down to water tile
            p.move(w, new Point(0, 1));
            // Check if player didn't move
            Assert.AreEqual(new Point(0, 1), p.getLocation());
        }

        [TestMethod]
        public void MoveToNonSolidOnNonWalkable()
        {
            // Set Player location
            p.setLocation(new Point(0, 1));
            // Add lilypad (walkable Entity) on location 0,2 (NonWalkable tile)
            Entity lilypad = new Plant(new Point(0, 2), (int)SPRITES.waterlily, 50, false, 0, 0);
            w.addEntity(lilypad);
            // Try to move one grid down to water tile with lilypad
            p.move(w, new Point(0, 1));
            // Check if player moved
            Assert.AreEqual(new Point(0, 2), p.getLocation());
        }

        [TestMethod]
        public void MoveToSolid()
        {
            // Set Player location
            p.setLocation(new Point(0, 1));
            // Add solid entity (tree) on location 1,1
            Entity tree = new Plant(new Point(1, 1), (int)SPRITES.sapling1, 50);
            w.addEntity(tree);
            // Try to move one grid to the right to tile with solid entity
            p.move(w, new Point(1, 1));
            // Check if player didn't move
            Assert.AreEqual(new Point(0, 1), p.getLocation());
        }

        [TestMethod]
        public void MoveToNonSolid()
        {
            // Set Player location
            p.setLocation(new Point(0, 1));
            // Add non-solid entity (tall grass) on location 0,0
            Entity tree = new Plant(new Point(0, 0), (int)SPRITES.tallgrass, 50);
            w.addEntity(tree);
            // Try to move one grid to the right to tile with solid entity
            p.move(w, new Point(1, 1));
            // Check if player moved
            Assert.AreEqual(new Point(0, 0), p.getLocation());
        }
    }
}
