using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace KBSGame
{
	public class World
	{
		static int minS = StaticVariables.minWorldSize;
		static int maxS = StaticVariables.maxWorldSize;

		private TerrainTile[] TileTypes;
		private List<TerrainTile> terrainTiles;
		private List<Byte> heightData;

		private int width, height;
		private List<Entity> objects;
		private Entity focusEntity;
	    private Player player; //TEMPORARY UNTIL MAINLOOP IS CREATED

		public World (int width, int height)
		{
			this.width = Math.Max(minS, Math.Min(width, maxS));
			this.height = Math.Max(minS, Math.Min(height, maxS));

            objects = new List<Entity>();
            terrainTiles = new List<TerrainTile>();
			heightData = new List<Byte> ();

            // TEMPORARY
			player = new Player(new Point(this.width/2, this.height/2), 50);
			player.setHeight (50);
		    objects.Add(player);


            TileTypes = new TerrainTile[(int)TERRAIN.count];
			TileTypes [(int)TERRAIN.grass] = new TerrainTile ((int)TERRAIN.grass);
			TileTypes [(int)TERRAIN.grass].setSpriteID ((int)SPRITES.grass);
			TileTypes [(int)TERRAIN.water] = new TerrainTile ((int)TERRAIN.water, false);
			TileTypes [(int)TERRAIN.water].setSpriteID ((int)SPRITES.water);
			TileTypes [(int)TERRAIN.sand] = new TerrainTile ((int)TERRAIN.sand);
			TileTypes [(int)TERRAIN.sand].setSpriteID ((int)SPRITES.sand);
			TileTypes [(int)TERRAIN.dirt] = new TerrainTile ((int)TERRAIN.dirt);
			TileTypes [(int)TERRAIN.dirt].setSpriteID ((int)SPRITES.dirt);


			temporaryWorldGenerator ();
            
			setFocusEntity (objects [0]); // TEMPORARY PLAYER
		}

		private void temporaryWorldGenerator()
		{
			//Fill world with water
			for (int i = 0; i < width * height; i++) 
			{
				terrainTiles.Add (TileTypes [(int)TERRAIN.grass]);
				heightData.Add (50);
			}

			//Place 10 rectangles
			Random rand = new Random ((int)DateTime.Now.Ticks);
			/*
			for (int i = 0; i < width * 4; i++) {
				int size = rand.Next (3, 5);
				int xTop = rand.Next (0, width - size);
				int yTop = rand.Next (0, height - size);

				for (int x = xTop; x < xTop + size; x++) {
					for (int y = yTop; y < yTop + size; y++) {
						int index = x * height + y;
						terrainTiles [index] = TileTypes [(int)TERRAIN.sand];
					}
				}
			}*/


			/*
			for (int i = 0; i < width * 5; i++) {
				int size = rand.Next (2, 4);
				int xTop = rand.Next (0, width - size);
				int yTop = rand.Next (0, height - size);

				for (int x = xTop; x < xTop + size; x++) {
					for (int y = yTop; y < yTop + size; y++) {
						int index = x * height + y;
						terrainTiles [index] = TileTypes [(int)TERRAIN.dirt];
					}
				}
			}*/

			//Amount of dirt patches
			for (int i = 0; i < width*height / 500; i++) {
				int size = rand.Next (2, 4);
				Point p = new Point (rand.Next (0, width), rand.Next (0, height));

				for (int j = 0; j < 50; j++) {
					int xOffset = rand.Next (-10, 10), yOffset = rand.Next (-10, 10);
					if (p.X + xOffset < (int)Math.Ceiling((double)size / 2) || p.X + xOffset >= width - (int)Math.Ceiling((double)size / 2) ||
						p.Y + yOffset < (int)Math.Ceiling((double)size / 2) || p.Y + yOffset >= height - (int)Math.Ceiling((double)size / 2))
						continue;

					for (int x = 0; x < size; x++) {
						for (int y = 0; y < size; y++) {
							terrainTiles [(p.X + xOffset + x) * height + p.Y + y + yOffset] = TileTypes [(int)TERRAIN.dirt];
							heightData[(p.X + xOffset + x) * height + p.Y + y + yOffset] = 52;
						}
					}
				}
			}

			//Amount of lakes
			for (int i = 0; i < width*height / 500; i++) {
				int size = rand.Next (2, 4);
				Point p = new Point (rand.Next (0, width), rand.Next (0, height));

				for (int j = 0; j < 10; j++) {
					int xOffset = rand.Next (-5, 5), yOffset = rand.Next (-5, 5);
					if (p.X + xOffset < (int)Math.Ceiling((double)size / 2) || p.X + xOffset >= width - (int)Math.Ceiling((double)size / 2) ||
						p.Y + yOffset < (int)Math.Ceiling((double)size / 2) || p.Y + yOffset >= height - (int)Math.Ceiling((double)size / 2))
						continue;

					for (int x = 0; x < size; x++) {
						for (int y = 0; y < size; y++) {
							terrainTiles [(p.X + xOffset + x) * height + p.Y + y + yOffset] = TileTypes [(int)TERRAIN.water];
						}
					}
				}
			}

			//Amount of rivers
			for (int i = 0; i < Math.Ceiling((double)width*height/30000); i++) 
			{
				int size = rand.Next (2, 4);
				int x = 0;
				int y = rand.Next (0, height - size);

				bool reached = false;
				Point relative = new Point (1, 0);
				while (!reached) {
					relative.Y += rand.Next (0, 3) - 1;
					if (relative.Y > 2)
						relative.Y = 2;
					if (relative.Y < -2)
						relative.Y = -2;
					y += relative.Y;

					if (y < 0 || y == height)
						break;

					for (int X = 0; X < size; X++) {
						for (int Y = 0; Y < size; Y++) {
							if((x + X) * height + y + Y < terrainTiles.Count)
								terrainTiles [(x + X) * height + y + Y] = TileTypes [(int)TERRAIN.water];
						}
					}

					x++;
					if (x == width)
						reached = true;
				}
			}

			for (int x = 1; x < width-1; x++) 
			{
				for (int y = 1; y < height-1; y++) {
					int tileIndex = x * height + y;
					if (terrainTiles[tileIndex].getID() == (int)TERRAIN.water) {
						if (terrainTiles [tileIndex - 1].IsWalkable) {
							terrainTiles [tileIndex - 1] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x-1) * height + y].IsWalkable) {
							terrainTiles [(x-1) * height + y] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [tileIndex + 1].IsWalkable) {
							terrainTiles [tileIndex + 1] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x+1) * height + y].IsWalkable) {
							terrainTiles [(x+1) * height + y] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x-1) * height + y - 1].IsWalkable) {
							terrainTiles [(x-1) * height + y - 1] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x-1) * height + y + 1].IsWalkable) {
							terrainTiles [(x-1) * height + y + 1] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x+1) * height + y - 1].IsWalkable) {
							terrainTiles [(x+1) * height + y - 1] = TileTypes [(int)TERRAIN.sand];
						}
						if (terrainTiles [(x+1) * height + y + 1].IsWalkable) {
							terrainTiles [(x+1) * height + y + 1] = TileTypes [(int)TERRAIN.sand];
						}
					}
				}
			}

			for(int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.dirt) {
						if(rand.Next(0, 5) == 0)
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.sapling1, 50, true));
						if(rand.Next(0, 50) == 0)
							objects.Add (new Entity(new Point(x, y), (int)SPRITES.banana, false, 50, 9));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.grass) {
						if(rand.Next(0, 100) == 0)
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.sapling2, 50, true));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.sand) {
						if(rand.Next(0, 50) == 0)
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.tallgrass, 50, false));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.water) {
						if(rand.Next(0, 60) == 0)
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.waterlily, 50, false, 0, 0));
					}
				}
			}
            // Test Key
            objects.Add(new Key(new Point(155, 150), (int)SPRITES.key));
            objects.Add(new Key(new Point(155, 150), (int)SPRITES.key));
            objects.Add(new Key(new Point(156, 150), (int)SPRITES.key, false, false));
            //objects.Add(new Plant(new Point(155, 150), (int)SPRITES.sapling1, 50, true));
        }


		public void moveObject(int entityID, Point relativeLocation)
		{

		}

	    public Player getPlayer()
	    {
	        return player;
	    }

	    public void RemoveItem(Entity e)
	    {
	        objects.Remove(e);
	    }

	    public Entity getEntity(int entityID)
	    {
	        return objects.FirstOrDefault(obj => obj.getID() == entityID);
	    }

		public List<Entity> getEntities()
		{
			return objects;
		} 

		public List<Entity> getEntitiesOnTerrainTile(Point point)
		{
            // Return list with entities on given tile
		    return objects.Where(e => e.getLocation() == point).ToList();
		}

	    public List<Item> getItemsOnTerrainTile(Point point)
	    {
	        // Return list with items on given tile
	        return objects.Where(e => e.getLocation() == point).Where(e => e is Item).Cast<Item>().ToList();
	    }

	    public TerrainTile getTerraintile(Point point)
	    {
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return null;

	        return terrainTiles[point.X*height + point.Y];
	    }

		public Byte getTerrainHeight(Point point)
		{
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return 0;

			return heightData[point.X*height + point.Y];
		}

	    //Stub
		public TerrainTile[] getTilesView(int viewWidth, int viewHeight)
		{
			if (viewWidth > width)
				viewWidth = width;
			if (viewHeight > height)
				viewHeight = height;

			TerrainTile[] returnTiles = new TerrainTile[viewWidth * viewHeight];

			//Get viewport
			Rectangle view = getView (viewWidth, viewHeight);

			int index = 0;
			for (int i = view.Left; i < view.Right; i++) {
				for (int j = view.Top; j < view.Bottom; j++) {
					returnTiles [index++] = terrainTiles[i * height + j];
				}
			}

			return returnTiles;
		}

		public Entity[] getEntitiesView(int viewWidth, int viewHeight)
		{
			List<Entity> returnEntities = new List<Entity> ();

			//Get viewport
			Rectangle view = getView (viewWidth, viewHeight);

			int[] drawOrderIndex = new int[StaticVariables.drawOrderSize];
			for (int i = 0; i < drawOrderIndex.Length; i++) {
				drawOrderIndex[i] = 0;
			}

			for (int i = 0; i < objects.Count; i++) {
				Point p = objects [i].getLocation ();
				if (p.X >= view.Left && p.X <= view.Right && p.Y >= view.Top && p.Y <= view.Bottom) {
					returnEntities.Insert (drawOrderIndex[objects[i].getDrawOrder()], objects [i]);

					for (int j = objects [i].getDrawOrder () + 1; j < StaticVariables.drawOrderSize; j++)
						drawOrderIndex [j]++;
				}
			}

			return returnEntities.ToArray();
		}

		public Point getFocusCoordinates()
		{
			return focusEntity.getLocation();
		}

		public void setFocusEntity(Entity entity)
		{
			focusEntity = entity;
		}

		public Rectangle getView(int viewWidth, int viewHeight)
		{
			if (viewWidth > width)
				viewWidth = width;
			if (viewHeight > height)
				viewHeight = height;

			//Get viewport
			int startX = focusEntity.getLocation ().X - (int)viewWidth / 2;
			int startY = focusEntity.getLocation ().Y - (int)viewHeight / 2;
			int endX = startX + viewWidth;
			int endY = startY + viewHeight;

			if (startX < 0) {
				startX = 0;
				endX = viewWidth;
			}
			if (startY < 0) {
				startY = 0;
				endY = viewHeight;
			}
			if (endX > width - 1) {
				endX = width;
				startX = width - viewWidth;
			}
			if (endY > height - 1) {
				endY = height;
				startY = height - viewHeight;
			}
			
			return new Rectangle (startX, startY, viewWidth, viewHeight);
		}


		//Stub for later
		public bool resize(int x, int y) //returns true on succes, false on fail
		{
			bool allowedSize = false;

			if (width + x >= minS && width + x <= maxS) {
				allowedSize = true;
			} else
				allowedSize = false;

			if (height + y >= minS && height + y <= maxS) {
				allowedSize = true;
			} else
				allowedSize = false;

			if (!allowedSize)
				return false;

			if (width + x < width) { //Shrink

			} else if (width + x == width) { //No change

			} else { //Grow

			}

			if (height + y < height) { //Shrink

			} else if (height + x == height) { //No change

			} else { //Grow

			}

			return false;
		}


	}
}

