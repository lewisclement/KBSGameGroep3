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
        private String file = "test";
        private World world;

        public XmlWrite()
        {
            // initialise levelFolder
            StaticVariables.execFolder = AppDomain.CurrentDomain.BaseDirectory;
            StaticVariables.levelFolder = StaticVariables.execFolder + "/worlds";
            // create world
            this.w = new World(10, 10);
            // fill world
            this.w.FillWorld(TERRAIN.grass, new Size(10, 10));
            // initialize player
            this.w.InitPlayer(new PointF(5, 5));
            this.p = w.getPlayer();
            LevelWriter.saveWorld(this.w, this.file);

            beforelist = new TerrainTile[w.getTerrain().Count];
            // save beforlist
            w.getTerrain().CopyTo(beforelist);
            world = new World(10, 10);
            world.loadLevel(this.file);
            afterlist = new TerrainTile[world.getTerrain().Count];
            // save afterlist
            world.getTerrain().CopyTo(afterlist);
        }
        [TestMethod]
        public void terrainCheck()
        {
            // checks if lists are equal
            for (int i =0; i < this.world.getTerrain().Count; i++)
            {
                Assert.AreEqual(this.beforelist[i].getID(),this.afterlist[0].getID());
            }
        }
        [TestMethod]
        public void entityCheck()
        {
            
        }
    }
}
