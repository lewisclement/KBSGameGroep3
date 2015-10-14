using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace KBSGame
{
    public class Finish : Entity
    {
        public Finish (Point location, int spriteID, Byte height = 50, bool solid = false, Byte depth = 8, int drawPrecision = 10)
			: base(location, spriteID, false, height, depth, drawPrecision)
		{
            this.spriteID = spriteID;
            this.solid = solid;

        }

        public void LevelDone()
        {
			System.Windows.Forms.Application.Exit();
            Console.WriteLine("HALLO LEKKER DING! LEWIS IS GREAT");
        }
    }
}
