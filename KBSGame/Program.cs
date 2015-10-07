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
			var f = new MainWindow ();

			Application.Run (f);
		}
	}
}
