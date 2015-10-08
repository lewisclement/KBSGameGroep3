﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Player : Entity
    {
		public Player(Point location, Byte height) : base(location, (int)SPRITES.player, true, height, 10)
        {
        }

		public override void move(World sender, Point relativeLocation)
		{
			int moveLocationX = location.X + relativeLocation.X;
			int moveLocationY = location.Y + relativeLocation.Y;

		    Point targetPoint = new Point(moveLocationX, moveLocationY);
            TerrainTile targetTile = sender.getTerraintile (targetPoint);
            Entity targetEntity = sender.getEntityOnTerrainTile(targetPoint);
           
            // Get the entity on the tile.
            // Assign SpriteID if entity exists, otherwise -1.
		    int targetEntityID = sender.getEntityOnTerrainTile(targetPoint)?.getSpriteID() ?? -1;

			if (targetTile == null)
				return;
            if (targetEntity != null && targetEntity.getSolid())
                return;
            // If terrain is walkable or stepping on a waterlily
            if (targetTile.IsWalkable || targetEntityID == (int) SPRITES.waterlily)
			{
				location.X = moveLocationX;
				location.Y = moveLocationY;
			}
            
		}
    }
}
