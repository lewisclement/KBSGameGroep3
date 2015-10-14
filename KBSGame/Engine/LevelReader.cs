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
		//private List<Terrain> TerrainTiles;
		private int defaultbackground;
		private XmlDocument reader;

		public LevelReader(string File)
		{
			//this.TerrainTiles = new List<Terrain>();
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

			XmlNodeList entityList = reader.GetElementsByTagName("e");
			foreach (XmlNode entity in entityList)
			{
				Point location = new Point();
				location.X = Int32.Parse(entity["x"].InnerText);
				location.Y = Int32.Parse(entity["y"].InnerText);
				int SpriteID = Int32.Parse(entity["s"].InnerText);
				bool solid = bool.Parse(entity["so"].InnerText);

				switch (Int32.Parse(entity["ty"].InnerText))
				{
				case (int)ENTITIES.player:
					objects.Add(new Player(location, 50));
					break;
				case (int)ENTITIES.finish:
					objects.Add(new Finish(location, SpriteID));
					break;
				case (int)ENTITIES.plant:
					objects.Add(new Plant(location, SpriteID, 50, solid));
					break;
				default:
					objects.Add (new Entity (location, SpriteID, solid));
					break;
				}
			}


		}

		public List<Entity> getObjects()
		{
			getEntitysxml();
			return this.objects;
		}

		public List<int> getTerrainTiles()
		{
			reader.Load(StaticVariables.execFolder + "/tiles.xml");
			XmlNodeList TerraintileList = reader.GetElementsByTagName("t");

			List<int> terrainTiles = new List<int> ();

			foreach (XmlNode TerrainList in TerraintileList)
			{
				terrainTiles.Add(Int32.Parse(TerrainList["i"].InnerText));
			}

			return terrainTiles;
		}

		public int getdefaultbackground()
		{
			XmlNodeList elemlist = reader.GetElementsByTagName("DefaultTerrain");
			return Int32.Parse(elemlist[0]["TerrainID"].InnerText);
		}
	}
}