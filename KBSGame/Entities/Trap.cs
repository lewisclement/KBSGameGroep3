﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Trap : Enemy
    {
        public Trap(PointF location, Byte height = 50, bool solid = false, Byte depth = 8, float boundingBox = 1.0f)
			: base(ENTITIES.trap, location, (int)SPRITES.trapOpened, solid, height, depth, boundingBox)
		{

        }

        public void Dead()
        {
            trapClosed();
            Console.WriteLine("You're dead");
        }

        private void trapClosed()
        {
            this.spriteID = (int)SPRITES.trapClosed;
        }
    }
}
