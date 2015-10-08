using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Key : Entity
    {
        public Key(Point location, bool solid = false, byte height = 50, byte drawOrder = 8, int drawPrecision = 10)
			: base(location, (int)SPRITES.key, solid, height, drawOrder, drawPrecision)
        {

        }
    }
}
