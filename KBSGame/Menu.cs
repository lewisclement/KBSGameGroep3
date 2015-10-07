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
using System;
using System.Collections.Generic;
<<<<<<< Upstream, based on origin/master
{
    public class Menu : Form
    {
        public Menu(int x, int y)
=======
{ 
	public class Menu : Gui
    {    
		public Menu(int ID, int ScreenresX, int ScreenresY) : base(ID, ScreenresX, ScreenresY)
>>>>>>> f1e1394 Prepared game menu class
        {
<<<<<<< Upstream, based on origin/master
            Width = x;
            Height = y;
            
=======
            

>>>>>>> f1e1394 Prepared game menu class
        }
<<<<<<< Upstream, based on origin/master
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

=======

		public override Bitmap getRender ()
		{
			var g = Graphics.FromImage (buffer);
			g.Clear (Color.FromArgb (0));

			g.FillRectangle (new SolidBrush(Color.FromArgb(140, Color.Black)), 25, 25, xRes - 35, yRes - 35);

			g.FillRectangle (new SolidBrush(Color.FromArgb(200, Color.White)), 20, 20, xRes - 40, yRes - 40); //To do: Figure out why -40 is nessecary to achieve the same margin
			g.DrawString("Pause", new Font("Arial", 16), new SolidBrush(Color.Black), 30, 30);

			Pen boldPen = new Pen (Color.Black, 5);
			g.DrawLine (boldPen, xRes - 70, 30, xRes - 30, 70);
			g.DrawLine (boldPen, xRes - 70, 70, xRes - 30, 30);

			return this.buffer;
		}
>>>>>>> f1e1394 Prepared game menu class
    }

}
