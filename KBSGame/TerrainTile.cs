using System;
using System.Drawing;

namespace KBSGame
{
	public class TerrainTile
	{
		private int ID;
		private int spriteID;

		public TerrainTile (int ID)
		{
			this.ID = ID;
		}

		public int getID()
		{
			return ID;
		}

		public int getSpriteID()
		{
			return spriteID;
		}

		public void setSpriteID(int ID)
		{
			this.spriteID = ID;
		}
	}
}

