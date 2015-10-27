using System;
using System.IO;
using System.Windows.Forms;

namespace KBSGame
{
	class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			StaticVariables.execFolder = Path.GetDirectoryName (Application.ExecutablePath);
            StaticVariables.levelFolder = Path.Combine(StaticVariables.execFolder, "worlds");
			StaticVariables.spriteFolder = Path.Combine(StaticVariables.execFolder, "sprites");
            StaticVariables.textFolder = Path.Combine(StaticVariables.execFolder, "text");

            var f = new MainWindow ();
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(StaticVariables.execFolder + ("/On_the_moon.wav"));
            //player.Play();
            Application.Run (f);
		}
	}
}
