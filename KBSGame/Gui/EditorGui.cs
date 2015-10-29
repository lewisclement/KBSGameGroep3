using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using KBSGame.Entities;

//Temporary dialog solution
public static class Dialog
{
	public static string ShowDialog(string caption, string text)
	{
		Form prompt = new Form();
		prompt.Width = 500;
		prompt.Height = 150;
		prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
		prompt.Text = caption;
		prompt.StartPosition = FormStartPosition.CenterScreen;
		Label textLabel = new Label() { Left = 50, Top=20, Text=text };
		TextBox textBox = new TextBox() { Left = 50, Top=50, Width=400 };
		Button confirmation = new Button() { Text = "Ok", Left=350, Width=100, Top=70, DialogResult = DialogResult.OK };
		confirmation.Click += (sender, e) => { prompt.Close(); };
		prompt.Controls.Add(textBox);
		prompt.Controls.Add(confirmation);
		prompt.Controls.Add(textLabel);
		prompt.AcceptButton = confirmation;

		return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
	}
}

namespace KBSGame
{
	public class EditorGui : Gui
	{
		private World world;

		//Sizes
		private int width;
		private const int margin = 5;
		private int tabbarWidth = StaticVariables.tileSize + margin;
		private int tabWidth;
		private const int lineHeight = 20;
		private Point currentHover;
		private int selectedTab = 0;
		private int selected = -1;
		private int rowLength;
		List<Entity> entityList;
		TerrainTile[] terrainTiles;
		//Save/load tabs
		FileInfo[] files;
		//World tab
		Size worldSize;

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
			tabWidth = width - tabbarWidth - margin*2;

			rowLength = (width - margin * 2 - tabbarWidth) / StaticVariables.tileSize;
			currentHover = new Point (0, 0);

			worldSize = world.getSize ();

			terrainTiles = world.getTileTypes ();
			loadEntities ();
		}

		public void reset(int screenResX, int screenResY, float drawRatio, World world)
		{
			this.world = world;
			base.resize (screenResX, screenResY, drawRatio);

			width = Math.Min (StaticVariables.dpi * 2, screenResX / 2);
			tabWidth = width - tabbarWidth - margin*2;

			rowLength = (width - margin * 2 - tabbarWidth) / StaticVariables.tileSize;
			currentHover = new Point (0, 0);

			worldSize = world.getSize ();

			terrainTiles = world.getTileTypes ();
			loadEntities ();

			selectedTab = 0;
			selected = -1;
		}

		/// <summary>
		/// Gets the render.
		/// </summary>
		/// <returns>The render.</returns>
		public override Bitmap getRender()
		{
			var g = Graphics.FromImage(buffer);
			g.Clear(Color.FromArgb(0));
			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes - width, 0, width, yRes);

			switch (selectedTab) {
			case 0:
				renderWorldTab (g);
				break;
			case 1:
				renderTerrainTab (g);
				break;
			case 2:
				renderEntityTab (g);
				break;
			case 3:
				renderSaveTab (g);
				break;
			case 4:
				renderLoadTab (g);
				break;
			default:
				break;
			}
			drawTabbar (g);
			g.Dispose ();
			return this.buffer;
		}

		/// <summary>
		/// Renders the terrain tab.
		/// </summary>
		/// <param name="g">The green component.</param>
		private void renderTerrainTab(Graphics g)
		{
			renderTerrainSelector (g, 0);

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
			terrainSelectorClick (pos, 0);
		}

		/// <summary>
		/// Renders the terrain selector.
		/// </summary>
		/// <param name="g">The green component.</param>
		/// <param name="verticalPosition">Vertical position.</param>
		private void renderTerrainSelector(Graphics g, int verticalPosition)
		{
			for (int i = 0; i < terrainTiles.Length; i++) {
				Bitmap bmp = DrawEngine.sprites [terrainTiles [i].getSpriteID ()].getBitmap ();
				int x = xRes - width + margin + (i % rowLength) * StaticVariables.tileSize;
				int y = margin + (i / rowLength) * StaticVariables.tileSize + verticalPosition;
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
		}

		/// <summary>
		/// Handles input when clicked on terrain selector
		/// </summary>
		/// <param name="pos">Position.</param>
		private void terrainSelectorClick(Point pos, int verticalPosition) 
		{
			int xArea = xRes - width + margin;
			int yArea = margin;
			int x = pos.X - xArea;
			int y = pos.Y - yArea - verticalPosition;

			if (x > rowLength * StaticVariables.tileSize || y < 0)
				return;
			selected = x / StaticVariables.tileSize + (y / StaticVariables.tileSize) * rowLength;
			if (selected > (int)TERRAIN.count)
				selected = -1;
		}

		/// <summary>
		/// Renders the entity tab.
		/// </summary>
		private void renderEntityTab(Graphics g) 
		{
            for (int i = 0; i < entityList.Count; i++)
			{
				Bitmap bmp = DrawEngine.sprites [entityList [i].getSpriteID ()].getBitmap ();
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
					//Make snap to a grid of 10x10 per tile
					float x = currentHover.X - currentHover.X % (StaticVariables.tileSize / 10.0f) - StaticVariables.tileSize/2;
					float y = currentHover.Y - currentHover.Y % (StaticVariables.tileSize / 10.0f) - StaticVariables.tileSize/2;

					Bitmap bmp = DrawEngine.sprites [entityList [selected].getSpriteID ()].getBitmap ();
					g.DrawImage (bmp, x, y, StaticVariables.tileSize, StaticVariables.tileSize);
				}
			}
		}

		/// <summary>
		/// Processes a click when entity tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void entityTabClick(Point pos)
		{
			int xArea = xRes - width + margin;
			int yArea = margin;

			int x = pos.X - xArea;
			int y = pos.Y - yArea;

			if (x > rowLength * StaticVariables.tileSize)
				return;

			selected = x / StaticVariables.tileSize + (y / StaticVariables.tileSize) * rowLength;

			if (selected > entityList.Count)
				selected = -1;
		}

		/// <summary>
		/// Renders the world tab.
		/// </summary>
		/// <param name="g">The green component.</param>
		private void renderWorldTab(Graphics g) 
		{
			int x = xRes - width + margin;
			int y = margin;
			int xEnd = 0;
			int yEnd = 0;

            Font subFont = new Font("Tahoma", lineHeight / 2, FontStyle.Bold);
            Image worldtab = Image.FromFile(StaticVariables.textFolder + "/editor_worldtab.png");
            g.DrawImage(worldtab, x - 20, y, 180, 30);
			y += lineHeight;
			g.DrawString ("width/height", subFont, new SolidBrush (Color.White), x, y);
			y += lineHeight;
            
			//Hover over width box
			xEnd = x + tabWidth / 3;
            yEnd = (int)(y + lineHeight * 0.6);
			if (currentHover.Y > y && currentHover.Y < yEnd && currentHover.X > x && currentHover.X < xEnd)
				g.FillRectangle (new SolidBrush (Color.FromArgb(40, Color.Black)), x, y, tabWidth / 3, (int)(lineHeight * 0.6));
			//Draw width box
			g.FillRectangle (new SolidBrush (Color.FromArgb(40, Color.Black)), x, y, tabWidth / 3, (int)(lineHeight * 0.6));
			g.DrawString (worldSize.Width.ToString(), subFont, new SolidBrush (Color.White), x, y);

			//Hover over height box
			xEnd = x + tabWidth / 3 * 2 + margin;
			if (currentHover.Y > y && currentHover.Y < yEnd && currentHover.X > x + tabWidth / 3 + margin && currentHover.X < xEnd)
				g.FillRectangle (new SolidBrush (Color.FromArgb(40, Color.Black)), x + tabWidth / 3 + margin, y, tabWidth / 3, (int)(lineHeight * 0.6));
			//Draw height box
			g.FillRectangle (new SolidBrush (Color.FromArgb(40, Color.Black)), x + tabWidth / 3 + margin, y, tabWidth / 3, (int)(lineHeight * 0.6));
			g.DrawString (worldSize.Height.ToString(), subFont, new SolidBrush (Color.White), x + tabWidth / 3 + margin, y);

			y += lineHeight;
			if (selected >= 0) {
				xEnd = x + tabWidth / 2;
				yEnd = y + lineHeight;
				if (currentHover.Y > y && currentHover.Y < yEnd && currentHover.X > x && currentHover.X < xEnd)
					g.FillRectangle (new SolidBrush (Color.Black), x, y, tabWidth / 2, lineHeight);
				else
					g.FillRectangle (new SolidBrush (Color.FromArgb(100, Color.Black)), x, y, tabWidth / 2, lineHeight);
				    g.DrawString ("Fill world", subFont, new SolidBrush (Color.White), x, y);
			}
			y += lineHeight;
			renderTerrainSelector (g, y);
		}

		/// <summary>
		/// Processes a click when world tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void worldTabClick(Point pos)
		{
			terrainSelectorClick (pos, margin + 4 * lineHeight);
			int x = xRes - width + margin;
			int y = margin + lineHeight * 2;

			//Clicked in "world size" row
			if (pos.Y > y && pos.Y < y + lineHeight * 0.6) {
				//Clicked width box
				if (pos.X > x && pos.X < x + tabWidth / 3) {
					String input = Dialog.ShowDialog("Set world width", "World width in tiles:");
					try {
						worldSize.Width = Int32.Parse(input);
						worldSize.Width = Math.Max(StaticVariables.minWorldSize, Math.Min(worldSize.Width, StaticVariables.maxWorldSize));
					} catch (Exception e) {
						Console.WriteLine ("Please supply a decimal number");
					}
				} 

				//Clicked height box
				else if (pos.X > x + tabWidth / 3 + margin && pos.X < x + tabWidth / 3 * 2 + margin) {
					String input = Dialog.ShowDialog("Set world height", "World height in tiles:");

					try {
						worldSize.Height = Int32.Parse(input);
						worldSize.Height = Math.Max(StaticVariables.minWorldSize, Math.Min(worldSize.Height, StaticVariables.maxWorldSize));
					} catch (Exception e) {
						Console.WriteLine ("Please supply a decimal number");
					}
				}
			}

			//Clicked on "Fill world"
			y += lineHeight;
			if (selected >= 0) {
				int xEnd = x + tabWidth / 2;
				int yEnd = y + lineHeight;
				if (pos.Y > y && pos.Y < yEnd && pos.X > x && pos.X < xEnd)
					world.FillWorld ((TERRAIN)terrainTiles[selected].getID(), worldSize);
			}
		}

		/// <summary>
		/// Renders the save tab.
		/// </summary>
		/// <param name="g">The green component.</param>
		private void renderSaveTab(Graphics g)
		{

        }

		/// <summary>
		/// Processes a click when save tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void saveTabClick(Point pos)
		{

		}

		/// <summary>
		/// Renders the load tab.
		/// </summary>
		/// <param name="g">The green component.</param>
		private void renderLoadTab(Graphics g)
		{
			if (files == null)
				return;

			Font font = new Font ("Arial", 10, FontStyle.Bold);

			if (currentHover.X > xRes - width && currentHover.X < xRes - tabbarWidth) {
				int index = (currentHover.Y - margin) / 10;
				if(index < files.Length)
					g.FillRectangle (new SolidBrush (Color.FromArgb (40, Color.Black)), xRes - width, index * 10 + 5, width - tabbarWidth, 10);
			}

			for (int i = 0; i < files.Length; i++) {
				String name = files [i].Name.Substring (0, files [i].Name.Length - 4);
				g.DrawString (name, font, new SolidBrush (Color.White), xRes - width + margin, margin + i * 10 - 5);
			}
		}

		/// <summary>
		/// Processes a click when load tab displayed
		/// </summary>
		/// <param name="pos">Position.</param>
		private void loadTabClick(Point pos)
		{
			if (pos.X > xRes - width && pos.X < xRes - tabbarWidth) {
				int index = (pos.Y - margin) / 10;
				if (index < files.Length) {
					String fileName = files [index].Name.Substring (0, files [index].Name.Length - 4);
					world.loadLevel (fileName);
				}
			}
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
				case 2:
					if (selected >= 0) {
						Entity e = entityList [selected];
                        
						float x = (mousePos.X - mousePos.X % (StaticVariables.tileSize / 10.0f)) / StaticVariables.tileSize;
						float y = (mousePos.Y - mousePos.Y % (StaticVariables.tileSize / 10.0f)) / StaticVariables.tileSize;
						x = (float)Math.Round (x, 1);
						y = (float)Math.Round (y, 1);
                        
                        e = new Entity(e.getType(), new PointF(0, 0), e.getSpriteID(), e.getSolid(), e.getHeight(), e.getDrawOrder(), e.getBoundingBox());
					    world.addEntityRelative (e, new PointF(x, y));
					}
					break;
				default:
					break;
				}
			} 

			//Click on tabbar
			else if(mousePos.X > xRes - tabbarWidth) { 
				if (mousePos.Y > margin && mousePos.Y < margin + StaticVariables.tileSize) {
					//World tab
					selected = -1;
					selectedTab = 0;
				} else if (mousePos.Y > margin + StaticVariables.tileSize && mousePos.Y < margin + StaticVariables.tileSize*2) {
					//Terrain tab
					selected = -1;
					selectedTab = 1;
				} else if (mousePos.Y > margin + StaticVariables.tileSize*2 && mousePos.Y < margin + StaticVariables.tileSize*3) {
					//Entity tab
					selected = -1;
					selectedTab = 2;
				} else if (mousePos.Y > margin + StaticVariables.tileSize*3 && mousePos.Y < margin + StaticVariables.tileSize*4) {
					//Save tab
					//Temporary promt box solution
					string fileName = Dialog.ShowDialog("Save level", "Level name");
					if (fileName != null && fileName != "")
						LevelWriter.saveWorld (world, fileName);
					//selected = -1;
					//selectedTab = 3;

				} else if (mousePos.Y > margin + StaticVariables.tileSize*4 && mousePos.Y < margin + StaticVariables.tileSize*5) {
					//Load tab
					DirectoryInfo d = new DirectoryInfo (StaticVariables.levelFolder);
					files = d.GetFiles ("*.xml");

					selected = -1;
					selectedTab = 4;
				}
			} 

			//Clicked on menu
			else { 
				switch (selectedTab) {
				case 0:
					//World tab
					worldTabClick (mousePos);
					break;
				case 1:
					//Terrain tab
					terrainTabClick (mousePos);
					break;
				case 2:
					//Entity tab
					entityTabClick (mousePos);
					break;
				case 3:
					//Save tab
					saveTabClick (mousePos);
					break;
				case 4:
					//Load tab
					loadTabClick (mousePos);
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

		private void drawTabbar(Graphics g)
		{
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
			bmp = DrawEngine.sprites [(int)SPRITES.grass_normal].getBitmap ();
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

			//Save icon
			bmp = DrawEngine.sprites [(int)SPRITES.save].getBitmap ();
			y = margin + StaticVariables.tileSize*3;
			g.DrawImage (tileIcon, xTile, y, StaticVariables.tileSize, StaticVariables.tileSize);
			y += StaticVariables.tileSize * 0.2f;
			g.DrawImage (bmp, x, y, StaticVariables.tileSize * 0.6f, StaticVariables.tileSize * 0.6f);
			if (selectedTab == 3)
				g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize * 0.6f - Pens.Black.Width, StaticVariables.tileSize * 0.6f - Pens.Black.Width);

			//Load icon
			bmp = DrawEngine.sprites [(int)SPRITES.load].getBitmap ();
			y = margin + StaticVariables.tileSize*4;
			g.DrawImage (tileIcon, xTile, y, StaticVariables.tileSize, StaticVariables.tileSize);
			y += StaticVariables.tileSize * 0.2f;
			g.DrawImage (bmp, x, y, StaticVariables.tileSize * 0.6f, StaticVariables.tileSize * 0.6f);
			if (selectedTab == 4)
				g.DrawRectangle (Pens.Black, x, y, StaticVariables.tileSize * 0.6f - Pens.Black.Width, StaticVariables.tileSize * 0.6f - Pens.Black.Width);
		}

		/// <summary>
		/// Loads the entities used for placing in world
		/// </summary>
		private void loadEntities() {
			entityList = new List<Entity> ();
			entityList.Add (new Player(new PointF(0,0), 50));
            entityList.Add (new Enemy(new PointF(0, 0)));
            entityList.Add (new Key(new PointF (0, 0)));
            entityList.Add (new Door(new PointF(0, 0)));
            entityList.Add (new Finish (new PointF (0, 0)));

			entityList.Add (new Entity (ENTITIES.fruit, new PointF (0, 0), (int)SPRITES.banana));
			entityList.Add (new Trap(new PointF(0,0),(int)SPRITES.trap_opened));

			///Plants
			//Trees
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.tree1));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.tree2));
            entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.tree3));
            entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.sapling_acacia));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.sapling_jungle));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.sapling_oak));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.sapling_roofed_oak));

			//Flowers
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_rose, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_oxeye_daisy, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_houstonia, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_dandelion, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_blue_orchid, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_allium, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_tulip_orange, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_tulip_pink, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_tulip_red, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.flower_tulip_white, 50, false));

			//Misc plants
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.tallgrass, 50, false));
			entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.waterlily, 50, false));
            entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.shrub1, 50, false));
            entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.shrub2, 50, false));
            entityList.Add (new Plant (new PointF (0, 0), (int)SPRITES.bamboo, 50, false));

            //Objects
			entityList.Add (new Entity (ENTITIES.hut, new PointF (0, 0), (int)SPRITES.hut1, 50, false));
			entityList.Add (new Entity (ENTITIES.hut, new PointF (0, 0), (int)SPRITES.hut2, 50, false));
			entityList.Add (new Entity (ENTITIES.hut, new PointF (0, 0), (int)SPRITES.hut3, 50, false));
			entityList.Add (new Entity (ENTITIES.hut, new PointF (0, 0), (int)SPRITES.hut4, 50, false));
			entityList.Add (new Entity (ENTITIES.rock, new PointF (0, 0), (int)SPRITES.rock, 50, false));
			entityList.Add (new Entity (ENTITIES.rock, new PointF (0, 0), (int)SPRITES.mountain, 50, false));
			entityList.Add (new Entity (ENTITIES.def, new PointF (0, 0), (int)SPRITES.cage, 50, false));
			entityList.Add (new Entity (ENTITIES.wood, new PointF (0, 0), (int)SPRITES.logpile, 50, false));
        }
	}
}

