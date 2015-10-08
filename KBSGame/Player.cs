using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Player : Entity
    {
        private List<Entity> Inventory;
		public Player(Point location, Byte height)
            : base(location, (int)SPRITES.player, true, height, 10)
		{
		    Inventory = new List<Entity>();
		}

		public override void move(World sender, Point relativeLocation)
		{
			int moveLocationX = location.X + relativeLocation.X;
			int moveLocationY = location.Y + relativeLocation.Y;

		    Point targetPoint = new Point(moveLocationX, moveLocationY);
            TerrainTile targetTile = sender.getTerraintile (targetPoint);
            List<Entity> targetEntities = sender.getEntitiesOnTerrainTile(targetPoint);

            // Check for waterlily on the tile
		    bool IsWaterlily = targetEntities.Any(e => e.getSpriteID() == (int) SPRITES.waterlily);
            // Check for solid objects on the tile
		    bool IsSolid = targetEntities.Any(e => e.getSolid());
            // Check for key on the tile. If true, assign it.
		    Key key = (Key) targetEntities.FirstOrDefault(e => e.getSpriteID() == (int) SPRITES.key);

            // Log current inventory
		    Inventory.ForEach(Console.WriteLine);

			if (targetTile == null || IsSolid)
                return;
		    if (key != null && !IsSolid)
		    {
		        PickUp(sender, key);
		    }
		    // If terrain is walkable or stepping on a waterlily
            if (targetTile.IsWalkable || IsWaterlily)
			{
				location.X = moveLocationX;
				location.Y = moveLocationY;
			}
		}

        public void PickUp(World world, Entity e)
        {
            world.RemoveItem(e);
            Inventory.Add(e);
        }

        // Removes first item from inventory if exists
        public void DropItem(World world)
        {
            Point dropLocation = new Point(location.X + 1, location.Y);

            // Check if item can be dropped on target from dropLocation
            bool hasSolidEntitiesOnTarget = world.getEntitiesOnTerrainTile(dropLocation).Any(e => e.getSolid());

            // If no item exists in inventory or there is a solid target on dropLocation, return
            if (!Inventory.Any() || hasSolidEntitiesOnTarget)
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
