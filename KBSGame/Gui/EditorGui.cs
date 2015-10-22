using System;
using System.Drawing;
using System.Collections.Generic;

namespace KBSGame
{
	public class EditorGui : Gui
	{
		private World world;
		private int width;
		private const int margin = 5;
		private int tabbarWidth = StaticVariables.tileSize + margin;
		private Point currentHover;
		private int selectedTab = 0;
		private int selected = -1;
		private int rowLength;

		/// <summary>
		/// Initializes a new instance of the <see cref="KBSGame.EditorGui"/> class.
		/// </summary>
		/// <param name="ID">I.</param>
		/// <param name="screenResX">Screen res x.</param>
		/// <param name="screenResY">Screen res y.</param>
		/// <param name="drawRatio">Draw ratio.</param>
		/// <param name="world">World.</param>
		public EditorGui (int ID, int screenResX, int screenResY, float drawRatio, World world) : base(ID, screenResX, screenResY, drawRatio)
		{
			this.world = world;
			width = Math.Min (StaticVariables.dpi * 2, screenResX / 2);
			rowLength = (width - margin * 2 - tabbarWidth) / StaticVariables.tileSize;
			currentHover = new Point (0, 0);
		}

		/// <summary>
		/// Gets the render.
		/// </summary>
		/// <returns>The render.</returns>
		public override Bitmap getRender()
		{
			var g = Graphics.FromImage(buffer);
			g.Clear(Color.FromArgb(0));

			StringFormat style = new StringFormat ();
			style.Alignment = StringAlignment.Center;
			Font font = new Font ("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes - width, 0, width, yRes);

			switch (selectedTab) {
			case 1:
				renderTerrainTab (g);
				break;
			case 2:
				renderEntityTab (g);
				break;
			default:
				break;
			}

			//Draw tabbar icons
			Bitmap tileIcon = DrawEngine.sprites [world.getTileTypes () [(int)TERRAIN.sandstone].getSpriteID ()].getBitmap ();

			//World icon
			Bitmap bmp = DrawEngine.sprites [(int)SPRITES.icon_world].getBitmap ();
			float xTile = xRes - tabbarWidth;
			float x = xTile + StaticVariables.tileSize * 0.2f;
			float y = margin;
			g.DrawImage (tileIcon, xTile, y, StaticVariables.tileSize, StaticVariables.tileSize);
			y += StaticVariables.tileSize * 0.2f;
			g.DrawImage (bmp, x, y, StaticVariables.tileSize * 0.6f, StaticVariables.tileSize * 0.6f);
			if (selectedTab == 0)
				g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize * 0.6f - Pens.Black.Width, StaticVariables.tileSize * 0.6f - Pens.Black.Width);

			//Terrain icon
			bmp = DrawEngine.sprites [(int)SPRITES.grass].getBitmap ();
			y = margin + StaticVariables.tileSize;
			g.DrawImage (tileIcon, xTile, y, StaticVariables.tileSize, StaticVariables.tileSize);
			y += StaticVariables.tileSize * 0.2f;
			g.DrawImage (bmp, x, y, StaticVariables.tileSize * 0.6f, StaticVariables.tileSize * 0.6f);
			if (selectedTab == 1)
				g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize * 0.6f - Pens.Black.Width, StaticVariables.tileSize * 0.6f - Pens.Black.Width);

			//Entity icon
			bmp = DrawEngine.sprites [(int)SPRITES.key].getBitmap ();
			y = margin + StaticVariables.tileSize*2;
			g.DrawImage (tileIcon, xTile, y, StaticVariables.tileSize, StaticVariables.tileSize);
			y += StaticVariables.tileSize * 0.2f;
			g.DrawImage (bmp, x, y, StaticVariables.tileSize * 0.6f, StaticVariables.tileSize * 0.6f);
			if (selectedTab == 2)
				g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize * 0.6f - Pens.Black.Width, StaticVariables.tileSize * 0.6f - Pens.Black.Width);

			g.Dispose ();
			return this.buffer;
		}

		/// <summary>
		/// Renders the terrain tab.
		/// </summary>
		/// <param name="g">The green component.</param>
		private void renderTerrainTab(Graphics g)
		{
			TerrainTile[] terrainTiles = world.getTileTypes ();

			for (int i = 0; i < terrainTiles.Length; i++) {
				Bitmap bmp = DrawEngine.sprites [terrainTiles [i].getSpriteID ()].getBitmap ();
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
		}

		/// <summary>
		/// Processes a click when terrain tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void terrainTabClick(Point pos) 
		{
			int xArea = xRes - width + margin;
			int yArea = margin;

			int x = pos.X - xArea;
			int y = pos.Y - yArea;

			if (x > rowLength * StaticVariables.tileSize)
				return;

			selected = x / StaticVariables.tileSize + (y / StaticVariables.tileSize) * rowLength;
		}

		/// <summary>
		/// Renders the entity tab.
		/// </summary>
		private void renderEntityTab(Graphics g) 
		{

		}

		/// <summary>
		/// Processes a click when entity tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void entityTabClick(Point pos)
		{

		}

		/// <summary>
		/// Sets the input.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		public override void setMouseClick(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);

			//Clicked in world
			if (mousePos.X < xRes - width) {
				switch (selectedTab) {
				case 1:
					if (selected >= 0) {
						world.setTerrainTileRelative (mousePos, selected);
					}
					break;
				default:
					break;
				}
			} 

			//Click on tabbar
			else if(mousePos.X > xRes - tabbarWidth) { 
				if (mousePos.Y > margin && mousePos.Y < margin + StaticVariables.tileSize) {
					selected = -1;
					selectedTab = 0;
				} else if (mousePos.Y > margin + StaticVariables.tileSize && mousePos.Y < margin + StaticVariables.tileSize*2) {
					selected = -1;
					selectedTab = 1;
				} else if (mousePos.Y > margin + StaticVariables.tileSize*2 && mousePos.Y < margin + StaticVariables.tileSize*3) {
					selected = -1;
					selectedTab = 2;
				}
			} 

			//Clicked on menu
			else { 
				switch (selectedTab) {
				case 1:
					terrainTabClick(mousePos);
					break;
				case 2:
					entityTabClick(mousePos);
					break;
				default:
					break;
				}
			}
		}

		/// <summary>
		/// Sets the mouse hover.
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
		public override void setMouseHover(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);
			currentHover = mousePos;
		}
	}
}

