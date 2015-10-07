using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
		private static World world;
		private static DrawEngine renderer;
        private static Menu menu;

		public MainWindow ()

		{
			Text = "MainWindow";
			Height = 500;
			Width = 500;

			world = new World (100, 100);

			renderer = new DrawEngine (world, this.CreateGraphics(), Height, Width);
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
			renderer.render ();
		}

		protected override void OnClick(EventArgs e)
		{
			renderer.render ();
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MainWindow";
            this.ResumeLayout(false);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    world.getEntity(0).move(world, new Point(0, -1));
                    break;
                case Keys.Down:
                    world.getEntity(0).move(world, new Point(0, 1));
                    break;
                case Keys.Left:
                    world.getEntity(0).move(world, new Point(-1, 0));
                    break;
                case Keys.Right:
                    world.getEntity(0).move(world, new Point(1, 0));
                    break;
                default:
                    return;
            }
			renderer.render ();
        }
    }
}

