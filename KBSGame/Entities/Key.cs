﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Key : Entity
    {
        private static int KeyCount = 0;
        public int Keyid;

        public Key(PointF location, bool solid = false, byte height = 50, byte drawOrder = 8, float boundingBox = 1)
            : base(ENTITIES.key, location, (int) SPRITES.key, solid, height, drawOrder, boundingBox)
        {
            Keyid = KeyCount++;
        }

        public int getKeyid()
        {
            return Keyid;
        }
    }
}
