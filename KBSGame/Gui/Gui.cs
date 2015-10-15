﻿using System;
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
		/// <summary>
		/// Initializes a new instance of the <see cref="KBSGame.Gui"/> class.
		/// </summary>
		/// <param name="ID">I.</param>
		/// <param name="ScreenresX">Screenres x.</param>
		/// <param name="ScreenresY">Screenres y.</param>
		public Gui(int ID, int ScreenresX, int ScreenresY)
        {
			this.ID = ID;

			xRes = ScreenresX;
			yRes = ScreenresY;

			buffer = new Bitmap (xRes, yRes);

			this.active = false;
        }
		/// <summary>
		/// Gets the render.
		/// </summary>
		/// <returns>The render.</returns>
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

		/// <summary>
		/// Sets the input.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		public virtual void setMouseClick(Point mousePos)
        {

        }

		/// <summary>
		/// Sets the mouse hover.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		public virtual void setMouseHover(Point mousePos)
		{

		}

        /// <summary>
        /// Sets the input.
        /// </summary>
        /// <param name="key">Key.</param>
		public virtual void setInput(System.Windows.Forms.Keys key)
        {

        }

		/// <summary>
		/// Resize the specified ScreenresX and ScreenresY.
		/// </summary>
		/// <param name="ScreenresX">Screenres x.</param>
		/// <param name="ScreenresY">Screenres y.</param>
		public virtual void resize(int ScreenresX, int ScreenresY)
		{
			xRes = ScreenresX;
			yRes = ScreenresY;

			buffer = new Bitmap (xRes, yRes);
		}

		/// <summary>
		/// Sets whether Gui is active.
		/// </summary>
		/// <param name="active">If set to <c>true</c> active.</param>
		public void setActive(bool active)
		{
			this.active = active;
		}

		/// <summary>
		/// Returns whether Gui is active.
		/// </summary>
		/// <returns><c>true</c>, if active was ised, <c>false</c> otherwise.</returns>
		public bool isActive()
		{
			return active;
		}

		/// <summary>
		/// Inverts the boolean active.
		/// </summary>
		public void switchActive()
		{
			active = !active;
		}
    }
}