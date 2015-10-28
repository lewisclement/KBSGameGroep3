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
        private String file = "test";

        public XmlWrite()
        {
            // initialise levelFolder
            StaticVariables.execFolder = AppDomain.CurrentDomain.BaseDirectory;
            StaticVariables.levelFolder = StaticVariables.execFolder + "/worlds";
            // create world
            this.w = new World(10, 10);
            // fill world
            this.w.FillWorld(TERRAIN.grass, new Size(50, 50));
            // initialize player
            this.w.InitPlayer(new PointF(5, 5));
            this.p = w.getPlayer();
        }
        [TestMethod]
        public void WriteReadXml()
        {
            LevelWriter.saveWorld(this.w, this.file);
            TerrainTile[] beforelist, afterlist;
            beforelist = new TerrainTile[w.getTerrain().Count];
            // save beforlist
            w.getTerrain().CopyTo(beforelist);
            World world = new World(10, 10);
            world.loadLevel(this.file);
            afterlist = new TerrainTile[world.getTerrain().Count];
            // save afterlist
            world.getTerrain().CopyTo(afterlist);
            // checks if lists are equal
            CollectionAssert.AreEqual(beforelist, afterlist);
        }
    }
}
