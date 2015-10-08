using System;
using System.Drawing;

namespace KBSGame
{
	public class Entity
	{
	    protected static int ID = 0;
		protected int spriteID;
		protected Point location;
		protected bool solid;
		protected Byte drawOrder;
	    protected int drawPrecision;

	    public Entity(Point location, bool solid = false, Byte drawOrder = 8, int drawPrecision = 10)
		{
			this.location = location;
			this.drawOrder = drawOrder;
		    this.drawPrecision = drawPrecision;
	        ID++;
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

	    public int getDrawPrecision()
	    {
            return drawPrecision;
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

