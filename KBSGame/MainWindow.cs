using System;
using System.Drawing;
using System.Windows.Forms;

namespace KBSGame
{
	public class MainWindow : Form
	{
        private Timer timer = new Timer();

		Controller control;

		public MainWindow ()
		{
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Text = "MainWindow";
			Height = 1920;
			Width = 1080;
            MinimumSize = new Size(320, 320);
             
			StaticVariables.world = new World(300, 300);

            Graphics g = this.CreateGraphics ();
			StaticVariables.dpi = (int)g.DpiX;

			StaticVariables.renderer = new DrawEngine (StaticVariables.world, g, this.ClientSize.Width, this.ClientSize.Height);

			StaticVariables.controller = new Controller ();
			control = StaticVariables.controller;

            timer.Interval = 1000 / 60;
			timer.Tick += StaticVariables.controller.cycle;

			timer.Start();
        }

		~MainWindow()
		{
			StaticVariables.world = null;
			StaticVariables.renderer = null;
		}

		protected override void OnResize(EventArgs e) 
		{
			StaticVariables.renderer?.resize (this.CreateGraphics(), this.ClientSize.Width, this.ClientSize.Height);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			control.mouseClick (PointToClient (Cursor.Position), e.Button == MouseButtons.Left);
        }

		protected override void OnMouseMove(MouseEventArgs e)
		{
			control.mouseHover (PointToClient (Cursor.Position));
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
			StaticVariables.controller.setKeyPress (e.KeyCode);
        }

		protected override void OnKeyUp(KeyEventArgs e)
		{
			StaticVariables.controller.setKeyRelease (e.KeyCode);
		}

	    public World getWorld()
	    {
	        return StaticVariables.world;
	    }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}

