using System;
using System.Collections.Generic;
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
        private const int Size = 3;
        private int bananaId = (int) SPRITES.banana;

        /*
         * Grid simulation setup:
         * G = Grass, W = Water
         * |G|G|G|
         * |G|G|G|
         * |W|W|W|
         */

        public PlayerInventory()
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
        public void PickUpBanana()
        {
            // Add banana in world on same location as player
            w.addEntity(new Entity(ENTITIES.fruit, new PointF(1, 1), bananaId));
            // Call pickup method on player
            p.PickupItems(w);
            // Check if first item in inventory is equal to a banana
            Assert.AreEqual(p.Inventory[0].Entity.getSpriteID(), bananaId);
        }

        [TestMethod]
        public void DropBananaOnGrassTile()
        {
            // Add banana to players inventory
            p.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), bananaId)));
            // Drop item above player on 1, 0
            p.DropItem(w);
            // Check if inventory is empty
            Assert.AreEqual(p.Inventory.Count, 0);
            // Check if entity exists on tile 1, 0
            List<Entity> entities = w.getEntitiesOnTerrainTile(new Point(1, 0));
            Assert.AreEqual(entities[0].getSpriteID(), bananaId);
        }

        [TestMethod]
        public void DropBananaOnWaterTile()
        {
            // Add banana to players inventory
            p.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), bananaId)));
            // Set direction to down
            p.CurrentDirection = (int) Player.Direction.Down;
            // Drop item below player on 1, 2
            p.DropItem(w);
            // Check if banana still exists in inventory
            Assert.AreEqual(p.Inventory[0].Entity.getSpriteID(), bananaId);
        }

        [TestMethod]
        public void DropBananaOnWaterTileWithLilypad()
        {
            // Add lilypad on location 1, 2
            Entity lilypad = new Plant(new Point(1, 2), (int)SPRITES.waterlily, 50, false, 0, 0);
            w.addEntity(lilypad);
            // Add banana to players inventory
            p.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), bananaId)));
            // Set direction to down
            p.CurrentDirection = (int)Player.Direction.Down;
            // Drop item below player on 1, 2
            p.DropItem(w);
            // Check if banana still exists in inventory
            Assert.AreEqual(p.Inventory.Count, 0);
            //Assert.AreEqual(p.Inventory[0].Entity.getSpriteID(), bananaId);

            // Check if banana is dropped on location 1, 2
            List<Entity> entities = w.getEntitiesOnTerrainTile(new Point(1, 2));
            Assert.AreEqual(entities[0].getSpriteID(), bananaId);
        }

        [TestMethod]
        public void DropBananaOnSolidEntity()
        {
            // Add tree on location 2, 1
            Entity tree = new Plant(new Point(1, 2), (int)SPRITES.sapling_oak, 50);
            w.addEntity(tree);
            // Add banana to players inventory
            p.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), bananaId)));
            // Set direction to right
            p.CurrentDirection = (int)Player.Direction.Right;
            // Drop item on the right of player on 2, 1
            p.DropItem(w);
            // Check if banana still exists in inventory
            Assert.AreEqual(p.Inventory.Count, 0);
        }
    }
}
