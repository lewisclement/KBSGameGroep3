using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame.Entities
{
    class Door : Entity
    {
        private static int DoorCount = 0;
        public int Doorid;

        public Door(PointF location, bool solid = true, Byte height = 50, Byte depth = 8, float boundingBox = 0.6f)
            : base(ENTITIES.door, location, (int)SPRITES.doorClosed, solid, height, depth, boundingBox)
        {
            Doorid = DoorCount++;
        }

        public void UnlockDoor()
        {
            Console.WriteLine("Unlocked door with ID: " + Doorid);
            spriteID = (int)SPRITES.doorOpened;
            setSolid(false);
        }

        public void LockDoor()
        {
            Console.WriteLine("Locked door with ID: " + Doorid);
            spriteID = (int)SPRITES.doorClosed;
            setSolid(true);
        }

        public int getDoorid()
        {
            return Doorid;
        }
    }
}
