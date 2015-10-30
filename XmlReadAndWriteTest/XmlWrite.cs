using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KBSGame;
using System.Collections.Generic;

namespace XmlReadAndWriteTest
{
    [TestClass]
    public class XmlWrite
    {
        private World w;
        private Player p;
        private TerrainTile[] beforelist, afterlist;
        private Entity[] before, after;
        private String file = "test";
        private World world;

        public XmlWrite()
        {
            // initialise levelFolder
            StaticVariables.execFolder = AppDomain.CurrentDomain.BaseDirectory;
            StaticVariables.levelFolder = StaticVariables.execFolder;
            // create world
            this.w = new World(10, 10);
            // fill world
            this.w.FillWorld(TERRAIN.grass_normal, new Size(10, 10));
            // initialize player
            this.w.InitPlayer(new PointF(5, 5));
            this.p = w.getPlayer();
            LevelWriter.saveWorld(this.w, this.file);
            beforelist = new TerrainTile[w.getTerrain().Count];
            before = new Entity[w.getEntities().Count];
            // save beforlist
            w.getTerrain().CopyTo(beforelist);
            w.getEntities().CopyTo(before);
            world = new World(10, 10);
            world.loadLevel(this.file);
            afterlist = new TerrainTile[world.getTerrain().Count];
            after = new Entity[world.getEntities().Count];
            // save afterlist
            world.getTerrain().CopyTo(afterlist);
            world.getEntities().CopyTo(after);
        }
        [TestMethod]
        public void terrainCheck()
        {
            // checks if lists are equal
            for (int i =0; i < this.world.getTerrain().Count; i++)
            {
                Assert.AreEqual(this.beforelist[i].getID(),this.afterlist[i].getID());
                Assert.AreEqual(this.beforelist[i].getSpriteID(), this.afterlist[i].getSpriteID());
                Assert.AreEqual(this.beforelist[i].GetType(), this.afterlist[i].GetType());
            }
        }
        [TestMethod]
        public void entityCheck()
        {
            for (int i = 0; i < before.Length; i++)
            {
                Entity x = before[i];
                while ((i - 1 >= 0) && (x.getLocation().Y < before[i - 1].getLocation().Y))
                {
                    before[i] = before[i - 1];
                    i--;
                }
                before[i] = x;
            }
            // checks if lists are equal
            for (int i = 0; i < this.w.getEntities().Count; i++)
            {
                Assert.AreEqual(this.before[i].getID(), this.after[i].getID());
                Assert.AreEqual(this.before[i].getSpriteID(), this.after[i].getSpriteID());
                Assert.AreEqual(this.before[i].getLocation(), this.after[i].getLocation());
                Assert.AreEqual(this.before[i].getType(), this.after[i].getType());
            }
        }
    }
}
