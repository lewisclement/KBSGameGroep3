using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace KBSGame
{
	public class World
	{
		private TerrainTile[] TileTypes;
		private List<TerrainTile> terrainTiles;
		private List<Byte> heightData;

		private int width, height;
		private List<Entity> objects;
		private Entity focusEntity;
		private Player player; //TEMPORARY UNTIL MAINLOOP IS CREATED

		private String currentLevelPath = null;

		private World() { //For serializing
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="KBSGame.World"/> class.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public World (int width, int height)
		{
			this.width = Math.Max(StaticVariables.minWorldSize, Math.Min(width, StaticVariables.maxWorldSize));
			this.height = Math.Max(StaticVariables.minWorldSize, Math.Min(height, StaticVariables.maxWorldSize));

			objects = new List<Entity>();
			terrainTiles = new List<TerrainTile>();
			heightData = new List<Byte> ();

			// TEMPORARY
			//player = new Player(new Point(this.width / 2, this.height / 2), 50);
			//player.setHeight(50);
			//objects.Add(player);

			loadTileTypes ();

            //temporaryWorldGenerator ();

            //LevelWriter levelWriter = new LevelWriter ();
            //levelWriter.saveWorld (this, "testworld");
		}

	    public void AddItemsToInventory(Player player)
	    {
            player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
			player.AddItemToInventory(new Item(new Entity(ENTITIES.fruit, new PointF(0, 0), (int) SPRITES.banana, false, 50, 8, 0.6f)));
	    }

		public void reload()
		{
			loadLevel (currentLevelPath);
		}

		public void loadLevel(String fileName)
		{
			if (fileName != null) {
				fileName = StaticVariables.levelFolder + "/" + fileName + ".xml";
				objects = new List<Entity> ();
				terrainTiles = new List<TerrainTile> ();
				heightData = new List<Byte> ();

				LevelReader level = new LevelReader (fileName);
				this.objects = level.getObjects ();

				List<int> terrain = level.getTerrainTiles ();
				foreach (int id in terrain) {
					terrainTiles.Add (TileTypes [id]);
				}

				Size size = level.getSize ();
				width = size.Width;
				height = size.Height;

			    player = (Player) objects.FirstOrDefault(e => e.getType() == ENTITIES.player);
				if (player == null) {
					Random rand = new Random ();
					player = new Player (new PointF (rand.Next(0, width), rand.Next(0, height)), 50);
					objects.Add (player);
				}
                setFocusEntity (player);
				currentLevelPath = fileName;
			} else {
			    player = new Player(new PointF(120, 120), 50);
				FillWorld (TERRAIN.grass, new Size(50, 50));
			}
        }

		public void FillWorld(TERRAIN terrain, Size size)
		{
			width = size.Width;
			height = size.Height;

			terrainTiles = new List<TerrainTile> ();
			objects = new List<Entity> ();

			for (int i = 0; i < width * height; i++)
			{
				terrainTiles.Add(TileTypes[(int)terrain]);
			}
		}

		/// <summary>
		/// This is a temporary world generator for as long as there isn't a world loader
		/// </summary>
		private void temporaryWorldGenerator()
		{
			for (int i = 0; i < width * height; i++) 
			{
				terrainTiles.Add (TileTypes [(int)TERRAIN.grass]);
				heightData.Add (50);

			}

			Random rand = new Random ((int)DateTime.Now.Ticks);

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
						if (rand.Next (0, 5) == 0)
							objects.Add (new Plant(new PointF(x + 0.5f + (rand.Next(-3, 3) / 10.0f), y + 0.5f - (rand.Next(0, 3) / 10.0f)), (int)SPRITES.sapling1, 50, true));
						if(rand.Next(0, 50) == 0)
							objects.Add (new Entity(ENTITIES.fruit, new PointF(x + 0.5f, y + 0.5f), (int)SPRITES.banana, false, 50, 9, 0.6f));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.grass) {
						if(rand.Next(0, 100) == 0)
							objects.Add (new Plant(new PointF(x + 0.5f, y + 0.5f), (int)SPRITES.sapling2, 50, true));
						if(rand.Next(0, 100) == 0) {
							int amountbushes = rand.Next (4, 10);
							for (int i = 0; i < amountbushes; i++) {
								float X = rand.Next (0, 40) / 10.0f - 2.0f;
								float Y = rand.Next (0, 40) / 10.0f - 2.0f;
								TerrainTile tile = getTerraintile (new PointF (x + X, y + Y));
								if(tile != null && tile.IsWalkable)
									objects.Add (new Plant(new PointF(x + X, y + Y), (int)SPRITES.berrybush, 50, true, 10, 0.2f));
							}
						}
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.sand) {
						if(rand.Next(0, 50) == 0)
							objects.Add (new Plant(new PointF(x + 0.5f, y + 0.5f), (int)SPRITES.tallgrass, 50, false));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.water) {
						if(rand.Next(0, 60) == 0)
							objects.Add (new Plant(new PointF(x + 0.5f, y + 0.5f), (int)SPRITES.waterlily, 50, false, 0, 0.51f));
					}
				}
			}

			for (bool placed = false; !placed;) {
				Point place = new Point (rand.Next (0, width), rand.Next (0, height));
				if (getTerraintile (place).IsWalkable) {
					objects.Add (new Player(place, 50));
					placed = true;
				}
			}

			for (bool placed = false; !placed;) {
				Point place = new Point (rand.Next (0, width), rand.Next (0, height));
				if (getTerraintile (place).IsWalkable) {
					objects.Add (new Finish(place, (int)SPRITES.finish));
					placed = true;
				}
			}
		}

		/// <summary>
		/// Moves the object.
		/// </summary>
		/// <param name="entityID">Entity I.</param>
		/// <param name="relativeLocation">Relative location.</param>
		public void moveObject(int entityID, PointF relativeLocation)
		{

		}

		/// <summary>
		/// Gets the player.
		/// </summary>
		/// <returns>The player.</returns>
		public Player getPlayer()
		{
			return (Player) objects.FirstOrDefault (e => e.getType () == ENTITIES.player);
		}

		/// <summary>
		/// Removes the item.
		/// </summary>
		/// <param name="e">E.</param>
		public void RemoveItem(Entity e)
		{
			objects.Remove(e);
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="entityID">Entity I.</param>
		public Entity getEntity(int entityID)
		{
			return objects.FirstOrDefault(obj => obj.getID() == entityID);
		}

		/// <summary>
		/// Gets the focus entity.
		/// </summary>
		/// <returns>The focus entity.</returns>
		public Entity getFocusEntity()
		{
			return focusEntity;
		}

		/// <summary>
		/// Adds the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void addEntity(Entity entity)
		{
			objects.Add (entity);
		}

		/// <summary>
		/// Adds the entity relative to view position.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <param name="relativeLocation">Relative location.</param>
		public void addEntityRelative(Entity entity, PointF relativeLocation)
		{
			Rectangle view = getView ();
			PointF offset = getViewOffset ();

			entity.setLocation (new PointF(view.Left + relativeLocation.X + offset.X, view.Top + relativeLocation.Y + offset.Y));

			objects.Add (entity);
		}

		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <returns>The entities.</returns>
		public List<Entity> getEntities()
		{
			return objects;
		} 
			
		/// <summary>
		/// Gets the terrain.
		/// </summary>
		/// <returns>The entities.</returns>
		public List<TerrainTile> getTerrain()
		{
			return terrainTiles;
		} 

		/// <summary>
		/// Gets the entities on terrain tile based on position.
		/// </summary>
		/// <returns>The entities on terrain tile.</returns>
		/// <param name="point">Point.</param>
		public Entity getEntityById(int ID)
		{
			Entity returnEntity = null;

			foreach (Entity e in objects) {
				if (e.getID () == ID) {
					returnEntity = e;
					break;
				}
			}

			return returnEntity;
		}

		public List<Entity> getEntitiesByType(ENTITIES type)
		{
			List<Entity> returnEntities = new List<Entity> ();

			foreach (Entity e in objects) {
				if (e.getType () == type)
					returnEntities.Add(e);
			}

			return returnEntities;
		}

		public List<Entity> getEntitiesOnTerrainTile(PointF point, bool nonSolidOnly = false)
		{
			List<Entity> returnObjects = new List<Entity> ();
		    
            // If nonSolidOnly, use the nonSolidObjects list, else go through all objects
			foreach (Entity e in objects)
		    {
                if (nonSolidOnly && e.getSolid())
                    continue;
                PointF loc = e.getLocation();
		        if (point.X > loc.X - e.getBoundingBox() && point.X < loc.X + e.getBoundingBox() &&
		            point.Y > loc.Y - e.getBoundingBox() && point.Y < loc.Y + e.getBoundingBox())
				{
				returnObjects.Add(e);
		        }
		    }
			return returnObjects;
		}

		public bool checkCollision(Entity entity, PointF target, bool solid = true)
		{
			bool collision = false;

			foreach (Entity e in objects) {
				if (e == entity)
					continue;

				if (!e.getSolid () == solid)
					continue;

				PointF loc = e.getLocation ();
				if (target.X > loc.X - e.getBoundingBox() && target.X < loc.X + e.getBoundingBox() && 
                    target.Y > loc.Y - e.getBoundingBox() && target.Y < loc.Y + e.getBoundingBox()) {
					collision = true;
					break;
				}
			}

			return collision;
		}

		public bool checkCollision(Entity entity1, Entity entity2)
		{
			bool collision = false;

			PointF loc1 = entity1.getLocation ();
			PointF loc2 = entity2.getLocation ();
			if (loc1.X > loc2.X - entity2.getBoundingBox() && loc1.X < loc2.X + entity2.getBoundingBox() && loc1.Y > loc2.Y - entity2.getBoundingBox() && loc1.Y < loc2.Y + entity2.getBoundingBox())
				collision = true;

			return collision;
		}

		public TerrainTile getTerraintile(PointF point)
		{
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return null;

			return terrainTiles[(int) point.X*height + (int) point.Y];
		}

		public void setTerraintile(PointF point, int terrainID)
		{
			TerrainTile tile = TileTypes[terrainID];
			terrainTiles[(int) point.X*height + (int) point.Y] = tile;
		}

		public void setTerrainTileRelative(Point point, int terrainID)
		{
			Rectangle view = getView ();
			PointF offset = getViewOffset ();

			float addX = (point.X - (point.X + StaticVariables.tileSize * offset.X) % StaticVariables.tileSize) / StaticVariables.tileSize;
			float addY = (point.Y - (point.Y + StaticVariables.tileSize * offset.Y) % StaticVariables.tileSize) / StaticVariables.tileSize;

			int x = view.X + (int)Math.Ceiling(addX);
			int y = view.Y + (int)Math.Ceiling(addY);

			terrainTiles [x * height + y] = TileTypes [terrainID];
		}

		public Byte getTerrainHeight(PointF point)
		{
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return 0;

			return heightData[(int) point.X*height + (int) point.Y];
		}

		//Stub
		public TerrainTile[] getTilesView()
		{
			if (StaticVariables.viewWidth > width)
				StaticVariables.viewWidth = width;
			if (StaticVariables.viewHeight > height)
				StaticVariables.viewHeight = height;

			TerrainTile[] returnTiles = new TerrainTile[StaticVariables.viewWidth * StaticVariables.viewHeight];

			//Get viewport
			Rectangle view = getView ();

			int index = 0;
			for (int i = view.Left; i < view.Right; i++) {
				for (int j = view.Top; j < view.Bottom; j++) {
					returnTiles [index++] = terrainTiles[i * height + j];
				}
			}

			return returnTiles;
		}

		public Entity[] getEntitiesView()
		{
			List<Entity> returnEntities = new List<Entity> ();

			//Get viewport
			Rectangle view = getView ();

			/*int[] drawOrderIndex = new int[StaticVariables.drawOrderSize];
			for (int i = 0; i < drawOrderIndex.Length; i++) {
				drawOrderIndex[i] = 0;
			}*/

			for (int i = 0; i < objects.Count; i++) {
				PointF p = objects [i].getLocation ();
				if (p.X >= view.Left && p.X <= view.Right && p.Y >= view.Top && p.Y <= view.Bottom) {
					returnEntities.Add (objects [i]);

					//for (int j = objects [i].getDrawOrder () + 1; j < StaticVariables.drawOrderSize; j++)
					//	drawOrderIndex [j]++;
				}
			}

			for (int i = 0; i < returnEntities.Count; i++) {
				Entity x = returnEntities[i];                              
				while ((i - 1 >= 0) && (x.getLocation().Y + x.getDrawOrder() * StaticVariables.viewHeight < returnEntities[i - 1].getLocation().Y + returnEntities[i - 1].getDrawOrder() * StaticVariables.viewHeight)) 
				{                                          
					returnEntities[i] = returnEntities[i - 1];                 
					i--;
				}
				returnEntities[i] = x;                             
			}

			return returnEntities.ToArray();
		}

		public PointF getFocusCoordinates()
		{
			return focusEntity.getLocation();
		}

		public void setFocusEntity(Entity entity)
		{
			focusEntity = entity;
		}

		public Rectangle getView()
		{
			if (StaticVariables.viewWidth > width)
				StaticVariables.viewWidth = width;
			if (StaticVariables.viewHeight > height)
				StaticVariables.viewHeight = height;

			//Get viewport
			int startX = (int) focusEntity.getLocation ().X - (int)StaticVariables.viewWidth / 2;
			int startY = (int) focusEntity.getLocation ().Y - (int)StaticVariables.viewHeight / 2;
			int endX = startX + StaticVariables.viewWidth;
			int endY = startY + StaticVariables.viewHeight;

			if (startX < 0) {
				startX = 0;
				endX = StaticVariables.viewWidth;
			}
			if (startY < 0) {
				startY = 0;
				endY = StaticVariables.viewHeight;
			}
			if (endX > width - 1) {
				endX = width;
				startX = width - StaticVariables.viewWidth;
			}
			if (endY > height - 1) {
				endY = height;
				startY = height - StaticVariables.viewHeight;
			}

			return new Rectangle (startX, startY, StaticVariables.viewWidth, StaticVariables.viewHeight);
		}

		public PointF getViewOffset()
		{
			float xCenter = focusEntity.getLocation().X;
			if (xCenter <= width - StaticVariables.viewWidth / 2 && xCenter > StaticVariables.viewWidth / 2)
				xCenter -= (float)Math.Floor (xCenter);
			else
				xCenter = 0;

			float yCenter = focusEntity.getLocation().Y;
			if (yCenter <= height - StaticVariables.viewHeight / 2 && yCenter > StaticVariables.viewHeight / 2)
				yCenter -= (float)Math.Floor (yCenter);
			else
				yCenter = 0;

			return new PointF (xCenter, yCenter);
		}

		public Size getSize()
		{
			return new Size (width, height);
		}

		private void loadTileTypes() 
		{
			TileTypes = new TerrainTile[(int)TERRAIN.count];

			TileTypes [(int)TERRAIN.grass] = new TerrainTile ((int)TERRAIN.grass, (int)SPRITES.grass);
			TileTypes [(int)TERRAIN.water] = new TerrainTile ((int)TERRAIN.water, (int)SPRITES.water, false);
			TileTypes [(int)TERRAIN.planks_birch] = new TerrainTile ((int)TERRAIN.planks_birch, (int)SPRITES.planks_birch);

			//Sandy types
			TileTypes [(int)TERRAIN.sand] = new TerrainTile ((int)TERRAIN.sand, (int)SPRITES.sand);
			TileTypes [(int)TERRAIN.red_sand] = new TerrainTile ((int)TERRAIN.red_sand, (int)SPRITES.red_sand);
			TileTypes [(int)TERRAIN.clay] = new TerrainTile ((int)TERRAIN.clay, (int)SPRITES.clay);
			TileTypes [(int)TERRAIN.sandstone] = new TerrainTile ((int)TERRAIN.sandstone, (int)SPRITES.sandstone);
			TileTypes [(int)TERRAIN.dirt] = new TerrainTile ((int)TERRAIN.dirt, (int)SPRITES.dirt);
			TileTypes [(int)TERRAIN.farmland] = new TerrainTile ((int)TERRAIN.farmland, (int)SPRITES.farmland);

			//Rocky types
			TileTypes [(int)TERRAIN.stone] = new TerrainTile ((int)TERRAIN.stone, (int)SPRITES.stone);
			TileTypes [(int)TERRAIN.stone_diorite] = new TerrainTile ((int)TERRAIN.stone_diorite, (int)SPRITES.stone_diorite);
			TileTypes [(int)TERRAIN.stone_granite] = new TerrainTile ((int)TERRAIN.stone_granite, (int)SPRITES.stone_granite);
			TileTypes [(int)TERRAIN.stonebrick] = new TerrainTile ((int)TERRAIN.stonebrick, (int)SPRITES.stonebrick);
			TileTypes [(int)TERRAIN.brick] = new TerrainTile ((int)TERRAIN.brick, (int)SPRITES.brick);
		}

		public TerrainTile[] getTileTypes()
		{
			return TileTypes;
		}
	}
}
