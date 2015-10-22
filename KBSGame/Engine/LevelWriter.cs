using System;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;


namespace KBSGame
{
	public class LevelWriter
	{
		public LevelWriter ()
		{
			
		}

		public static void saveWorld(World world, String fileName) 
		{
			List<TerrainTile> tiles = world.getTerrain ();
			List<Entity> entities = world.getEntities ();
			Size size = world.getSize ();

			using (XmlWriter writer = XmlWriter.Create(StaticVariables.levelFolder + "/" + fileName + ".xml"))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("world");

				writer.WriteStartElement("size");
				writer.WriteElementString(xmlVar.Width, size.Width.ToString());
				writer.WriteElementString(xmlVar.Height, size.Width.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("tiles");

				foreach (TerrainTile tile in tiles)
				{
					writer.WriteStartElement(xmlVar.Tile);

					writer.WriteElementString(xmlVar.ID, tile.getID().ToString());

					writer.WriteEndElement();
				}

				writer.WriteEndElement();

				writer.WriteStartElement("entities");

				foreach (Entity e in entities) {
					writer.WriteStartElement(xmlVar.Entity);

					writer.WriteElementString(xmlVar.ID, e.getID().ToString());
					writer.WriteElementString(xmlVar.Type, ((int)e.getType()).ToString());
					writer.WriteElementString(xmlVar.SpriteID, e.getSpriteID().ToString());
					writer.WriteElementString("x", e.getLocation().X.ToString());
					writer.WriteElementString("y", e.getLocation().Y.ToString());
					writer.WriteElementString(xmlVar.Solid, e.getSolid().ToString());
					writer.WriteElementString(xmlVar.DrawOrder, e.getDrawOrder().ToString());

					writer.WriteEndElement();
				}

				writer.WriteEndElement();

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
	}
}

