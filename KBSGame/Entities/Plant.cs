using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (PointF location, int spriteID, Byte height = 50, bool solid = true, Byte depth = 8, int drawPrecision = 10)
			: base(location, spriteID, false, height, depth, drawPrecision)
		{
			this.spriteID = spriteID;
            this.solid = solid;
			this.type = ENTITIES.plant;
		}
	}
}

