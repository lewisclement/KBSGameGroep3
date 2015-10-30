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
        }

        public void UnlockDoor()
        {
            spriteID = (int)SPRITES.door_opened;
            setSolid(false);
        }

        public void LockDoor()
        {
            spriteID = (int)SPRITES.door_closed;
            setSolid(true);
        }

        public int getDoorid()
        {
            return Doorid;
        }
    }
}
