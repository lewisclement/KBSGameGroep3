using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (int ID, Point location, int spriteID, bool solid =  true Byte depth = 8, int drawPrecision = 10)
            : base(ID, location, false, depth, drawPrecision)
		{
			this.spriteID = spriteID;
            this.solid = solid;
            
		}
	}
}

