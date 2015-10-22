using System;
using System.Drawing;
using System.Collections.Generic;

namespace KBSGame
{
	public class EditorGui : Gui
	{
		World world;
		private int width;
		private const int margin = 5;
		Point currentHover;
		int selected= -1;
		int rowLength;

		public EditorGui (int ID, int screenResX, int screenResY, float drawRatio, World world) : base(ID, screenResX, screenResY, drawRatio)
		{
			this.world = world;
			width = Math.Min (StaticVariables.dpi * 2, screenResX / 2);
			rowLength = (width - margin * 2) / StaticVariables.tileSize;
			currentHover = new Point (0, 0);
		}

		public override Bitmap getRender()
		{
			var g = Graphics.FromImage(buffer);
			g.Clear(Color.FromArgb(0));

			StringFormat style = new StringFormat ();
			style.Alignment = StringAlignment.Center;
			Font font = new Font ("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

			TerrainTile[] terrainTiles = world.getTileTypes ();

			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes - width, 0, width, yRes);

			for (int i = 0; i < terrainTiles.Length; i++) {
				Bitmap bmp = (Bitmap)DrawEngine.sprites [terrainTiles [i].getSpriteID ()].getBitmap ();
				int x = xRes - width + margin + (i % rowLength) * StaticVariables.tileSize;
				int y = margin + (i / rowLength) * StaticVariables.tileSize;
				g.DrawImage (bmp, x, y, StaticVariables.tileSize, StaticVariables.tileSize);

				if (selected == i) {
					g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize - Pens.Green.Width, StaticVariables.tileSize - Pens.Green.Width);
					continue;
				}

				if(currentHover.X > x && currentHover.X < x + StaticVariables.tileSize && currentHover.Y > y && currentHover.Y < y + StaticVariables.tileSize)
					g.FillRectangle (new SolidBrush(Color.FromArgb(40, Color.White)), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
				else
					g.FillRectangle (new SolidBrush(Color.FromArgb(40, Color.Black)), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
			}

			if (currentHover.X < xRes - width) { //Pos in world
				if (selected >= 0) {
					PointF offset = world.getViewOffset ();

					int x = (int)(currentHover.X - (currentHover.X + StaticVariables.tileSize * offset.X) % StaticVariables.tileSize);
					int y = (int)(currentHover.Y - (currentHover.Y + StaticVariables.tileSize * offset.Y) % StaticVariables.tileSize);

					g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize, StaticVariables.tileSize);
				}
			}

			g.Dispose ();
			return this.buffer;
		}

		public override void setMouseClick(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);

			if (mousePos.X < xRes - width) { //Clicked in world
				if (selected >= 0) {
					world.setTerrainTileRelative (mousePos, selected);
				}
			} else { //Clicked on menu
				int xArea = xRes - width + margin;
				int yArea = margin;

				int x = mousePos.X - xArea;
				int y = mousePos.Y - yArea;

				selected = x / StaticVariables.tileSize + (y / StaticVariables.tileSize) * rowLength;
			}
		}

		public override void setMouseHover(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);
			currentHover = mousePos;
		}
	}
}

