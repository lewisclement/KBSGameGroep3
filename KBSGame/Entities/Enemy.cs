using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class Enemy : Entity
    {
        public Enemy(ENTITIES type, PointF location, int spriteID, bool solid = false, Byte height = 50, Byte drawOrder = 8, float boundingBox = 1.0f)
			: base(type, location, spriteID, solid, height, drawOrder, boundingBox)
		{

        }
		/// <summary>
		/// Dead this instance.
		/// </summary>
        public void Dead()
        {
        }
		/// <summary>
		/// checks for collisions.
		/// </summary>
		/// <param name="e">E.</param>
        public override void collisionCheck(Entity e)
        {
            if (this.location == e.getLocation())
            {
                Dead();
            }
        }
    }
}
