using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (Point location, int spriteID, Byte depth = 8, int drawPrecision = 10)
            : base(location, false, depth, drawPrecision)
		{
			this.spriteID = spriteID;
		}
	}
}

