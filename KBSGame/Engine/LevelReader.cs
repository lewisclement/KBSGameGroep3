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
        private Entity[] Entities;
        private List<Terrain> TerrainTiles;
        private int defaultbackground;
        private XmlDocument reader;
        public LevelReader(string File)
        {
            this.TerrainTiles = new List<Terrain>();
            this.objects = new List<Entity>();

            this.reader = new XmlDocument();
            try
            {
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
        }
        private void getEntitysxml()
        {

            XmlNodeList entityList = reader.GetElementsByTagName("Item");
            foreach (XmlNode entity in entityList)
            {
                Point location = new Point();
                location.X = Int32.Parse(entity["X"].InnerText);
                location.Y = Int32.Parse(entity["Y"].InnerText);
                int Height = Int32.Parse(entity["height"].InnerText);
                int SpriteID = Int32.Parse(entity["SpriteID"].InnerText);
                Entities = new Entity[(int)TERRAIN.count];
                Entities[(int)ENTITIES.player] = new Player(location,50);
                Entities[(int)ENTITIES.finish] = new Finish(location, SpriteID);
                Entities[(int)ENTITIES.plant] = new Plant(location, SpriteID);
                switch (entity["Type"].InnerText)
                {
                    case "Player":
                        this.objects.Add(Entities[(int)ENTITIES.player]);
                        break;
                    case "Finish":
                        this.objects.Add(Entities[(int)ENTITIES.finish]);
                        break;
                    case "Plant":
                        this.objects.Add(Entities[(int)ENTITIES.plant]);
                        break;
                }
            }
        }
        public List<Entity> getObjects()
        {
            getEntitysxml();
            return this.objects;
        }
        public List<Terrain> getTerrainTiles()
        {
            XmlNodeList TerraintileList = reader.GetElementsByTagName("TerrainTile");
            foreach (XmlNode TerrainList in TerraintileList)
            {
                Point location = new Point();
                location.X = Int32.Parse(TerrainList["X"].InnerText);
                location.Y = Int32.Parse(TerrainList["Y"].InnerText);
                int TerrainID =Int32.Parse(TerrainList["TerrainID"].InnerText);
                this.TerrainTiles.Add(new Terrain(location, TerrainID));

            }
                return this.TerrainTiles;
        }
        public int getdefaultbackground()
        {
            XmlNodeList elemlist = reader.GetElementsByTagName("DefaultTerrain");
            return Int32.Parse(elemlist[0]["TerrainID"].InnerText);
        }
    }
}