using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KBSGame
{
	public class Gui
    {
        private int ID;
		private Boolean active;
		protected Bitmap buffer;
		protected int xRes, yRes;

		public Gui(int ID, int ScreenresX, int ScreenresY)
        {
			this.ID = ID;

			xRes = ScreenresX;
			yRes = ScreenresY;

			buffer = new Bitmap (xRes, yRes);

			this.active = false;
        }

		public virtual Bitmap getRender()
        {
			var g = Graphics.FromImage (buffer);
			g.Clear (Color.FromArgb (0));

			Pen boldPen = new Pen (Color.Black, 5);
			g.FillRectangle (new SolidBrush(Color.FromArgb(140, Color.Black)), 25, 25, xRes - 35, yRes - 35);
			g.FillRectangle (new SolidBrush(Color.FromArgb(200, Color.White)), 20, 20, xRes - 40, yRes - 40); //To do: Figure out why -40 is nessecary to achieve the same margin
			g.DrawLine (boldPen, 20, 20, xRes - 20, yRes - 20);
			g.DrawLine (boldPen, xRes - 20, 20, 20, yRes - 20);
			g.DrawString("Placeholder Gui\n\rPlease extend class", new Font("Arial", 16), new SolidBrush(Color.Red), 30, 30);

            return this.buffer;
        }

		public virtual void setInput(Point mousePos)
        {

        }

        // processes keyboard input
		public virtual void setInput(System.Windows.Forms.Keys key)
        {

        }

		public void resize(int ScreenresX, int ScreenresY)
		{
			xRes = ScreenresX;
			yRes = ScreenresY;

			buffer = new Bitmap (xRes, yRes);
		}

		public void setActive(bool active)
		{
			this.active = active;
		}

		public bool isActive()
		{
			return active;
		}

		public void switchActive()
		{
			active = !active;
		}
    }
}
