using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Item : Entity
    {
        public bool CanPickup { get; set; }

        public Item(Point location, int spriteID, bool solid = false, bool CanPickup = true, byte height = 50, byte drawOrder = 8, int drawPrecision = 10)
            : base(location, spriteID, solid, height, drawOrder, drawPrecision)
        {
            this.CanPickup = CanPickup;
        }
    }
}
