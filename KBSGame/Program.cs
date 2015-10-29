using System;
using System.Windows.Forms;

namespace KBSGame
{
	class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
			customCulture.NumberFormat.NumberDecimalSeparator = ".";

			System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

			StaticVariables.execFolder = System.IO.Path.GetDirectoryName (Application.ExecutablePath);
			StaticVariables.levelFolder = StaticVariables.execFolder + "/worlds";
			StaticVariables.spriteFolder = StaticVariables.execFolder + "/sprites";
            StaticVariables.textFolder = StaticVariables.execFolder + "/text";
            StaticVariables.musicFolder = StaticVariables.execFolder + "/music";

            var f = new MainWindow ();
            Application.Run (f);
		}
	}
}
