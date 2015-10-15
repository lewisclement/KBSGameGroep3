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
		private int defaultbackground;
		private XmlDocument reader;

		public LevelReader(string File)
		{
			this.reader = new XmlDocument();
			try
			{
				this.reader.Load(File);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		private List<Entity> getObjects()
		{
			List<Entity> objects = new List<Entity> ();
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

			return objects;
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
	}
}