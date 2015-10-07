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

		public Entity (int ID, Point location, bool solid = false)
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

	        location.X += relativeLocation.X;
	        location.Y += relativeLocation.Y;
		}

		public virtual void collisionCheck()
		{

		}
	}
}

