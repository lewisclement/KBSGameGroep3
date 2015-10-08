using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
		private static World world;
		private static DrawEngine renderer;

		public MainWindow ()

		{
			Text = "MainWindow";
			Height = 1920;
			Width = 1080;
          
 
    
			world = new World (100, 100);

			renderer = new DrawEngine (world, this.CreateGraphics(), this.ClientSize.Width, this.ClientSize.Height);
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
                world.getEntities()[0].move(world, new Point(0, -1));
                break;
            case Keys.Down:
                world.getEntities()[0].move(world, new Point(0, 1));
                break;
            case Keys.Left:
                world.getEntities()[0].move(world, new Point(-1, 0));
                break;
            case Keys.Right:
                world.getEntities()[0].move(world, new Point(1, 0));
                break;
			case Keys.Escape:
				renderer.getGui ((int)GUI.def).switchActive ();
				break;

            default:
                return;
            }
			renderer.render ();
        }
    }
}

