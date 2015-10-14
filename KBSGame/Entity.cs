using System;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KBSGame
{
    public class Entity        
	{
        protected int ID = 0;
        protected int spriteID;
        protected PointF location;
        protected bool solid;
        protected Byte drawOrder;
        protected int drawPrecision;
        protected Byte height;

		public Entity(PointF location, int spriteID, bool solid = false, Byte height = 50, Byte drawOrder = 8, int drawPrecision = 10)
		{
			this.location = location;
			this.height = height;
			this.drawOrder = drawOrder;
		    this.drawPrecision = drawPrecision;
			this.spriteID = spriteID;
			this.solid = solid;
	        ID++;
		}
        public Entity(int ID, PointF location, int spriteID, bool solid = false, Byte height = 50, Byte drawOrder = 8, int drawPrecision = 10)
        {
            this.location = location;
            this.height = height;
            this.drawOrder = drawOrder;
            this.drawPrecision = drawPrecision;
            this.spriteID = spriteID;
            this.solid = solid;
            this.ID = ID;
        }

        public int getID()
		{
			return ID;
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

		public virtual void collisionCheck()
		{

            

		}

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}

