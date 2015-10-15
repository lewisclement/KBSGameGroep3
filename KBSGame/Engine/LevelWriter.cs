using System;
using System.Xml;
using System.Collections.Generic;



namespace KBSGame
{
	public class LevelWriter
	{
		String sTile = "t";
		String sID = "i";
		String sEntity = "e";
		String sType = "ty";
		String sSpriteID = "s";
		String sSolid = "so";
		String sDrawOrder = "d";

		public LevelWriter ()
		{
			
		}

		public void saveWorld(World world) 
		{
			List<TerrainTile> tiles = world.getTerrain ();
			List<Entity> entities = world.getEntities ();

			using (XmlWriter writer = XmlWriter.Create(StaticVariables.execFolder + "/tiles.xml"))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("world");

				writer.WriteStartElement("tiles");

				foreach (TerrainTile tile in tiles)
				{
					writer.WriteStartElement(sTile);

					writer.WriteElementString(sID, tile.getID().ToString());

					writer.WriteEndElement();
				}

				writer.WriteEndElement();

				writer.WriteStartElement("entities");

				foreach (Entity e in entities) {
					writer.WriteStartElement(sEntity);

					writer.WriteElementString(sID, e.getID().ToString());
					writer.WriteElementString(sType, ((int)e.getType()).ToString());
					writer.WriteElementString(sSpriteID, e.getSpriteID().ToString());
					writer.WriteElementString("x", e.getLocation().X.ToString());
					writer.WriteElementString("y", e.getLocation().Y.ToString());
					writer.WriteElementString(sSolid, e.getSolid().ToString());
					writer.WriteElementString(sDrawOrder, e.getDrawOrder().ToString());

					writer.WriteEndElement();
				}

				writer.WriteEndElement();

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}
	}
}

