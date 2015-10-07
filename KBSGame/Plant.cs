using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (int ID, Point location, int spriteID, Byte depth = 8) : base(ID, location, false, depth)
		{
			this.spriteID = spriteID;
		}
	}
}

