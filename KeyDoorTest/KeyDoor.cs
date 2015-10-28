using System;
using System.Drawing;
using KBSGame;
using KBSGame.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KeyDoorTest
{
    [TestClass]
    public class KeyDoor
    {
        private World w;
        private Player p;
        private const int Size = 3;
        private int bananaId = (int)SPRITES.banana;
        private Door d;
        private Key k;

        /*
         * Grid simulation setup:
         * G = Grass, W = Water
         * |G|G|G|
         * |G|G|G|
         * |G|G|G|
         */

        public KeyDoor()
        {
            // Initialize world
            this.w = new World(Size, Size);
            // Fill world with tiles of grass
            w.FillWorld(TERRAIN.grass, new Size(Size, Size));

            // Initialize player
            w.InitPlayer(new PointF(0, 0));
            this.p = w.getPlayer();
            p.CurrentDirection = (int)Player.Direction.Down;

            // Initialize door and key
            this.d = new Door(new PointF(1, 0));
            w.addEntity(d);
            this.k = new Key(new PointF(0, 0));
            w.addEntity(k);
        }


        [TestMethod]
        public void OpenDoorOnPickup()
        {
            // Pickup key
            p.PickupItems(w);
            // Check if door is not solid anymore
            Assert.AreEqual(d.getSolid(), false);
        }

        [TestMethod]
        public void CloseDoorOnDrop()
        {
            // Pickup key
            p.PickupItems(w);
            // Drop key
            p.DropItem(w);
            // Check if door is solid again
            Assert.AreEqual(d.getSolid(), true);
        }

        [TestMethod]
        public void DropKeyWhileInDoor()
        {
            // Pickup key
            p.PickupItems(w);
            // Walk towards door
            p.move(w, new Point(1, 0));
            // Try to drop key
            p.DropItem(w);
            // Check if door is still open
            Assert.AreEqual(d.getSolid(), false);
            // Check if key is still in inventory
            Assert.AreEqual(p.Inventory[0].Entity, k);
        }

        [TestMethod]
        public void WalkThroughClosedDoor()
        {
            // Walk towards door
            p.move(w, new Point(1, 0));
            // Check if player didn't move
            Assert.AreEqual(p.getLocation(), new PointF(0, 0));
        }

        [TestMethod]
        public void WalkThroughOpenDoor()
        {
            // Pickup key
            p.PickupItems(w);
            // Walk towards door
            p.move(w, new Point(1, 0));
            // Check if player is in the same position as the door
            Assert.AreEqual(p.getLocation(), d.getLocation());
        }
    }
}
