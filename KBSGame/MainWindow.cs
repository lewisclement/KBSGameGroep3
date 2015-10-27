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
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Text = "MainWindow";
			Height = 1920;
			Width = 1080;
            MinimumSize = new Size(320, 320);
             
			world = new World(300, 300);

            Graphics g = this.CreateGraphics ();
			StaticVariables.dpi = (int)g.DpiX;

			renderer = new DrawEngine (world, g, this.ClientSize.Width, this.ClientSize.Height);

            timer.Interval = 1000 / 60;
            timer.Tick += Gameloop_Tick;

			timer.Start();
        }

		~MainWindow()
		{
			world = null;
			renderer = null;
		}

        private void Gameloop_Tick(object sender, EventArgs e)
        {
			long startTick = System.DateTime.UtcNow.Ticks;
            renderer.render();
			Text = 10000000 / (System.DateTime.UtcNow.Ticks - startTick) + " fps";
        }

		protected override void OnResize(EventArgs e) 
		{
		    renderer?.resize (this.CreateGraphics(), this.ClientSize.Width, this.ClientSize.Height);
		}

		protected override void OnClick(EventArgs e)
		{
			for (int i = 0; i < renderer.getGuiCount (); i++) {
				if (renderer.getGui (i).isActive ())
					renderer.getGui (i).setMouseClick(PointToClient(Cursor.Position));
			}
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			for (int i = 0; i < renderer.getGuiCount (); i++) {
				if (renderer.getGui (i).isActive ())
					renderer.getGui (i).setMouseHover(PointToClient(Cursor.Position));
			}
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);

        }

      

        protected override void OnKeyDown(KeyEventArgs e)
        {
			Player player = world.getPlayer ();

			if(player != null)
			switch (e.KeyCode) {
			case Keys.Up:
				        player.move (world, new PointF (0.0f, -0.2f));
			            player.CurrentDirection = (int) Player.Direction.Up;
				        break;
			case Keys.Down:
				        player.move (world, new PointF (0.0f, 0.2f));
                        player.CurrentDirection = (int)Player.Direction.Down;
                        break;
			case Keys.Left:
				        player.move (world, new PointF (-0.2f, 0.0f));
                        player.CurrentDirection = (int)Player.Direction.Left;
                        break;
			case Keys.Right:
				        player.move (world, new PointF (0.2f, 0.0f));
                        player.CurrentDirection = (int)Player.Direction.Right;
                        break;
			case Keys.Space:
				player.PickupItems (world);
				break;
			case Keys.Z:
				player.DropItem (world);
				break;
				}
			else
			switch (e.KeyCode) {
			case Keys.Up:
				world.getFocusEntity ().move (world, new PointF (0.0f, -0.2f));
				break;
			case Keys.Down:
				world.getFocusEntity ().move (world, new PointF (0.0f, 0.2f));
				break;
			case Keys.Left:
				world.getFocusEntity ().move (world, new PointF (-0.2f, 0.0f));
				break;
			case Keys.Right:
				world.getFocusEntity ().move (world, new PointF (0.2f, 0.0f));
				break;
			}
				

			switch(e.KeyCode) {
			case Keys.Escape:
				renderer.getGui ((int)GUI.def).setInput (Keys.Escape);
				break;
            case Keys.K:
                renderer.getGui((int)GUI.gameover).switchActive();
                break;

            case Keys.L:
                renderer.getGui((int)GUI.finish).switchActive(); //GUI finish innitiated when L is pressed.
                break;
            case Keys.E:
                world.getPlayer().DropItem(world);
                break;
            case Keys.I:
                    renderer.getGui((int) GUI.guiinventory).switchActive();
                break;
            default:
                return;
            }
        }

	    public World getWorld()
	    {
	        return world;
	    }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}

