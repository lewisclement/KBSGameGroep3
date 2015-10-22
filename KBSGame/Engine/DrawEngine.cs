using System;
using System.Drawing;
using System.Collections.Generic;
using KBSGame.gui;

namespace KBSGame
{
	public class DrawEngine
	{
		public static Sprite[] sprites; 				//List of existing Sprites in game
		private World world;
		private Bitmap buffer; 					//Back buffer to which the view is drawn

		private int xRes, yRes; 				//Resolution of buffer
		private int xDrawRes, yDrawRes;

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

			buffer = new Bitmap (StaticVariables.viewWidth * StaticVariables.tileSize, StaticVariables.viewHeight * StaticVariables.tileSize);
			this.world = world;

			// Load sprites
			sprites = getSprites();

			Menu menu = new Menu ((int)GUI.def, xRes, yRes, xRes / xDrawRes, world);

			Interfaces = new List<Gui> ();
			Interfaces.Insert ((int)GUI.def, menu);       
			Interfaces.Insert ((int)GUI.gameover, new GameOverMenu((int)GUI.gameover, xRes, yRes, xRes / xDrawRes, "Oh no.. you died?", world));   //Game over menu
			     //Temporary static Gui
			Interfaces.Insert ((int)GUI.finish, new FinishMenu((int)GUI.finish, xRes, yRes, xRes / xDrawRes, "Finished!", world));               //Create FinishedMenu GUI
			Interfaces.Insert ((int)GUI.guiinventory, new GuiInventory((int) GUI.guiinventory, xRes, yRes, xRes / xDrawRes, world.getPlayer(), sprites)); //Inventory GUI
			Interfaces.Insert ((int)GUI.editor, menu.getEditorGui());
		}

		/// <summary>
		/// Render de view naar scherm
		/// </summary>
		public void render()
		{ 
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
		}

		/// <summary>
		/// Draws the terrain.
		/// </summary>
		/// <param name="area">Area.</param>
		public void drawTerrain(Graphics area)
		{
			//Get array of tiles that are within view
			TerrainTile[] tiles = world.getTilesView ();

			PointF offset = world.getViewOffset ();

			for (int i = 0; i < tiles.Length; i++) {
				//Retreive coordinate based on index
				int x = (int)((i / StaticVariables.viewHeight - offset.X) * StaticVariables.tileSize);
				int y = (int)((i % StaticVariables.viewHeight - offset.Y) * StaticVariables.tileSize);
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
			Entity[] entities = world.getEntitiesView ();
			Rectangle view = world.getView ();

			PointF offset = world.getViewOffset ();

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

			setView(xDrawRes / StaticVariables.tileSize + 1, yDrawRes / StaticVariables.tileSize + 1);

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
			StaticVariables.viewWidth = Math.Max(1, Width+1);
			StaticVariables.viewHeight = Math.Max(1, Height+1);
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
			sprites[(int)SPRITES.water] = new Sprite((int)SPRITES.water, StaticVariables.spriteFolder + "/water_still.png");
			sprites[(int)SPRITES.grass] = new Sprite((int)SPRITES.grass, StaticVariables.spriteFolder + "/grass_top.png");
			sprites[(int)SPRITES.sand] = new Sprite((int)SPRITES.sand, StaticVariables.spriteFolder + "/sand.png");
			sprites[(int)SPRITES.player] = new Sprite((int)SPRITES.player, StaticVariables.spriteFolder + "/player.png");
			sprites[(int)SPRITES.dirt] = new Sprite((int)SPRITES.dirt, StaticVariables.spriteFolder + "/dirt.png");
			sprites[(int)SPRITES.sapling1] = new Sprite((int)SPRITES.sapling1, StaticVariables.spriteFolder + "/sapling1.png");
			sprites[(int)SPRITES.sapling2] = new Sprite((int)SPRITES.sapling2, StaticVariables.spriteFolder + "/sapling2.png");
			sprites[(int)SPRITES.tallgrass] = new Sprite((int)SPRITES.tallgrass, StaticVariables.spriteFolder + "/tallgrass.png");
			sprites[(int)SPRITES.waterlily] = new Sprite((int)SPRITES.waterlily, StaticVariables.spriteFolder + "/waterlily.png");
			sprites[(int)SPRITES.banana] = new Sprite((int)SPRITES.banana, StaticVariables.spriteFolder + "/banana.png");
			sprites[(int)SPRITES.key] = new Sprite((int)SPRITES.key, StaticVariables.spriteFolder + "/gold_key.png");
			sprites[(int)SPRITES.finish] = new Sprite((int)SPRITES.finish, StaticVariables.spriteFolder + "/finish.png");
			sprites[(int)SPRITES.berrybush] = new Sprite((int)SPRITES.berrybush, StaticVariables.spriteFolder + "/berrybush.png");
			sprites[(int)SPRITES.trapOpened] = new Sprite((int)SPRITES.trapOpened, StaticVariables.spriteFolder + "/trap_opened.png");
			sprites[(int)SPRITES.trapClosed] = new Sprite((int)SPRITES.trapClosed, StaticVariables.spriteFolder + "/trap_closed.png");
            return sprites;
		}
	}
}
