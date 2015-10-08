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
		protected Byte drawOrder;

		public Entity (int ID, Point location, bool solid = false, Byte drawOrder = 8)
		{
			this.ID = ID;
			this.location = location;
			this.drawOrder = drawOrder;
            
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

		public Byte getDrawOrder()
		{
			return this.drawOrder;
		}

		protected void setDrawOrder(Byte drawOrder)
		{
			if (drawOrder >= StaticVariables.drawOrderSize)
				this.drawOrder = StaticVariables.drawOrderSize-1;
			else
				this.drawOrder = drawOrder;
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

