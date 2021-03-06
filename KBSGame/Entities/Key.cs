﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    /// <summary>
    /// Extention of the Entity class.
    /// </summary>
    public class Key : Entity
    {
        public static int KeyCount = 0;
        public int Keyid;

        public Key(PointF location, bool solid = false, byte height = 50, byte drawOrder = 8, float boundingBox = 1)
            : base(ENTITIES.key, location, (int) SPRITES.key, solid, height, drawOrder, boundingBox)
        {
            Keyid = KeyCount++;
			if (Keyid == 1)
				spriteID = (int)SPRITES.key2;
			if (Keyid == 2)
				spriteID = (int)SPRITES.key3;
			if (Keyid == 3)
				spriteID = (int)SPRITES.key4;
        }

        //Simple getter to get the Key ID which connects to a certain doorID.
        public int getKeyid()
        {
            return Keyid;
        }
    }
}
