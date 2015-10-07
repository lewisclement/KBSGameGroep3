using System;
using System.Drawing;
using System.Collections.Generic;

namespace KBSGame
{
	public class DrawEngine
	{
		private Sprite[] sprites;
		private World world;
		private Bitmap buffer;

		private int xRes, yRes;
		private int viewWidth, viewHeight;

		private List<Gui> Interfaces;
		private Gui blockingGui;			//Modal interface, blocking all other input/output

		private Graphics drawingArea; //Store in RAM to minimize createGraphics() calls

		public DrawEngine (World world, Graphics drawingArea, int xResolution, int yResolution)
		{
			this.drawingArea = drawingArea;

			xRes = xResolution;
			yRes = yResolution;

			setView(xRes / StaticVariables.tileSize, yRes / StaticVariables.tileSize);

			buffer = new Bitmap (viewWidth * StaticVariables.tileSize, viewHeight * StaticVariables.tileSize);
			this.world = world;

			Interfaces = new List<Gui>();
			//Gui gui = new Gui ((int)GUI.def, xRes, yRes);	//Temporary static Gui
            Gui menu = new Menu((int)GUI.def, xRes, yRes);
            Interfaces.Add (menu);

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

			foreach (Gui gui in Interfaces) {
				if (!gui.isActive ())
					continue;

				Bitmap render = gui.getRender ();
				drawingArea.DrawImage (render, 0, 0, render.Width, render.Height);
			}
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

			foreach (Gui gui in Interfaces) { 
				gui.resize (xRes, yRes);
			}
		}

		private void setView(int Width, int Height) 
		{
			viewWidth = Math.Max(1, Width);
			viewHeight = Math.Max(1, Height);
		}

		public Gui getGui(int ID)
		{
			return Interfaces [ID];
		}
	}
}

