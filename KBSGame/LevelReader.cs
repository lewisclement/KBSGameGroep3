using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KBSGame
{
    class LevelReader
    {
        private List<Entity> objects;
        private List<TerrainTile> TerrainTiles;
        public LevelReader(String File)
        {
            this.objects = new List<Entity>();

            XmlDocument reader = new XmlDocument();
            try {
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
                            // Byte height = node["height"].InnerText;
                            Player p = new Player(location, 50);
                            this.objects.Add(p);
                            p = null;
                            Console.WriteLine("player");
                            Console.WriteLine(node["ID"].InnerText);
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    case "Terrain":
                        try
                        {

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
                            Console.WriteLine(ex.Message);
                        }
                        break;
                }    
            }

        }
        public List<Entity> getObjects()
        {
            return this.objects;
        }
    }
}
