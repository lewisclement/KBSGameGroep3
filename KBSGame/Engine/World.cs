using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using KBSGame.Entities;
using System.IO;
using System.Windows.Forms;

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
		private Player player;
		private List<Enemy> enemies;

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
			enemies = new List<Enemy> ();
			loadTileTypes ();
		}

        /// <summary>
        /// Reload level
        /// </summary>
		public void reload()
		{
			loadLevel (currentLevelPath);
		}

		public void loadLevel(String fileName)
		{
			if (fileName != null) {
                if (currentLevelPath == null || currentLevelPath != fileName) {
                    fileName = Path.Combine(StaticVariables.levelFolder, fileName + ".xml");
                } else
                {
                    fileName = currentLevelPath;
                }
                
				objects = new List<Entity> ();
				terrainTiles = new List<TerrainTile> ();
				heightData = new List<Byte> ();
				enemies = new List<Enemy> ();

				Door.DoorCount = 0;
				Key.KeyCount = 0;

				LevelReader level = new LevelReader (fileName);
				this.objects = level.getObjects ();

				//Pre-sort entities by Y-position for improved performance when rendering
				for (int i = 0; i < objects.Count; i++) {
					Entity x = objects[i];                              
					while ((i - 1 >= 0) && (x.getLocation().Y < objects[i - 1].getLocation().Y)) 
					{
						objects[i] = objects[i - 1];
						i--;
					}
					objects[i] = x;
				}

				List<int> terrain = level.getTerrainTiles ();
				foreach (int id in terrain) {
					terrainTiles.Add (TileTypes [id]);
				}

				Size size = level.getSize ();
				width = size.Width;
				height = size.Height;

				List<Entity> enemyEntities = objects.FindAll (e => e.getType() == ENTITIES.enemy);
				foreach (Enemy enemy in enemyEntities) {
					enemies.Add ((Enemy)enemy);
				}

				if (StaticVariables.currentState == STATE.editor) {
					player = null;
					Entity focus = new Entity (ENTITIES.def, new PointF (size.Width / 2, size.Height / 2), 0);
					setFocusEntity (focus);
				} else {
					player = (Player)objects.FirstOrDefault (e => e.getType () == ENTITIES.player);
					if (player == null) {
						Random rand = new Random ();
						player = new Player (new PointF (rand.Next (0, width), rand.Next (0, height)), 50);
						objects.Add (player);
					}
					Console.WriteLine (player.getLocation ());
					setFocusEntity (player);
				}
				currentLevelPath = fileName;
			} else {
			    player = new Player(new PointF(120, 120), 50);
				FillWorld (TERRAIN.grass_normal, new Size(50, 50));
			}
            try {
                System.Media.SoundPlayer music = new System.Media.SoundPlayer(StaticVariables.musicFolder + ("/Icescape.wav"));
                music.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Fill the world with given terraintile-spriteID
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="size"></param>
		public void FillWorld(TERRAIN terrain, Size size)
		{
			this.width = Math.Max(StaticVariables.minWorldSize, Math.Min(size.Width, StaticVariables.maxWorldSize));
			this.height = Math.Max(StaticVariables.minWorldSize, Math.Min(size.Height, StaticVariables.maxWorldSize));
			terrainTiles = new List<TerrainTile> ();
			objects = new List<Entity> ();

			for (int i = 0; i < width * height; i++)
			{
				terrainTiles.Add(TileTypes[(int)terrain]);
			}
			player = null;
			Entity focus = new Entity (ENTITIES.def, new PointF (size.Width / 2, size.Height / 2), 0);
			setFocusEntity (focus);
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
		public void RemoveItem(int ID)
		{
			int i = 0;
			foreach (Entity entity in objects) {
				if (entity.getID () == ID) {
					objects.RemoveAt (i);
					return;
				}
				i++;
			}
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

        /// <summary>
        /// Gets all entities by given ENTITIES type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>All entities with given type</returns>
		public List<Entity> getEntitiesByType(ENTITIES type)
		{
			List<Entity> returnEntities = new List<Entity> ();
			foreach (Entity e in objects) {
				if (e.getType () == type)
					returnEntities.Add(e);
			}
			return returnEntities;
		}

        /// <summary>
        /// Gets all entities on given terraintile, optional nonsolid only objects
        /// </summary>
        /// <param name="point"></param>
        /// <param name="nonSolidOnly"></param>
        /// <returns>All entities on given terraintile</returns>
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

		public List<Entity> getEntitiesOnTerrainTileRelative(PointF relativeLocation, bool nonSolidOnly = false)
		{
			Rectangle view = getView ();
			PointF offset = getViewOffset ();

			PointF point = new PointF (view.Left + relativeLocation.X + offset.X, view.Top + relativeLocation.Y + offset.Y);

			Console.WriteLine (point);

			return getEntitiesOnTerrainTile (point, nonSolidOnly);
		}
        
        /// <summary>
        /// Checks for collision between an entity and a target point, optional check for solid only
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="solid"></param>
        /// <returns>True on collision, false on no collision</returns>
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

        /// <summary>
        /// Checks for a collision between 2 objects
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns>True on collision, false on no collision </returns>
		public bool checkCollision(Entity entity1, Entity entity2)
		{
			bool collision = false;
			PointF loc1 = entity1.getLocation ();
			PointF loc2 = entity2.getLocation ();
			if (loc1.X > loc2.X - entity2.getBoundingBox() && loc1.X < loc2.X + entity2.getBoundingBox() && loc1.Y > loc2.Y - entity2.getBoundingBox() && loc1.Y < loc2.Y + entity2.getBoundingBox())
				collision = true;

			return collision;
		}

        /// <summary>
        /// Gets given terraintile
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Terraintile on given location</returns>
		public TerrainTile getTerraintile(PointF point)
		{
			if (point.X * height + point.Y > terrainTiles.Count || point.X < 0 || point.Y < 0 || point.X > width-1 || point.Y > height-1)
				return null;
			return terrainTiles[(int) point.X*height + (int) point.Y];
		}

        /// <summary>
        /// Sets terraintile on given location with TERRAIN id
        /// </summary>
        /// <param name="point"></param>
        /// <param name="terrainID"></param>
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
        
        /// <summary>
        /// Gets the height of the terraintile
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Byte with terraintileheight</returns>
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

			for (int i = 0; i < objects.Count; i++) {
				PointF p = objects [i].getLocation ();
				if (p.X >= view.Left && p.X <= view.Right && p.Y >= view.Top && p.Y <= view.Bottom) {
					returnEntities.Add (objects [i]);
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

        /// <summary>
        /// Locks door with matching key
        /// </summary>
        /// <param name="key"></param>
        public void LockDoor(Key key)
        {
            foreach (Entity e in objects)
            {
                if(e is Door && key.getKeyid() == ((Door)e).getDoorid())
                { 
                    ((Door)e).LockDoor();
                    return;
                }
            }
        }

        /// <summary>
        /// Unlocks door with matching key
        /// </summary>
        /// <param name="key"></param>
        public void UnlockDoor(Key key)
	    {
            foreach (Entity e in objects)
	        {
	            if (e is Door && key.getKeyid() == ((Door) e).getDoorid())
	            {
                    ((Door)e).UnlockDoor();
	                return;
	            }
	        }
	    }

        /// <summary>
        /// Gets the location of the focussed entity
        /// </summary>
        /// <returns>PointF with location of focussed entity</returns>
	    public PointF getFocusCoordinates()
		{
			return focusEntity.getLocation();
		}

        /// <summary>
        /// Sets the focussed entity
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// Gets size of the grid
        /// </summary>
        /// <returns>Size of the grid</returns>
		public Size getSize()
		{
			return new Size (width, height);
		}

        /// <summary>
        /// Manually initialize player on given location
        /// </summary>
        /// <param name="location"></param>
	    public void InitPlayer(PointF location)
	    {
            player = new Player(location, 50);
        }

		public Enemy[] getEnemies()
		{
			return enemies.ToArray ();
		}

        /// <summary>
        /// Loads all tiletypes
        /// </summary>
	    private void loadTileTypes() 
		{
			TileTypes = new TerrainTile[(int)TERRAIN.count];
            TileTypes[(int)TERRAIN.water] = new TerrainTile((int)TERRAIN.water, (int)SPRITES.water, false);

            //Sandy types
            TileTypes [(int)TERRAIN.dirt] = new TerrainTile ((int)TERRAIN.dirt, (int)SPRITES.dirt);
            TileTypes [(int)TERRAIN.light_dirt] = new TerrainTile ((int)TERRAIN.light_dirt, (int)SPRITES.light_dirt);
            TileTypes [(int)TERRAIN.dark_dirt] = new TerrainTile ((int)TERRAIN.dark_dirt, (int)SPRITES.dark_dirt);
            TileTypes[(int)TERRAIN.clay] = new TerrainTile((int)TERRAIN.clay, (int)SPRITES.clay);
            TileTypes[(int)TERRAIN.red_sand] = new TerrainTile((int)TERRAIN.red_sand, (int)SPRITES.red_sand);
            TileTypes[(int)TERRAIN.sand] = new TerrainTile((int)TERRAIN.sand, (int)SPRITES.sand);
            TileTypes[(int)TERRAIN.sandstone] = new TerrainTile((int)TERRAIN.sandstone, (int)SPRITES.sandstone);

            //Grassy types
            TileTypes [(int)TERRAIN.grass_normal] = new TerrainTile ((int)TERRAIN.grass_normal, (int)SPRITES.grass_normal);
            TileTypes [(int)TERRAIN.grass_dark] = new TerrainTile ((int)TERRAIN.grass_dark, (int)SPRITES.grass_dark);
            TileTypes [(int)TERRAIN.grass_noisy] = new TerrainTile ((int)TERRAIN.grass_noisy, (int)SPRITES.grass_noisy);
            TileTypes [(int)TERRAIN.grass_path] = new TerrainTile ((int)TERRAIN.grass_path, (int)SPRITES.grass_path);

			//Rocky types
            TileTypes [(int)TERRAIN.stone_cracked_light] = new TerrainTile ((int)TERRAIN.stone_cracked_light, (int)SPRITES.stone_cracked_light);
            TileTypes [(int)TERRAIN.stone_cracked_dark] = new TerrainTile ((int)TERRAIN.stone_cracked_dark, (int)SPRITES.stone_cracked_dark);
            TileTypes [(int)TERRAIN.stone_mossy_light] = new TerrainTile ((int)TERRAIN.stone_mossy_light, (int)SPRITES.stone_mossy_light);
            TileTypes [(int)TERRAIN.stone_mossy_dark] = new TerrainTile ((int)TERRAIN.stone_mossy_dark, (int)SPRITES.stone_mossy_dark);
            TileTypes [(int)TERRAIN.stone] = new TerrainTile ((int)TERRAIN.stone, (int)SPRITES.stone);
            TileTypes [(int)TERRAIN.stone_wall] = new TerrainTile ((int)TERRAIN.stone_wall, (int)SPRITES.stone_wall);
            TileTypes [(int)TERRAIN.stonebrick] = new TerrainTile ((int)TERRAIN.stonebrick, (int)SPRITES.stonebrick);
			TileTypes [(int)TERRAIN.stone_diorite] = new TerrainTile ((int)TERRAIN.stone_diorite, (int)SPRITES.stone_diorite);
            TileTypes [(int)TERRAIN.lava] = new TerrainTile ((int)TERRAIN.lava, (int)SPRITES.lava, false);
            TileTypes [(int)TERRAIN.lava_stones] = new TerrainTile ((int)TERRAIN.lava_stones, (int)SPRITES.lava_stones);
			TileTypes [(int)TERRAIN.stone_granite] = new TerrainTile ((int)TERRAIN.stone_granite, (int)SPRITES.stone_granite);
		}

        /// <summary>
        /// Get all tiletypes
        /// </summary>
        /// <returns>All tiletypes</returns>
		public TerrainTile[] getTileTypes()
		{
			return TileTypes;
		}
	}
}
