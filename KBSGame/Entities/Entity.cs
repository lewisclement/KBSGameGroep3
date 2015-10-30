using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KBSGame
{
	public class Entity        
	{
		private static int entityID = 0;
		protected int ID = 0;

		protected ENTITIES type = ENTITIES.def;
		protected int spriteID;
		protected PointF location;
		protected bool solid;
		protected Byte drawOrder;
		protected float boundingBox;
		protected Byte height;

		public Entity(ENTITIES type, PointF location, int spriteID, bool solid = false, Byte height = 50, Byte drawOrder = 8, float boundingBox = 1.0f)
		{
			this.type = type;
			this.location = location;
			this.height = height;
			this.drawOrder = drawOrder;
			this.boundingBox = boundingBox;
			this.spriteID = spriteID;
			this.solid = solid;
			this.ID = entityID++;
		}
		public Entity(int ID, PointF location, int spriteID, bool solid = false, Byte height = 50, Byte drawOrder = 8, float boundingBox = 1.0f)
		{
			this.location = location;
			this.height = height;
			this.drawOrder = drawOrder;
			this.boundingBox = boundingBox;
			this.spriteID = spriteID;
			this.solid = solid;
			this.ID = ID;
		}

		public int getID()
		{
			return ID;
		}

		public ENTITIES getType()
		{
			return type;
		}

		public bool getSolid()
		{
			return solid;
		}

		public PointF getLocation()
		{
			return location;
		}

		public int getSpriteID()
		{
			return spriteID;
		}

		public float getBoundingBox()
		{
			return boundingBox;
		}

		public void setSolid(float boundingBox)
		{
			this.boundingBox = boundingBox;
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

		public Byte getHeight()
		{
			return this.height;
		}

		public void setHeight(Byte height)
		{
			this.height = height;
		}

		public void setLocation(PointF point)
		{
			this.location = point;
		}

        /// <summary>
        /// Set DrawOrder which allows more depth for the player when standing on top of an entity.
        /// </summary>
        /// <param name="drawOrder"></param>
		protected void setDrawOrder(Byte drawOrder)
		{
			if (drawOrder >= StaticVariables.drawOrderSize)
				this.drawOrder = StaticVariables.drawOrderSize-1;
			else
				this.drawOrder = drawOrder;
		}

		public virtual void move(World sender, PointF relativeLocation)
		{
			//Entity checks
			//Have world move the entity
			location.X += relativeLocation.X;
			location.Y += relativeLocation.Y;
		}

        //Reaction on Collision.
		public virtual void onCollision(Entity e = null)
        {
		}
	}
}
