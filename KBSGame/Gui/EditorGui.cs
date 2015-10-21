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

		public EditorGui (int ID, int screenResX, int screenResY, float drawRatio, World world) : base(ID, screenResX, screenResY, drawRatio)
		{
			this.world = world;
			width = Math.Min (StaticVariables.dpi * 2, screenResX / 2);
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
				int x = xRes - width + margin + i * StaticVariables.tileSize;
				int y = margin;
				g.DrawImage (bmp, x, y, StaticVariables.tileSize, StaticVariables.tileSize);

				if (selected == i) {
					g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize - Pens.Green.Width, StaticVariables.tileSize - Pens.Green.Width);
					continue;
				}

				if(currentHover.X > x && currentHover.X < x + StaticVariables.tileSize && currentHover.Y > y && currentHover.Y < y + StaticVariables.tileSize)
					g.FillRectangle (new SolidBrush(Color.FromArgb(40, Color.White)), x, margin, StaticVariables.tileSize, StaticVariables.tileSize);
				else
					g.FillRectangle (new SolidBrush(Color.FromArgb(40, Color.Black)), x, margin, StaticVariables.tileSize, StaticVariables.tileSize);
			}

			if (currentHover.X < xRes - width) { //Pos in world
				if (selected >= 0) {
					PointF focusPos = world.getFocusEntity ().getLocation ();

					int offsetX = (int)(StaticVariables.tileSize * (focusPos.X - (int)focusPos.X));
					int offsetY = (int)(StaticVariables.tileSize * (focusPos.Y - (int)focusPos.Y));

					int x = currentHover.X - currentHover.X % StaticVariables.tileSize - offsetX;
					int y = currentHover.Y - currentHover.Y % StaticVariables.tileSize - offsetY;

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

				selected = x / StaticVariables.tileSize;
			}
		}

		public override void setMouseHover(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);
			currentHover = mousePos;
		}
	}
}

