﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Item
    {
        public bool CanPickup { get; set; }
        private Entity Entity { get; set; }

        public Item(Entity e)
        {
            this.Entity = e;
        }
    }
}
