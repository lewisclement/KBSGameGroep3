using System;
using System.Drawing;

namespace KBSGame
{
	public class Sprite
	{
		private int ID;
		private Bitmap bitmap;

		public Sprite (int ID, String file)
		{
			this.ID = ID;

			try {
			bitmap = (Bitmap)Image.FromFile (file);
			} catch(System.IO.FileNotFoundException e) {
				Console.WriteLine ("File \"" + file + "\" not found!");
				Console.WriteLine (e.FusionLog);

				//When file not found, draw a placeholder
				bitmap = new Bitmap (StaticVariables.tileSize, StaticVariables.tileSize);

				var g = Graphics.FromImage (bitmap);
				g.Clear (Color.White);
				g.DrawLine (Pens.Black, 0, 0, StaticVariables.tileSize, StaticVariables.tileSize);
				g.DrawLine (Pens.Black, 0, StaticVariables.tileSize, StaticVariables.tileSize, 0);
			}
		}

		public Bitmap getBitmap()
		{
			return bitmap;
		}

		public int getID()
		{
			return ID;
		}
	}
}

