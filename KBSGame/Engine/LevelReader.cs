using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using KBSGame.Entities;
using System.Windows.Forms;

namespace KBSGame
{
	public class LevelReader
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

					int SpriteID = 0;
					try {
						SpriteID = Int32.Parse(entity[xmlVar.SpriteID].InnerText);
					} catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }

					bool solid = false;
					try {
						solid = bool.Parse(entity[xmlVar.Solid].InnerText);
					} catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

					Byte drawOrder = 10;
					try {
						drawOrder = Byte.Parse (entity [xmlVar.DrawOrder].InnerText);
					} catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

					float BoudingBox = 0.51f;
					try {
						BoudingBox = float.Parse (entity [xmlVar.BoudingBox].InnerText);
					} catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

					switch (Int32.Parse(entity[xmlVar.Type].InnerText))
					{
					case (int)ENTITIES.player:
						objects.Add(new Player(location, 50));
						break;
                    case (int)ENTITIES.enemy:
                        objects.Add(new Enemy(location));
                        break;
	    			case (int)ENTITIES.finish:
						objects.Add(new Finish(location));
						break;
                    case (int)ENTITIES.trap:
                        objects.Add(new Trap(location, SpriteID));
                        break;
                    case (int)ENTITIES.key:
                        objects.Add(new Key(location));
                        break;
                    case (int)ENTITIES.door:
                        objects.Add(new Door(location));
                        break;
                    case (int)ENTITIES.plant:
						objects.Add(new Plant(location, SpriteID, 50, solid, drawOrder, BoudingBox));
						break;
                    default:
						objects.Add (new Entity (ENTITIES.def, location, SpriteID, solid, 50, drawOrder, BoudingBox));
						break;
					}
				} catch (Exception e) {

                    MessageBox.Show("Could not load entity: " + entity.InnerXml + "\n\r" + e.Message);
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