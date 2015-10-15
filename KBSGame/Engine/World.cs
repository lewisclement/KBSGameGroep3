﻿using System;
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

			TileTypes = new TerrainTile[(int)TERRAIN.count];
			TileTypes [(int)TERRAIN.grass] = new TerrainTile ((int)TERRAIN.grass);
			TileTypes [(int)TERRAIN.grass].setSpriteID ((int)SPRITES.grass);
			TileTypes [(int)TERRAIN.water] = new TerrainTile ((int)TERRAIN.water, false);
			TileTypes [(int)TERRAIN.water].setSpriteID ((int)SPRITES.water);
			TileTypes [(int)TERRAIN.sand] = new TerrainTile ((int)TERRAIN.sand);
			TileTypes [(int)TERRAIN.sand].setSpriteID ((int)SPRITES.sand);
			TileTypes [(int)TERRAIN.dirt] = new TerrainTile ((int)TERRAIN.dirt);
			TileTypes [(int)TERRAIN.dirt].setSpriteID ((int)SPRITES.dirt);

			currentLevelPath = StaticVariables.execFolder + "/tiles.xml";
			loadLevel();
			//temporaryWorldGenerator ();

			//LevelWriter levelWriter = new LevelWriter ();
			//levelWriter.saveWorld (this);

			setFocusEntity (objects.FirstOrDefault(e => e.getType() == ENTITIES.player));
		}

		public void loadLevel()
		{
			objects = new List<Entity>();
			terrainTiles = new List<TerrainTile>();
			heightData = new List<Byte> ();

			LevelReader level = new LevelReader(currentLevelPath);
			this.objects = level.getObjects();

			List<int> terrain = level.getTerrainTiles();
			foreach (int id in terrain)
			{
				terrainTiles.Add(TileTypes[id]);
			}

			setFocusEntity (objects [0]); // TEMPORARY PLAYER
		}

		public void AddItems()
		{
			player.AddItemToInventory(new Item(new Entity(new PointF(0.0f, 0.0f), (int)SPRITES.banana)));
			player.AddItemToInventory(new Item(new Entity(new PointF(0.0f, 0.0f), (int)SPRITES.banana)));
		}

		private void FillWorld(int Sprite)
		{
			for (int i = 0; i < width * height; i++)
			{
				terrainTiles.Add(TileTypes[Sprite]);
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
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.tallgrass, 50, false, 11));
					}

					if (terrainTiles [x * height + y].getID () == (int)TERRAIN.water) {
						if(rand.Next(0, 60) == 0)
							objects.Add (new Plant(new Point(x, y), (int)SPRITES.waterlily, 50, false, 0, 0));
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
					objects.Add (new Finish(place, 50));
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
			return player;
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

		public List<Item> getItemsOnTerrainTile(PointF point)
		{
			// Return list with items on given tile
			return objects.Where(e => e.getLocation() == point).Where(e => e is Item).Cast<Item>().ToList();
		}

		public bool checkCollision(Entity entity, PointF target)
		{
			bool collision = false;

			foreach (Entity e in objects) {
				if (e == entity)
					continue;

				if (!e.getSolid ())
					continue;

				PointF loc = e.getLocation ();
				if (target.X > loc.X - 1.0f && target.X < loc.X + 1.0f && target.Y > loc.Y - 1.0f && target.Y < loc.Y + 1.0f) {
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
			if (loc1.X > loc2.X - 1.0f && loc1.X < loc2.X + 1.0f && loc1.Y > loc2.Y - 1.0f && loc1.Y < loc2.Y + 1.0f)
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
			if (terrainID == (int)SPRITES.water)
			{
				tile.IsWalkable = false;
			}
			terrainTiles[(int) point.X*height + (int) point.Y] = tile;
		}

		public Byte getTerrainHeight(PointF point)
		{
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return 0;

			return heightData[(int) point.X*height + (int) point.Y];
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
				PointF p = objects [i].getLocation ();
				if (p.X >= view.Left && p.X <= view.Right && p.Y >= view.Top && p.Y <= view.Bottom) {
					returnEntities.Insert (drawOrderIndex[objects[i].getDrawOrder()], objects [i]);

					for (int j = objects [i].getDrawOrder () + 1; j < StaticVariables.drawOrderSize; j++)
						drawOrderIndex [j]++;
				}
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

		public Rectangle getView(int viewWidth, int viewHeight)
		{
			if (viewWidth > width)
				viewWidth = width;
			if (viewHeight > height)
				viewHeight = height;

			//Get viewport
			int startX = (int) focusEntity.getLocation ().X - (int)viewWidth / 2;
			int startY = (int) focusEntity.getLocation ().Y - (int)viewHeight / 2;
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

			if (width + x >= StaticVariables.minWorldSize && width + x <= StaticVariables.maxWorldSize) {
				allowedSize = true;
			} else
				allowedSize = false;

			if (height + y >= StaticVariables.minWorldSize && height + y <= StaticVariables.maxWorldSize) {
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
