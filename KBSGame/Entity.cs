using System;
using System.Drawing;

namespace KBSGame
{
	public class Entity
	{
		protected int ID;
		protected int spriteID;
		protected Point location;
		protected bool solid;

		public Entity (int ID, Point location)
		{
			this.ID = ID;
			this.location = location;
		}

		public int getID()
		{
			return ID;
		}

		public bool getSolid()
		{
			return solid;
		}

		public Point getLocation()
		{
			return location;
		}

		public int getSpriteID()
		{
			return spriteID;
		}

	    public void setSolid(bool solid)
		{
			this.solid = solid;
		}

		public void setSprite(int ID)
		{
			this.spriteID = ID;
		}

		public void setSprite(Sprite sprite)
		{
			this.spriteID = sprite.getID ();
		}

		public virtual void move(World sender, Point relativeLocation)
		{
			//Entity checks

			//Have world move the entity

		    int moveLocationX = location.X + relativeLocation.X;
            int moveLocationY = location.Y + relativeLocation.Y;
		    if (sender.getTerraintile(new Point(moveLocationX, moveLocationY)).IsWalkable)
		    {
		        location.X += relativeLocation.X;
		        location.Y += relativeLocation.Y;
		    }
		}

		public virtual void collisionCheck()
		{

		}
	}
}

