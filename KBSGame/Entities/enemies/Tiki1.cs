using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
	public class Tiki1 : Enemy
    {
		static int radius = 5;

        public Tiki1(PointF location, Byte height = 50)
            : base(location, height, true, 8, 0.6f)
        {

        }
    }
}
