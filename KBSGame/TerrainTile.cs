using System;
using System.Drawing;

namespace KBSGame
{
	public class TerrainTile
	{
		private int ID;
		private int spriteID;
        public bool IsWalkable { get; private set; }

		public TerrainTile (int ID, bool IsWalkable = true)
		{
			this.ID = ID;
		    this.IsWalkable = IsWalkable;
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

