using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public LevelReader(String File)
        {
            this.TerrainTiles = new List<Terrain>();
            this.objects = new List<Entity>();

            XmlDocument reader = new XmlDocument();
            try
            {
                reader.Load(File);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            foreach (XmlNode node in reader.DocumentElement.ChildNodes)
            {
                switch (node["type"].InnerText)
                {
                    case "Player":
                        try
                        {
                            Point location = new Point();
                            location.X = Int32.Parse(node["X"].InnerText);
                            location.Y = Int32.Parse(node["Y"].InnerText);
                            Player p = new Player(location, 50);
                            this.objects.Add(p);
                        }
                        catch (Exception ex)
                        {
                             Console.WriteLine(ex.Message);
                        }
                        break;
                    case "Terrain":
                        try
                        {
                            int X = Int32.Parse(node["X"].InnerText);
                            int Y = Int32.Parse(node["Y"].InnerText);
                            int S = Int32.Parse(node["SpriteID"].InnerText);
                            Terrain t = new Terrain(new Point(X, Y), S);
                            this.TerrainTiles.Add(t);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "DefaultTerrain":
                        try
                        {
                            this.defaultbackground = Int32.Parse(node["SpriteID"].InnerText);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    default:
                        try
                        {
                            Point location = new Point();
                            location.X = Int32.Parse(node["X"].InnerText);
                            location.Y = Int32.Parse(node["Y"].InnerText);
                            bool solid = Convert.ToBoolean(node["solid"].InnerText);
                            int sprite = Int32.Parse(node["spriteID"].InnerText);
                            int ID = Int32.Parse(node["ID"].InnerText);
                            Entity e = new Entity(ID, location, sprite, solid);
                            this.objects.Add(e);
                        }
                        catch (Exception ex)
                        {
                            //  Console.WriteLine(ex.Message);
                        }
                        break;
                }
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