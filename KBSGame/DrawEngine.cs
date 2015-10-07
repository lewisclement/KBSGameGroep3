using System;
using System.Drawing;

namespace KBSGame
{
	public class DrawEngine
	{
		private Sprite[] sprites;
		private World world;
        private Bitmap buffer;
		private Graphics drawingArea;
		private int xRes, yRes;
		private int viewWidth, viewHeight;

		public DrawEngine (World world, Graphics drawingArea, int xResolution, int yResolution)
		{
			this.drawingArea = drawingArea;

			xRes = xResolution;
			yRes = yResolution;

			setView(xRes / StaticVariables.tileSize, yRes / StaticVariables.tileSize);

			buffer = new Bitmap (viewWidth * StaticVariables.tileSize, viewHeight * StaticVariables.tileSize);
			this.world = world;


			//Temporary static solution
			sprites = new Sprite[(int)SPRITES.count];
			sprites [(int)SPRITES.water] = new Sprite ((int)SPRITES.water, StaticVariables.execFolder + "/water_still.png");
			sprites [(int)SPRITES.grass] = new Sprite ((int)SPRITES.grass, StaticVariables.execFolder + "/grass_top.png");
			sprites [(int)SPRITES.sand] = new Sprite ((int)SPRITES.sand, StaticVariables.execFolder + "/sand.png");
			sprites [(int)SPRITES.player] = new Sprite ((int)SPRITES.player, StaticVariables.execFolder + "/player.png");
			//Temporary
		}

        public void render()
		{
			var g = Graphics.FromImage (buffer);
			g.Clear (Color.White);

			drawTerrain (g);
			drawEntities (g);

			drawingArea.DrawImage (buffer, 0, 0, xRes, yRes);
		}

		public void drawTerrain(Graphics area)
		{
			//Quick'n'dirty terrain drawing
			TerrainTile[] tiles = world.getTilesView (viewWidth, viewHeight);
			for (int i = 0; i < tiles.Length; i++) {
				int x = (i / viewHeight) * StaticVariables.tileSize;
				int y = (i % viewHeight) * StaticVariables.tileSize;
				area.DrawImage (sprites [tiles [i].getSpriteID ()].getBitmap(), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
			}
		}

		public void drawEntities(Graphics area)
		{
			Entity[] entities = world.getEntitiesView (viewWidth, viewHeight);
			Rectangle view = world.getView (viewWidth, viewHeight);

			for (int i = 0; i < entities.Length; i++) {
				int x = (entities [i].getLocation ().X - view.Left) * StaticVariables.tileSize;
				int y = (entities [i].getLocation ().Y - view.Top) * StaticVariables.tileSize;
				area.DrawImage (sprites [entities [i].getSpriteID ()].getBitmap (), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
			}
		}

		public void resize(Graphics drawingArea, int xResolution, int yResolution)
		{
			this.drawingArea = drawingArea;

			xRes = xResolution;
			yRes = yResolution;

			setView(xRes / StaticVariables.tileSize, yRes / StaticVariables.tileSize);

			buffer = new Bitmap (viewWidth * StaticVariables.tileSize, viewHeight * StaticVariables.tileSize);
		}

		private void setView(int Width, int Height) 
		{
			viewWidth = Math.Max(1, Width);
			viewHeight = Math.Max(1, Height);
		}
	}
}

