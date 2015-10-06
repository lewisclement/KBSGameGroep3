﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Player : Entity
    {
        public Player(int ID, Point location) : base(ID, location)
        {
            this.ID = ID;
            this.location = location;
            this.setSprite((int) SPRITES.player);
        }
    }
}
