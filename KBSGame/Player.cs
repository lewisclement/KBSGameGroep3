﻿using System;
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
		public Player(Point location, Byte height)
            : base(location, (int)SPRITES.player, true, height, 10)
		{
		    Inventory = new List<Item>();
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

            // Log current inventory
		    Inventory.ForEach(Console.WriteLine);

			if (targetTile == null || IsSolid)
                return;
		    // If terrain is walkable or stepping on a waterlily
            if (targetTile.IsWalkable || IsWaterlily)
			{
				location.X = moveLocationX;
				location.Y = moveLocationY;
			    PickUpItems(sender);
			}
		}

        public void PickUpItems(World world)
        {
            List<Item> items = world.getItemsOnTerrainTile(new Point(location.X, location.Y));
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
            Point dropLocation = new Point(location.X + 1, location.Y);

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
