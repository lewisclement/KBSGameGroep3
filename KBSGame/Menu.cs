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
            g.Clear(Color.FromArgb(0));
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, xRes, yRes); //To do: Figure out why -40 is nessecary to have the same margin
            g.DrawString("Pause", new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), xRes / 3, 60, xRes / 2, 25);
            g.DrawString("Resume", new Font("Arial", 16), new SolidBrush(Color.White), xRes / 2, 60);
            Pen boldPen = new Pen(Color.Black, 5);
            g.DrawLine(boldPen, xRes - 70, 30, xRes - 30, 70);
            g.DrawLine(boldPen, xRes - 70, 70, xRes - 30, 30);
            return this.buffer;
        }


        //public override Bitmap getRender()
        //{
        //    var g = Graphics.FromImage(buffer);
        //    g.Clear(Color.FromArgb(0));

        //    g.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), 25, 25, xRes - 35, yRes - 35);

        //    g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.White)), 20, 20, xRes - 40, yRes - 40); //To do: Figure out why -40 is nessecary to achieve the same margin
        //    g.DrawString("Pause", new Font("Arial", 16), new SolidBrush(Color.Black), 30, 30);

        //    Pen boldPen = new Pen(Color.Black, 5);
        //    g.DrawLine(boldPen, xRes - 70, 30, xRes - 30, 70);
        //    g.DrawLine(boldPen, xRes - 70, 70, xRes - 30, 30);

        //    return this.buffer;
        //}
    }

}
