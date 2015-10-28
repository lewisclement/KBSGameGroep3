using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlayerMovementTest
{
    [TestClass]
    public class PlayerMovement
    {
        private World w;
        private Player p;
        private const int Size = 3;

        /*
         * Grid simulation setup:
         * G = Grass, W = Water
         * |G|G|G|
         * |G|G|G|
         * |W|W|W|
         */

        public PlayerMovement()
        {
            // Initialize world
            this.w = new World(Size, Size);
            // Fill world with tiles of grass
            w.FillWorld(TERRAIN.grass, new Size(Size, Size));
            // Fill third row with water
            for (int i = 0; i <= 2; i++)
                w.setTerraintile(new Point(i, 2), (int)SPRITES.water);

            // Initialize player
            w.InitPlayer(new PointF(1, 1));
            this.p = w.getPlayer();
        }

        [TestMethod]
        public void Move()
        {
            // Set Player location
            p.setLocation(new PointF(0, 0));

            // Move is relative to current position
            // Move right
            p.move(w, new PointF(1, 0));
            Assert.AreEqual(new PointF(1, 0), p.getLocation());
            // Move left
            p.move(w, new PointF(-1, 0));
            Assert.AreEqual(new PointF(0, 0), p.getLocation());
            // Move down
            p.move(w, new PointF(0, 1));
            Assert.AreEqual(new PointF(0, 1), p.getLocation());
            // Move up
            p.move(w, new PointF(0, -1));
            Assert.AreEqual(new PointF(0, 0), p.getLocation());
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
            p.setLocation(new PointF(0, 1));
            // Add solid entity (tree) on location 1,1
            Entity tree = new Plant(new PointF(1, 1), (int)SPRITES.sapling1, 50);
            w.addEntity(tree);
            // Try to move one grid to the right to tile with solid entity
            p.move(w, new PointF(1, 1));
            // Check if player didn't move
            Assert.AreEqual(new PointF(0, 1), p.getLocation());
        }

        [TestMethod]
        public void MoveToNonSolid()
        {
            // Set Player location
            p.setLocation(new PointF(0, 1));
            // Add non-solid entity (tall grass) on location 0,0
            Entity tree = new Plant(new PointF(0, 0), (int)SPRITES.tallgrass, 50, false);
            w.addEntity(tree);
            // Try to move one grid up to tile with non-solid entity
            p.move(w, new PointF(0, -1));
            // Check if player moved
            Assert.AreEqual(new PointF(0, 0), p.getLocation());
        }
    }
}
