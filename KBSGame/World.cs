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
		private int width, height;
		private List<Entity> objects;
		private Entity focusEntity;
	    private Player player; //TEMPORARY UNTIL MAINLOOP IS CREATED

		private int entityCount = 0;

		public World (int width, int height)
		{
            objects = new List<Entity>();
            terrainTiles = new List<TerrainTile>();

            // TEMPORARY
            player = new Player(entityCount, new Point(width/2, height/2));
		    objects.Add(player);
		    entityCount++;


            TileTypes = new TerrainTile[(int)TERRAIN.count];
			TileTypes [(int)TERRAIN.grass] = new TerrainTile ((int)TERRAIN.grass);
			TileTypes [(int)TERRAIN.grass].setSpriteID ((int)SPRITES.grass);
			TileTypes [(int)TERRAIN.water] = new TerrainTile ((int)TERRAIN.water, false);
			TileTypes [(int)TERRAIN.water].setSpriteID ((int)SPRITES.water);
			TileTypes [(int)TERRAIN.sand] = new TerrainTile ((int)TERRAIN.sand);
			TileTypes [(int)TERRAIN.sand].setSpriteID ((int)SPRITES.sand);

			this.width = Math.Max(minS, Math.Min(width, maxS));
			this.height = Math.Max(minS, Math.Min(height, maxS));

			temporaryWorldGenerator ();
            
			setFocusEntity (objects [0]); // TEMPORARY PLAYER
		}

		private void temporaryWorldGenerator()
		{
			//Fill world with water
			for (int i = 0; i < width * height; i++) {
				terrainTiles.Add (TileTypes [(int)TERRAIN.grass]);
			}

			//Place 10 rectangles
			Random rand = new Random ((int)DateTime.Now.Ticks);
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
			}

			for (int i = 0; i < width * 10; i++) {
				int size = rand.Next (2, 4);
				int xTop = rand.Next (0, width - size);
				int yTop = rand.Next (0, height - size);

				for (int x = xTop; x < xTop + size; x++) {
					for (int y = yTop; y < yTop + size; y++) {
						int index = x * height + y;
						terrainTiles [index] = TileTypes [(int)TERRAIN.water];
					}
				}
			}
		}

		public void moveObject(int entityID, Point relativeLocation)
		{

		}

	    public Entity getEntity(int entityID)
	    {
	        return objects.FirstOrDefault(obj => obj.getID() == entityID);
	    }

	    public TerrainTile getTerraintile(Point point)
	    {
	        return terrainTiles[point.X*height + point.Y];
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
			int startX = focusEntity.getLocation ().X - (int)viewWidth / 2;
			int startY = focusEntity.getLocation ().Y - (int)viewHeight / 2;
			int endX = startX + viewWidth;
			int endY = startY + viewHeight;

			int index = 0;
			for (int i = startX; i < endX; i++) {
				for (int j = startY; j < endY; j++) {
					returnTiles [index++] = terrainTiles[i * height + j];
				}
			}

			return returnTiles;
		}

		public Entity[] getEntitiesView(int viewWidth, int viewHeight)
		{
			if (viewWidth > width)
				viewWidth = width;
			if (viewHeight > height)
				viewHeight = height;

			List<Entity> returnEntities = new List<Entity> ();

			//Get viewport
			int startX = focusEntity.getLocation ().X - (int)viewWidth / 2;
			int startY = focusEntity.getLocation ().Y - (int)viewHeight / 2;
			int endX = startX + viewWidth;
			int endY = startY + viewHeight;

			for (int i = 0; i < objects.Count; i++) {
				Point p = objects [i].getLocation ();
				if (p.X >= startX && p.X <= endX && p.Y >= startY && p.Y <= endY) {
					returnEntities.Add (objects [i]);
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

