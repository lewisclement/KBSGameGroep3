using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (PointF location, int spriteID, Byte height = 50, bool solid = true, Byte depth = 10, float boundingBox = 0.51f)
			: base(location, spriteID, false, height, depth, boundingBox)
		{
			this.spriteID = spriteID;
            this.solid = solid;
			this.type = ENTITIES.plant;
		}
	}
}

