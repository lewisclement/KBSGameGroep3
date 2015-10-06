using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Player : Entity
    {
        public Player(int ID, Point location) : base(ID, location)
        {
            this.ID = ID;
            this.location = location;
            this.setSprite((int) SPRITES.player);
        }

		public override void move(World sender, Point relativeLocation)
		{
			int moveLocationX = location.X + relativeLocation.X;
			int moveLocationY = location.Y + relativeLocation.Y;

			if (moveLocationX < 0 || moveLocationY < 0)
				return;

			if (sender.getTerraintile(new Point(moveLocationX, moveLocationY)).IsWalkable)
			{
				location.X = moveLocationX;
				location.Y = moveLocationY;
			}
		}
    }
}
