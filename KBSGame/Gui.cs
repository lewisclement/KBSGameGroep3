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
        private Bitmap buffer;
		private int xRes, yRes;

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

			g.FillRectangle (new SolidBrush(Color.White), 20, 20, xRes - 40, yRes - 40); //To do: Figure out why -40 is nessecary to have the same margin
			g.DrawString("Pause", new Font("Arial", 16), new SolidBrush(Color.Black), 30, 30);

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
