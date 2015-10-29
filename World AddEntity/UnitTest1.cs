using System;
using System.Drawing;
using KBSGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace World_AddEntity
{
    [TestClass]
    public class addEntity
    {
        private World w;
        private const int Size = 3;
        private Entity entityTest;

        public addEntity()
        {
            // Initialize world
            this.w = new World(Size, Size);
        }
        [TestMethod]
        public void testAddEntity()
        {
            entityTest = new Entity(ENTITIES.def, new PointF(1,1), 22, false, 50, 8, 1);

            w.getEntities().Add(entityTest);

            Assert.AreEqual(w.getEntities()[0], entityTest);
            
        }
            
            
            
            
            

        
        
    }
}
