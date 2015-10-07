using System;
using System.Windows.Forms;

namespace KBSGame
{
	class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			StaticVariables.execFolder = System.IO.Path.GetDirectoryName (Application.ExecutablePath);
            //var f = new MainWindow ();
            //Gui g = new Gui(500, 500);
            Menu m = new Menu(500, 500);
			Application.Run (m);
		}
	}
}
