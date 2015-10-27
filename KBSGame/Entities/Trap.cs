﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Trap : Entity
    {
        public Trap(PointF location, int spriteID, Byte height = 50, bool solid = false, Byte depth = 8, float boundingBox = 1.0f)
			: base(ENTITIES.trap, location, spriteID, solid, height, depth, boundingBox)
		{

        }

        public override void onCollision()
        {
            this.spriteID = (int)SPRITES.trapClosed;

            Console.WriteLine("you're dead");
        }
    }
}