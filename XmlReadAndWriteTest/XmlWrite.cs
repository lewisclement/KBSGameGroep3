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
        private List<TerrainTile> beforelist, afterlist;
        private String file = "test";
        public XmlWrite()
        {
            this.w = new World(10, 10);
            this.w.FillWorld(TERRAIN.grass, new Size(50, 50));
            p = new Player(new PointF(5, 5), 50);
            this.w.addEntity(p);
            LevelWriter level = new LevelWriter();
            beforelist = w.getTerrain();
            LevelWriter.saveWorld(w, file);
        }
        [TestMethod]
        public void WriteReadXml()
        {
            World world = new World(10, 10);
            LevelReader level = new LevelReader(file);
            this.afterlist= world.getTerrain();
            Assert.AreEqual(this.beforelist, this.afterlist);
        }
    }
}
