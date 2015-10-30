using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace KBSGame
{
	public class Finish : Entity
	{
		public Finish (PointF location, Byte height = 50, bool solid = false, Byte depth = 8, float boundingBox = 1.0f)
			: base(ENTITIES.finish, location, (int)SPRITES.finish, solid, height, depth, boundingBox)
		{

		}

        public override void onCollision(Entity e)
        {
            if(e is Player)
			    StaticVariables.controller.finish ();
        }
	}
}