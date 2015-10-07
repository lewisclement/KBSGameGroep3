using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame
{
    public class Menu : Form
    {
        public Menu(int x, int y)
        {
            Width = x;
            Height = y;
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics dc = e.Graphics;
            Pen p = new Pen(Color.Black, 1);
            dc.DrawLine(p, 10, 10, 100, 100);
            Pen thickBluePen = new Pen(Color.Blue, 10);
            dc.DrawEllipse(thickBluePen, 100, 100, 200, 200);
            Pen thickRedPen = new Pen(Color.Red, 10);
            dc.DrawRectangle(thickRedPen, 100, 100, 200, 200);
        }

    }

}
