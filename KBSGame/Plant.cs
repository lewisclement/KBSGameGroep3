using System;
using System.Drawing;

namespace KBSGame
{
	public class Plant : Entity
	{
		public Plant (int ID, Point location, int spriteID) : base(ID, location)
		{
			this.spriteID = spriteID;
		}
	}
}

