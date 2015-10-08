using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
		private static World world;
		private static DrawEngine renderer;
        private Point mouseOnClick;
        private int ScreenY;
        private int ScreenX;

		public MainWindow ()

		{
			Text = "MainWindow";
			Height = 1920;
			Width = 1080;
          
 
    
			world = new World (300, 300);

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
            this.ScreenY=this.ClientSize.Width;
            Rectangle recRes = new Rectangle(0, 60, this.ScreenY, 60);
            Rectangle RecSettings = new Rectangle(0, 120, this.ScreenY, 60);

            Rectangle RecQuit = new Rectangle(0, 180, this.ScreenY, 60);
            Point mousepoint = (Cursor.Position);
                if (recRes.Contains(mousepoint))
            {
                renderer.getGui((int)GUI.def).setActive(false);
            }
            else if (RecSettings.Contains(mousepoint))
            {
                renderer.getGui(1).switchActive();
                renderer.getGui((int)GUI.def).switchActive();
            }
            else if (RecQuit.Contains(mousepoint))
            {
                Application.Exit();
            }

            renderer.render();
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
                    //temporary solution for escape if settings is active
                    if (!renderer.getGui(1).isActive() &&!renderer.getGui((int)GUI.def).isActive() || renderer.getGui((int)GUI.def).isActive())
                    {
                        renderer.getGui((int)GUI.def).switchActive();
                    }
                    else if (renderer.getGui(1).isActive())
                    {
                        renderer.getGui((int)GUI.def).setActive(false);
                        renderer.getGui(1).setActive(false);
                    }

				break;
            case Keys.E:
                world.getPlayer().DropItem(world);
                break;
            default:
                return;
            }
			renderer.render ();
        }
    }
}

