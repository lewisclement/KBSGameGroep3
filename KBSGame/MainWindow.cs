using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
		private static World world;
		private static DrawEngine renderer;
        private int ScreenY;
        private int ScreenX;
        private Timer timer = new Timer();

		public MainWindow ()

		{
			Text = "MainWindow";
			Height = 1920;
			Width = 1080;
          
 
			world = new World (300, 300);

			Graphics g = this.CreateGraphics ();
			StaticVariables.dpi = (int)g.DpiX;

			renderer = new DrawEngine (world, g, this.ClientSize.Width, this.ClientSize.Height);

            timer.Interval = 1000 / 60;
            timer.Tick += Gameloop_Tick;

            timer.Start();
        }

        private void Gameloop_Tick(object sender, EventArgs e)
        {
            renderer.render();
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
			for (int i = 0; i < renderer.getGuiCount (); i++) {
				if (renderer.getGui (i).isActive ())
					renderer.getGui (i).setMouseClick(PointToClient(Cursor.Position));
			}

            renderer.render();
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			for (int i = 0; i < renderer.getGuiCount (); i++) {
				if (renderer.getGui (i).isActive ())
					renderer.getGui (i).setMouseHover(PointToClient(Cursor.Position));
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
                world.getEntities()[0].move(world, new PointF(0.0f, -0.4f));
                break;
            case Keys.Down:
                world.getEntities()[0].move(world, new PointF(0.0f, 0.2f));
                break;
            case Keys.Left:
                world.getEntities()[0].move(world, new PointF(-0.2f, 0.0f));
                break;
            case Keys.Right:
                world.getEntities()[0].move(world, new PointF(0.2f, 0.0f));
                break;
			case Keys.Escape:
                renderer.getGui((int)GUI.def).switchActive();
				break;
            case Keys.K:
                renderer.getGui((int)GUI.gameover).switchActive();
                break;
            case Keys.E:
                //world.getPlayer().DropItem(world);
                break;
                case Keys.I:
                    renderer.getGui((int) GUI.guiinventory);
                    break;
            default:
                return;
            }
        }

	    public World getWorld()
	    {
	        return world;
	    }
	}
}

