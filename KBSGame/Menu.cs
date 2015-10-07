using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame
{
    public class Menu : Gui
    {
        private int xRes, yRes;
        private Bitmap buffer;
        public Menu(int ID, int ScreenresX, int ScreenresY) : base(ID, ScreenresX, ScreenresY)
        { 
            new Gui(ID, ScreenresX, ScreenresY);
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 0, 0)), 0, 0, xRes, yRes); //To do: Figure out why -40 is nessecary to have the same margin
            g.DrawString("Pause", new Font("Arial", 20), new SolidBrush(Color.White), xRes / 2, 20);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), xRes/3, 60, xRes / 2, 25 );
            g.DrawString("Resume", new Font("Arial", 16), new SolidBrush(Color.White), xRes / 2, 60);
            return this.buffer;
        }
    }

}
