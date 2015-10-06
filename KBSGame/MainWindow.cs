using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
		private static DrawEngine renderer;

		public MainWindow ()

		{
			Text = "MainWindow";
			Height = 500;
			Width = 500;

			//this.DoubleBuffered = true;

			renderer = new DrawEngine (this.CreateGraphics(), Height, Width);
		}

		protected override void OnPaint(PaintEventArgs e) 
		{
			renderer.render ();
		}

		protected override void OnResize(EventArgs e) 
		{
			if (renderer == null)
				return;

			renderer.resize (this.CreateGraphics(), this.ClientSize.Width, this.ClientSize.Height);
			Invalidate();
		}

		protected override void OnClick(EventArgs e)
		{
			Invalidate();
		}
	}
}

