using System;
using System.Drawing;

namespace KBSGame
{
	public class TerrainTile
	{
		private int ID;
		private int spriteID;
        public bool IsWalkable { get; set; }

		public TerrainTile (int ID, int spriteID, bool IsWalkable = true)
		{
			this.ID = ID;
			this.spriteID = spriteID;
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
	}
}

