using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Settings : Gui
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
            g.DrawString("The settings wil be rendered down here!", new Font("Arial", 40, FontStyle.Underline), new SolidBrush(Color.WhiteSmoke), 0, 60);
            //  g.DrawRectangle(new Pen(Color.Black,5), xRes / 2, yRes / 6, 30, 30);

            // g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), xRes / 3, 60, xRes / 2, 25);
            //  g.DrawString("Resume", new Font("Arial", 16), new SolidBrush(Color.White), 0, 60);
            
            return this.buffer;
        }
    }
}
