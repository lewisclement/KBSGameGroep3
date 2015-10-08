using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Settings : Gui
    {
        public Settings(int ID, int ScreenresX, int ScreenresY) : base(ID, ScreenresX, ScreenresY)
        {
            new Gui(ID, ScreenresX, ScreenresY);
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
        }
        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, xRes, yRes); //To do: Figure out why -40 is nessecary to have the same margin
            g.DrawString("Settings", new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);

            // g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), xRes / 3, 60, xRes / 2, 25);
            //  g.DrawString("Resume", new Font("Arial", 16), new SolidBrush(Color.White), 0, 60);
            Pen boldPen = new Pen(Color.White, 5);
            g.DrawLine(boldPen, xRes - 70, 30, xRes - 30, 70);
            g.DrawLine(boldPen, xRes - 70, 70, xRes - 30, 30);
            return this.buffer;
        }
    }
}
