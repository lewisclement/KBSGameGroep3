using System;
using System.Drawing;
using System.Collections.Generic;
using KBSGame.gui;

namespace KBSGame
{
	public class DrawEngine
	{
		private Sprite[] sprites; 				//List of existing Sprites in game
		private World world;
		private Bitmap buffer; 					//Back buffer to which the view is drawn

		private int xRes, yRes; 				//Resolution of buffer
		private int xDrawRes, yDrawRes;
		private int viewWidth, viewHeight;		//Size of view in tiles

		private List<Gui> Interfaces;			//List of GUI
		private Gui modalGui;				//Modal interface, blocking all other input/output

		private Graphics drawingArea; 			//Store in RAM to minimize createGraphics() calls

		/// <summary>
		/// Initializes a new instance of the <see cref="KBSGame.DrawEngine"/> class.
		/// Runs all necessary code to draw a world to the screen.
		/// </summary>
		/// <param name="world">World.</param>
		/// <param name="drawingArea">Drawing area.</param>
		/// <param name="xResolution">X resolution.</param>
		/// <param name="yResolution">Y resolution.</param>
		public DrawEngine (World world, Graphics drawingArea, int xResolution, int yResolution)
		{
			this.drawingArea = drawingArea;

			xRes = xResolution;
			yRes = yResolution;

			xDrawRes = xRes / 2;
			yDrawRes = yRes / 2;

			setView(xRes / StaticVariables.tileSize, yRes / StaticVariables.tileSize);

			buffer = new Bitmap (viewWidth * StaticVariables.tileSize, viewHeight * StaticVariables.tileSize);
			this.world = world;

			// Load sprites
			sprites = getSprites();

            Interfaces = new List<Gui>
            {
				new Menu ((int)GUI.def, xRes, yRes, xRes / xDrawRes, "Pause"),                           //Temporary static Gui
				new FinishMenu((int)GUI.finish, xRes, yRes, xRes / xDrawRes, "Finished!", world),               //Create FinishedMenu GUI
				new GameOverMenu((int)GUI.gameover, xRes, yRes, xRes / xDrawRes, "Oh no.. you died?", world),   //Game over menu
				new GuiInventory((int) GUI.guiinventory, xRes, yRes, xRes / xDrawRes, world.getPlayer(), sprites) //Inventory GUI
            };
		}

		/// <summary>
		/// Render de view naar scherm
		/// </summary>
		public void render()
		{ 
			long startTick = System.DateTime.UtcNow.Ticks;
			var g = Graphics.FromImage (buffer);
			g.Clear (Color.White);

			drawTerrain (g);
			drawEntities (g);

			foreach (Gui gui in Interfaces) {
				if (!gui.isActive ())
					continue;

				Bitmap render = gui.getRender ();
				g.DrawImage (render, 0, 0, render.Width, render.Height);
			}

			drawingArea.DrawImage (buffer, 0, 0, xRes, yRes);
			g.Dispose ();

			Console.WriteLine (10000000 / (System.DateTime.UtcNow.Ticks - startTick) + " fps");
		}

		/// <summary>
		/// Draws the terrain.
		/// </summary>
		/// <param name="area">Area.</param>
		public void drawTerrain(Graphics area)
		{
			//Get array of tiles that are within view
			TerrainTile[] tiles = world.getTilesView (viewWidth, viewHeight);

			PointF offset = world.getViewOffset (viewWidth, viewHeight);

			for (int i = 0; i < tiles.Length; i++) {
				//Retreive coordinate based on index
				int x = (int)((i / viewHeight - offset.X) * StaticVariables.tileSize);
				int y = (int)((i % viewHeight - offset.Y) * StaticVariables.tileSize);
				area.DrawImage (sprites [tiles [i].getSpriteID ()].getBitmap(), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
			}
		}

		/// <summary>
		/// Draws the entities.
		/// </summary>
		/// <param name="area">Area.</param>
		public void drawEntities(Graphics area)
		{
			//Get array of entities that are within view
			Entity[] entities = world.getEntitiesView (viewWidth, viewHeight);
			Rectangle view = world.getView (viewWidth, viewHeight);

			PointF offset = world.getViewOffset (viewWidth, viewHeight);

			foreach (Entity t in entities)
			{
				float x = (t.getLocation ().X - view.Left - 0.5f - offset.X) * StaticVariables.tileSize;
				float y = (t.getLocation().Y - view.Top - 0.5f - offset.Y) * StaticVariables.tileSize;
				area.DrawImage (sprites [t.getSpriteID ()].getBitmap (), x, y, StaticVariables.tileSize, StaticVariables.tileSize);
			}
		}

		/// <summary>
		/// Resize the specified drawingArea, xResolution and yResolution.
		/// </summary>
		/// <param name="drawingArea">Drawing area.</param>
		/// <param name="xResolution">X resolution.</param>
		/// <param name="yResolution">Y resolution.</param>
		public void resize(Graphics drawingArea, int xResolution, int yResolution)
		{
			this.drawingArea = drawingArea;

			xRes = xResolution;
			yRes = yResolution;

			xDrawRes = xRes / 2;
			yDrawRes = yRes / 2;

			setView(xDrawRes / StaticVariables.tileSize, yDrawRes / StaticVariables.tileSize);

			buffer = new Bitmap (xDrawRes, yDrawRes);

			foreach (Gui gui in Interfaces) {
				gui.resize (xDrawRes, yDrawRes, xRes / xDrawRes);
			}
		}

		/// <summary>
		/// Sets the view.
		/// </summary>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		private void setView(int Width, int Height) 
		{
			viewWidth = Math.Max(1, Width+1);
			viewHeight = Math.Max(1, Height+1);
		}

		/// <summary>
		/// Gets the GUI based on ID.
		/// </summary>
		/// <returns>The GUI.</returns>
		/// <param name="ID">I.</param>
		public Gui getGui(int ID)
		{
			return Interfaces[ID];
		}

		/// <summary>
		/// Gets the GUI count.
		/// </summary>
		/// <returns>The GUI count.</returns>
		public int getGuiCount()
		{
			return Interfaces.Count;
		}

		/// <summary>
		/// Gets the sprites.
		/// </summary>
		/// <returns>The sprites.</returns>
		private Sprite[] getSprites()
		{
			Sprite[] sprites = new Sprite[(int)SPRITES.count];
			sprites[(int)SPRITES.water] = new Sprite((int)SPRITES.water, StaticVariables.execFolder + "/water_still.png");
			sprites[(int)SPRITES.grass] = new Sprite((int)SPRITES.grass, StaticVariables.execFolder + "/grass_top.png");
			sprites[(int)SPRITES.sand] = new Sprite((int)SPRITES.sand, StaticVariables.execFolder + "/sand.png");
			sprites[(int)SPRITES.player] = new Sprite((int)SPRITES.player, StaticVariables.execFolder + "/player.png");
			sprites[(int)SPRITES.dirt] = new Sprite((int)SPRITES.dirt, StaticVariables.execFolder + "/dirt.png");
			sprites[(int)SPRITES.sapling1] = new Sprite((int)SPRITES.sapling1, StaticVariables.execFolder + "/sapling1.png");
			sprites[(int)SPRITES.sapling2] = new Sprite((int)SPRITES.sapling2, StaticVariables.execFolder + "/sapling2.png");
			sprites[(int)SPRITES.tallgrass] = new Sprite((int)SPRITES.tallgrass, StaticVariables.execFolder + "/tallgrass.png");
			sprites[(int)SPRITES.waterlily] = new Sprite((int)SPRITES.waterlily, StaticVariables.execFolder + "/waterlily.png");
			sprites[(int)SPRITES.banana] = new Sprite((int)SPRITES.banana, StaticVariables.execFolder + "/banana.png");
			sprites[(int)SPRITES.key] = new Sprite((int)SPRITES.key, StaticVariables.execFolder + "/gold_key.png");
			sprites[(int)SPRITES.finish] = new Sprite((int)SPRITES.finish, StaticVariables.execFolder + "/finish.png");
			sprites[(int)SPRITES.berrybush] = new Sprite((int)SPRITES.berrybush, StaticVariables.execFolder + "/berrybush.png");
			return sprites;
		}
	}
}
