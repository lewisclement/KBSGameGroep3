using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    /// <summary>
    /// Expends the Entity class and adds a new function to it.
    /// </summary>
    public class Trap : Entity
    {
        public Trap(PointF location, int spriteID, Byte height = 50, bool solid = false, Byte depth = 8, float boundingBox = 0.7f)
			: base(ENTITIES.trap, location, spriteID, solid, height, depth, boundingBox)
		{
        }

        /// <summary>
        /// Method which interacts whenever a player hit the object with the onCollision() method.
        /// </summary>
        public override void onCollision(Entity e)
        {
            if (this.spriteID != (int) SPRITES.trap_opened) return;
            this.spriteID = (int) SPRITES.trap_closed;
            if (e is Player)
                StaticVariables.controller.gameover();
            if (e.getType() == ENTITIES.enemy)
            {
                ((Enemy) e).Die();
            }
        }
    }
}
