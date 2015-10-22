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

			sprites[(int)SPRITES.player] = new Sprite((int)SPRITES.player, StaticVariables.spriteFolder + "/player.png");
			sprites[(int)SPRITES.banana] = new Sprite((int)SPRITES.banana, StaticVariables.spriteFolder + "/banana.png");
			sprites[(int)SPRITES.key] = new Sprite((int)SPRITES.key, StaticVariables.spriteFolder + "/gold_key.png");
			sprites[(int)SPRITES.finish] = new Sprite((int)SPRITES.finish, StaticVariables.spriteFolder + "/finish.png");
			sprites[(int)SPRITES.trapOpened] = new Sprite((int)SPRITES.trapOpened, StaticVariables.spriteFolder + "/trap_opened.png");
			sprites[(int)SPRITES.trapClosed] = new Sprite((int)SPRITES.trapClosed, StaticVariables.spriteFolder + "/trap_closed.png");
			sprites[(int)SPRITES.icon_world] = new Sprite((int)SPRITES.icon_world, StaticVariables.spriteFolder + "/icon_world.png");

			//Terrain
			sprites[(int)SPRITES.water] = new Sprite((int)SPRITES.water, StaticVariables.spriteFolder + "/water_still.png");
			sprites[(int)SPRITES.grass] = new Sprite((int)SPRITES.grass, StaticVariables.spriteFolder + "/grass_top.png");
			sprites[(int)SPRITES.sand] = new Sprite((int)SPRITES.sand, StaticVariables.spriteFolder + "/sand.png");
			sprites[(int)SPRITES.dirt] = new Sprite((int)SPRITES.dirt, StaticVariables.spriteFolder + "/dirt.png");
			sprites[(int)SPRITES.clay] = new Sprite((int)SPRITES.clay, StaticVariables.spriteFolder + "/clay.png");
			sprites[(int)SPRITES.brick] = new Sprite((int)SPRITES.brick, StaticVariables.spriteFolder + "/brick.png");
			sprites[(int)SPRITES.farmland] = new Sprite((int)SPRITES.farmland, StaticVariables.spriteFolder + "/farmland_wet.png");
			sprites[(int)SPRITES.planks_birch] = new Sprite((int)SPRITES.planks_birch, StaticVariables.spriteFolder + "/planks_birch.png");
			sprites[(int)SPRITES.red_sand] = new Sprite((int)SPRITES.red_sand, StaticVariables.spriteFolder + "/red_sand.png");
			sprites[(int)SPRITES.sandstone] = new Sprite((int)SPRITES.sandstone, StaticVariables.spriteFolder + "/sandstone.png");
			sprites[(int)SPRITES.stone] = new Sprite((int)SPRITES.stone, StaticVariables.spriteFolder + "/stone.png");
			sprites[(int)SPRITES.stonebrick] = new Sprite((int)SPRITES.stonebrick, StaticVariables.spriteFolder + "/stonebrick.png");
			sprites[(int)SPRITES.stone_diorite] = new Sprite((int)SPRITES.stone_diorite, StaticVariables.spriteFolder + "/stone_diorite.png");
			sprites[(int)SPRITES.stone_granite] = new Sprite((int)SPRITES.stone_granite, StaticVariables.spriteFolder + "/stone_granite.png");

			//Saplings
			sprites[(int)SPRITES.sapling1] = new Sprite((int)SPRITES.sapling1, StaticVariables.spriteFolder + "/sapling1.png");
			sprites[(int)SPRITES.sapling2] = new Sprite((int)SPRITES.sapling2, StaticVariables.spriteFolder + "/sapling2.png");
			sprites[(int)SPRITES.sapling_acacia] = new Sprite((int)SPRITES.sapling_acacia, StaticVariables.spriteFolder + "/sapling_acacia.png");
			sprites[(int)SPRITES.sapling_birch] = new Sprite((int)SPRITES.sapling_birch, StaticVariables.spriteFolder + "/sapling_birch.png");
			sprites[(int)SPRITES.sapling_jungle] = new Sprite((int)SPRITES.sapling_jungle, StaticVariables.spriteFolder + "/sapling_jungle.png");
			sprites[(int)SPRITES.sapling_oak] = new Sprite((int)SPRITES.sapling_oak, StaticVariables.spriteFolder + "/sapling_oak.png");
			sprites[(int)SPRITES.sapling_roofed_oak] = new Sprite((int)SPRITES.sapling_roofed_oak, StaticVariables.spriteFolder + "/sapling_roofed_oak.png");
			sprites[(int)SPRITES.sapling_spruce] = new Sprite((int)SPRITES.sapling_spruce, StaticVariables.spriteFolder + "/sapling_spruce.png");

			//Flowers
			sprites[(int)SPRITES.flower_rose] = new Sprite((int)SPRITES.flower_rose, StaticVariables.spriteFolder + "/flower_rose.png");
			sprites[(int)SPRITES.flower_oxeye_daisy] = new Sprite((int)SPRITES.flower_oxeye_daisy, StaticVariables.spriteFolder + "/flower_oxeye_daisy.png");
			sprites[(int)SPRITES.flower_houstonia] = new Sprite((int)SPRITES.flower_houstonia, StaticVariables.spriteFolder + "/flower_houstonia.png");
			sprites[(int)SPRITES.flower_dandelion] = new Sprite((int)SPRITES.flower_dandelion, StaticVariables.spriteFolder + "/flower_dandelion.png");
			sprites[(int)SPRITES.flower_blue_orchid] = new Sprite((int)SPRITES.flower_blue_orchid, StaticVariables.spriteFolder + "/flower_blue_orchid.png");
			sprites[(int)SPRITES.flower_allium] = new Sprite((int)SPRITES.flower_allium, StaticVariables.spriteFolder + "/flower_allium.png");
			sprites[(int)SPRITES.flower_tulip_orange] = new Sprite((int)SPRITES.flower_tulip_orange, StaticVariables.spriteFolder + "/flower_tulip_orange.png");
			sprites[(int)SPRITES.flower_tulip_pink] = new Sprite((int)SPRITES.flower_tulip_pink, StaticVariables.spriteFolder + "/flower_tulip_pink.png");
			sprites[(int)SPRITES.flower_tulip_red] = new Sprite((int)SPRITES.flower_tulip_red, StaticVariables.spriteFolder + "/flower_tulip_red.png");
			sprites[(int)SPRITES.flower_tulip_white] = new Sprite((int)SPRITES.flower_tulip_white, StaticVariables.spriteFolder + "/flower_tulip_white.png");

			//Misc plants
			sprites[(int)SPRITES.berrybush] = new Sprite((int)SPRITES.berrybush, StaticVariables.spriteFolder + "/berrybush.png");
			sprites[(int)SPRITES.waterlily] = new Sprite((int)SPRITES.waterlily, StaticVariables.spriteFolder + "/waterlily.png");
			sprites[(int)SPRITES.tallgrass] = new Sprite((int)SPRITES.tallgrass, StaticVariables.spriteFolder + "/tallgrass.png");
			sprites[(int)SPRITES.deadbush] = new Sprite((int)SPRITES.deadbush, StaticVariables.spriteFolder + "/deadbush.png");
			sprites[(int)SPRITES.wheat_stage_7] = new Sprite((int)SPRITES.wheat_stage_7, StaticVariables.spriteFolder + "/wheat_stage_7.png");
			sprites[(int)SPRITES.carrots_stage_0] = new Sprite((int)SPRITES.carrots_stage_0, StaticVariables.spriteFolder + "/carrots_stage_0.png");
			sprites[(int)SPRITES.carrots_stage_1] = new Sprite((int)SPRITES.carrots_stage_1, StaticVariables.spriteFolder + "/carrots_stage_1.png");
			sprites[(int)SPRITES.carrots_stage_2] = new Sprite((int)SPRITES.carrots_stage_2, StaticVariables.spriteFolder + "/carrots_stage_2.png");
			sprites[(int)SPRITES.carrots_stage_3] = new Sprite((int)SPRITES.carrots_stage_3, StaticVariables.spriteFolder + "/carrots_stage_3.png");

            return sprites;
		}
	}
}
