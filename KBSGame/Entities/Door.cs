using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame.Entities
{
    public class Door : Entity
    {
        public static int DoorCount = 0;
        public int Doorid;

        public Door(PointF location, bool solid = true, Byte height = 50, Byte depth = 8, float boundingBox = 0.6f)
            : base(ENTITIES.door, location, (int)SPRITES.door_closed, solid, height, depth, boundingBox)
        {
            Doorid = DoorCount++;
			if (Doorid == 1)
				spriteID = (int)SPRITES.door_closed2;
			if (Doorid == 2)
				spriteID = (int)SPRITES.door_closed3;
			if (Doorid == 3)
				spriteID = (int)SPRITES.door_closed4;
        }

        public void UnlockDoor()
        {
            spriteID = (int)SPRITES.door_opened;
			if (Doorid == 1)
				spriteID = (int)SPRITES.door_opened2;
			if (Doorid == 2)
				spriteID = (int)SPRITES.door_opened3;
			if (Doorid == 3)
				spriteID = (int)SPRITES.door_opened4;
            setSolid(false);
        }

        public void LockDoor()
        {
            spriteID = (int)SPRITES.door_closed;
			if (Doorid == 1)
				spriteID = (int)SPRITES.door_closed2;
			if (Doorid == 2)
				spriteID = (int)SPRITES.door_closed3;
			if (Doorid == 3)
				spriteID = (int)SPRITES.door_closed4;
            setSolid(true);
        }

        public int getDoorid()
        {
            return Doorid;
        }
    }
}
