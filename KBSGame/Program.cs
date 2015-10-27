using System4
using System.Windows.Forms;

namespace KBSGame
{
	class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			StaticVariables.execFolder = System.IO.Path.GetDirectoryName (Application.ExecutablePath);
			StaticVariables.levelFolder = StaticVariables.execFolder + "/worlds";
			StaticVariables.spriteFolder = StaticVariables.execFolder + "/sprites";
            StaticVariables.textFolder = StaticVariables.execFolder + "/text";

			var f = new MainWindow ();
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(StaticVariables.execFolder + ("/On_the_moon.wav"));
            //player.Play();
            Application.Run (f);
		}
	}
}
