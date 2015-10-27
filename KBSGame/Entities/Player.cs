 using System;
using System.Collections.Generic;
 using System.Diagnostics;
 using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
 using KBSGame.Entities;

namespace KBSGame
{
	public class Player : Entity
	{
		public List<Item> Inventory { get; private set; }
	    public enum Direction { Up = 0, Down, Left, Right };

	    public int CurrentDirection;

	    public Player(PointF location, Byte height)
			: base(ENTITIES.player, location, (int)SPRITES.player, true, height, 10)
		{
			Inventory = new List<Item>();
	        CurrentDirection = (int) Direction.Down;
		}

		public override void move(World sender, PointF relativeLocation)
		{
			float moveLocationX = location.X + relativeLocation.X;
			float moveLocationY = location.Y + relativeLocation.Y;

			PointF targetPoint = new PointF(moveLocationX, moveLocationY);
			TerrainTile targetTile = sender.getTerraintile (targetPoint);
		    //bool HasNonSolidEntities = sender.getEntitiesOnTerrainTile((targetPoint)).Any(e => e.getSolid() == false);

			// Log current inventory
			//Inventory.ForEach(Console.WriteLine);

			// If terrain contains solid objects OR if tile has no walkable entity on a non-walkable tile
			if (targetTile == null || sender.checkCollision(this, targetPoint) || !targetTile.IsWalkable && !sender.checkCollision(this, targetPoint, false))
				return;

			location.X = (float)Math.Round(moveLocationX, 1);
			location.Y = (float)Math.Round(moveLocationY, 1);
            //PickUpItems(sender);

            //Check if the player is moving on a entity and call oncolission of that entity
            List<Entity> entities = sender.getEntities();

            for(int i = 0; i < entities.Count; i++)
            {
                if(sender.checkCollision(this, entities[i]) && entities[i].getType() != ENTITIES.player)
                {
                    entities[i].onCollision();
                }
            }            
        }

		public void AddItemToInventory(Item i)
		{
			Inventory.Add(i);
		}

	    public void PickupItems(World w)
	    {
	        if (Inventory.Count >= 10) return;
            // Get a list with nonSolid entities on terrain tile and filter for bananas or keys
	        List<Entity> EntitiesOnTile =
	            w.getEntitiesOnTerrainTile(getLocation(), true)
	                .Where(e => e.getSpriteID() == (int) SPRITES.banana || e.getSpriteID() == (int) SPRITES.key)
	                .ToList();
            // If there's nothing in the list, return;
	        if (EntitiesOnTile.Count == 0) return;
            // Add first item of list to inventory
	        Inventory.Add(new Item(EntitiesOnTile[0]));
            // If key is picked up, unlock door
	        if (EntitiesOnTile[0] is Key)
	        {
                w.UnlockDoor((Key)EntitiesOnTile[0]);
	        }
	        

	        // Remove item from world
	        w.getEntities().Remove(EntitiesOnTile[0]);
        }


	    // Removes first item from inventory if exists
        public void DropItem(World world)
        {
            PointF dropLocation = GetLocationInFrontOfPlayer();

            // Check if item can be dropped on target from dropLocation
            bool isLand = world.getTerraintile(dropLocation).IsWalkable;
            bool hasSolidEntitiesOnTarget = world.checkCollision(this, dropLocation);
            List<Entity> entitiesOnTerrainTile = world.getEntitiesOnTerrainTile(location);

            // If no item exists in inventory or 
            //      there is a solid target on dropLocation or
            //      targettile is not land, return
            if (!Inventory.Any() || hasSolidEntitiesOnTarget || !isLand)
                return;

            // If item is key, lock door
            if (Inventory[0].Entity is Key)
            {
                if (entitiesOnTerrainTile.Exists(e => e is Door))
                    return;
                world.LockDoor((Key)Inventory[0].Entity);
            }

            // Set droplocation on item
            Inventory[0].Entity.setLocation(dropLocation);
            // Push item in world objects list
            world.getEntities().Add(Inventory[0].Entity);
            // Remove item from inventory
            Inventory.RemoveAt(0);
        }

	    private PointF GetLocationInFrontOfPlayer()
	    {
	        switch (CurrentDirection)
	        {
                case (int)Direction.Up:
                    return new PointF(location.X, location.Y - 1);
                case (int)Direction.Down:
                    return new PointF(location.X, location.Y + 1);
                case (int)Direction.Left:
                    return new PointF(location.X - 1, location.Y);
                case (int)Direction.Right:
                    return new PointF(location.X + 1, location.Y);
            }
            return new PointF(location.X, location.Y);
        }
	}
}