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
        public XmlWrite()
        {
            w = new World(10,10);
            w.FillWorld((int)SPRITES.sand);
            LevelWriter level = new LevelWriter();
            this.beforelist = w.getTerrain();
            level.saveWorld(w);
        }
        [TestMethod]
        public void WriteReadXml()
        {
            World world = new World(10, 10);            
            LevelReader level = new LevelReader("Test.xml");
            this.afterlist= world.getTerrain();
            Assert.AreEqual(this.beforelist, this.afterlist);
        }
    }
}
