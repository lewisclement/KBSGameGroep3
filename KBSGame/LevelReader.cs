using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KBSGame
{
    class Terrain
    {
        public Point p;
        public int i; 
        public Terrain(Point p, int i)
        {
            this.i = i;
            this.p = p;
        }
        public int geti()
        {
            return this.i;
        }
        public Point getP()
        {
            return this.p;
        }
    }
    class LevelReader
    {
        private List<Entity> objects;
        private List<Terrain> TerrainTiles;
        private int defaultbackground;
        private XmlDocument reader;
        public LevelReader(string File)
        {          
            this.TerrainTiles = new List<Terrain>();
            this.objects = new List<Entity>();

            this.reader = new XmlDocument();
            try {
            this.reader.Load(File);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //foreach (XmlNode node in reader.DocumentElement)
            //{
            //    switch (node["type"].InnerText)
            //    {
            //            break;
            //        case "Terrain":
            //            try
            //            {
            //                int X = Int32.Parse(node["X"].InnerText);
            //                int Y = Int32.Parse(node["Y"].InnerText);
            //                int S = Int32.Parse(node["SpriteID"].InnerText);
            //                Terrain t = new Terrain(new Point(X, Y), S);
            //                this.TerrainTiles.Add(t);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //            }
            //            break;
            //        case "DefaultTerrain":
            //            try
            //            {
            //                this.defaultbackground = Int32.Parse(node["SpriteID"].InnerText);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //            }
            //            break;
            //        default:
            //            try
            //            {
            //                Point location = new Point();
            //                location.X = Int32.Parse(node["X"].InnerText);
            //                location.Y = Int32.Parse(node["Y"].InnerText);
            //                bool solid = Convert.ToBoolean(node["solid"].InnerText);
            //                int sprite = Int32.Parse(node["spriteID"].InnerText);
            //                int ID = Int32.Parse(node["ID"].InnerText);
            //                Entity e = new Entity(ID, location, sprite, solid);
            //                this.objects.Add(e);
            //            }
            //            catch (Exception ex)
            //            {
            //              //  Console.WriteLine(ex.Message);
            //            }
            //            break;
            //    }
            //  }
            addEntity("Player");
            addEntity("Finish");
        }
        private void addEntity(String sort)
        {
            try
            {
                XmlNodeList elemlist = reader.GetElementsByTagName(sort);
                foreach (XmlNode Entity in elemlist)
                {
                    switch (Entity.ParentNode.LocalName)
                    {
                        case "Player":

                            Point location = new Point();
                            location.X = Int32.Parse(Entity["X"].InnerText);
                            location.Y = Int32.Parse(Entity["Y"].InnerText);
                            int Height = Int32.Parse(Entity["height"].InnerText);
                            int SpriteID = Int32.Parse(Entity["SpriteID"].InnerText);
                            Player p = new Player(location, 50);
                            this.objects.Add(p);
                            break;
                        case "Finish":
                            //Finish F = new Finish(location, SpriteID);
                            //this.objects.Add(F);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<Entity> getObjects()
        {
            return this.objects;
        }
        public List<Terrain> getTerrainTiles()
        {
            return this.TerrainTiles;
        }
        public int getdefaultbackground()
        {
            return this.defaultbackground;
        }
    }
}
