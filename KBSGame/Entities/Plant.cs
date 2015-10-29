using System;
using System.Drawing;

namespace KBSGame
{
    /// <summary>
    /// Extention of the entity class.
    /// </summary>
	public class Plant : Entity
	{
		public Plant (PointF location, int spriteID, Byte height = 50, bool solid = true, Byte depth = 10, float boundingBox = 0.51f)
			: base(ENTITIES.plant, location, spriteID, false, height, depth, boundingBox)
		{
			this.spriteID = spriteID;
            this.solid = solid;
		}
	}
}

