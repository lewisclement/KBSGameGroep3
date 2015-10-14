using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KBSGame
{
    public class Player : Entity
    {
        private List<Item> Inventory;
		public Player(PointF location, Byte height)
            : base(location, (int)SPRITES.player, true, height, 10)
		{
		    Inventory = new List<Item>();
		}
		public override void move(World sender, PointF relativeLocation)
		{
			float moveLocationX = location.X + relativeLocation.X;
			float moveLocationY = location.Y + relativeLocation.Y;

		    PointF targetPoint = new PointF(moveLocationX, moveLocationY);
            TerrainTile targetTile = sender.getTerraintile (targetPoint);
            List<Entity> targetEntities = sender.getEntitiesOnTerrainTile(targetPoint);
            
            // Initialize booleans for entities on the tile
		    bool HasSolidEntities = targetEntities.Any(e => e.getSolid());
		    bool HasWalkableEntities = targetEntities.Any(e => e.getSolid() == false);

            // Log current inventory
		    Inventory.ForEach(Console.WriteLine);

            // If terrain contains solid objects OR if tile has no walkable entity on a non-walkable tile
            if (targetTile == null || HasSolidEntities ||
                (!HasWalkableEntities && !targetTile.IsWalkable))
                return;

            //Check if the player is moving on a entity and if that entity is Finish
            foreach (Entity f in sender.getEntitiesOnTerrainTile(targetPoint))
            {
                if (f.getSpriteID() == (int)SPRITES.finish)
                {
                    ((Finish)f).LevelDone();
                    
                }
            }
            

		    location.X = moveLocationX;
		    location.Y = moveLocationY;
		    PickUpItems(sender);
		}

        public void PickUpItems(World world)
        {
            List<Item> items = world.getItemsOnTerrainTile(new PointF(location.X, location.Y));
            foreach (Item i in items.Where(i => i.CanPickup))
            {
                PickUp(world, i);
            }
        }

        public void PickUp(World world, Item i)
        {
            world.RemoveItem(i);
            Inventory.Add(i);
        }

        // Removes first item from inventory if exists
        public void DropItem(World world)
        {
            PointF dropLocation = new PointF(location.X + 1, location.Y);

            // Check if item can be dropped on target from dropLocation
            bool isLand = world.getTerraintile(dropLocation).IsWalkable;
            bool hasSolidEntitiesOnTarget = world.getEntitiesOnTerrainTile(dropLocation).Any(e => e.getSolid());

            // If no item exists in inventory or 
            //      there is a solid target on dropLocation or
            //      targettile is not land, return
            if (!Inventory.Any() || hasSolidEntitiesOnTarget || !isLand)
                return;

            // Set droplocation on item
            Inventory[0].setLocation(dropLocation);
            // Push item in world objects list
            world.getEntities().Add(Inventory[0]);
            // Remove item from inventory
            Inventory.RemoveAt(0);
        }
    }
}
