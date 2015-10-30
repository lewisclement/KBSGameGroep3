using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class Enemy : Entity
    {
		static int radius = 5;
        private bool IsAlive = true;

        public Enemy(PointF location, Byte height = 50, bool solid = true, Byte depth = 8, float boundingBox = 0.6f)
            : base(ENTITIES.enemy, location, (int)SPRITES.tiki1, solid, height, depth, boundingBox)
        {

        }

        public override void onCollision(Entity e)
        {
            if (!IsAlive) return;
            StaticVariables.controller.gameover();
        }

		public virtual void processTick()
		{
			PointF player = StaticVariables.world.getPlayer ().getLocation();

		    if (!IsAlive) return;
			PointF relativeLocation = new PointF (0.0f, 0.0f);
			if (player.X > location.X - radius && player.X < location.X + radius) {
				if (player.X > location.X)
					relativeLocation.X += 0.06f;
				else if (player.X < location.X)
					relativeLocation.X -= 0.06f;

				move (relativeLocation);
			}

			relativeLocation = new PointF (0.0f, 0.0f);
			if (player.Y > location.Y - radius && player.Y < location.Y + radius) {
				if (player.Y > location.Y)
					relativeLocation.Y += 0.06f;
				else if (player.Y < location.Y)
					relativeLocation.Y -= 0.06f;

				move (relativeLocation);
			} 
		}

		private void move(PointF relativeLocation) 
		{
            if (!IsAlive) return;
            World world = StaticVariables.world;

			float moveLocationX = location.X + relativeLocation.X;
			float moveLocationY = location.Y + relativeLocation.Y;

			PointF targetPoint = new PointF(moveLocationX, moveLocationY);
			TerrainTile targetTile = world.getTerraintile (targetPoint);

			if (world.checkCollision (this, StaticVariables.world.getPlayer ())) {
				onCollision(this);
			}

			// If terrain contains solid objects OR if tile has no walkable entity on a non-walkable tile
			if (targetTile == null || world.checkCollision(this, targetPoint) || !targetTile.IsWalkable && !world.checkCollision(this, targetPoint, false))
				return;

			location.X = (float)Math.Round(moveLocationX, 2);
			location.Y = (float)Math.Round(moveLocationY, 2);

			//Check if the player is moving on a entity and call oncolission of that entity
			List<Entity> entities = world.getEntities();

			for(int i = 0; i < entities.Count; i++)
			{
				if(world.checkCollision(this, entities[i]) && entities[i].getType() != ENTITIES.enemy)
				{
					entities[i].onCollision(this);
				}
			}
		}

        public void Die()
        {
            IsAlive = false;
        }
    }
}
