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
        public Finish (PointF location, int spriteID, Byte height = 50, bool solid = false, Byte depth = 8, int drawPrecision = 10)
			: base(location, spriteID, false, height, depth, drawPrecision)
		{
            this.spriteID = spriteID;
            this.solid = solid;

        }




        public void LevelDone()
        {
            GameOverMenu finish = new GameOverMenu(50, 200, 200, "finish");
            finish.addMenuItem("HE DIKKE JONKO");



            //System.Windows.Forms.Application.Exit();
            Console.WriteLine("HALLO LEKKER DING! LEWIS IS GREAT");
        }

    }
}
