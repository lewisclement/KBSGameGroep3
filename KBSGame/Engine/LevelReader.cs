﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KBSGame
{
	class LevelReader
	{
		private XmlDocument reader;

		public LevelReader(string file)
		{
			this.reader = new XmlDocument();
			try
			{
				reader.Load(file);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		~LevelReader()
		{
			reader = null;
		}

		public List<Entity> getObjects()
		{
			List<Entity> objects = new List<Entity> ();
			XmlNodeList entityList = reader.GetElementsByTagName(xmlVar.Entity);
			foreach (XmlNode entity in entityList)
			{
				try {
					PointF location = new PointF();
					location.X = float.Parse(entity["x"].InnerText);
					location.Y = float.Parse(entity["y"].InnerText);
					int SpriteID = Int32.Parse(entity[xmlVar.SpriteID].InnerText);
					bool solid = bool.Parse(entity[xmlVar.Solid].InnerText);
					Byte drawOrder = Byte.Parse (entity [xmlVar.DrawOrder].InnerText);

					switch (Int32.Parse(entity[xmlVar.Type].InnerText))
					{
					case (int)ENTITIES.player:
						objects.Add(new Player(location, 50));
						break;
					case (int)ENTITIES.finish:
						objects.Add(new Finish(location, SpriteID));
						break;
                    case (int)ENTITIES.trap:
                        objects.Add(new Trap(location, SpriteID));
                        break;
					case (int)ENTITIES.plant:
						objects.Add(new Plant(location, SpriteID, 50, solid, drawOrder));
						break;
                    default:
						objects.Add (new Entity (location, SpriteID, solid, 50, drawOrder));
						break;
					}
				} catch (Exception e) {
					Console.WriteLine ("Could not load entity: " + entity.InnerXml + "\n\r" + e.Message);
				}
			}

			return objects;
		}

		public List<int> getTerrainTiles()
		{
			XmlNodeList TerraintileList = reader.GetElementsByTagName(xmlVar.Tile);

			List<int> terrainTiles = new List<int> ();

			foreach (XmlNode TerrainList in TerraintileList)
			{
				terrainTiles.Add(Int32.Parse(TerrainList[xmlVar.ID].InnerText));
			}

			return terrainTiles;
		}

		public Size getSize()
		{
			XmlNode size = reader.GetElementsByTagName("size")[0];
			return new Size (Int32.Parse(size[xmlVar.Width].InnerText), Int32.Parse(size[xmlVar.Height].InnerText));
		}
	}
}