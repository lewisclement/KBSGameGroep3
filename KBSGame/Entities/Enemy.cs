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
        public Enemy(PointF location, Byte height = 50, bool solid = false, Byte depth = 8, float boundingBox = 1.0f)
            : base(ENTITIES.enemy, location, (int)SPRITES.tiki, solid, height, depth, boundingBox)
        {

        }

        public override void onCollision()
        {
            StaticVariables.controller.gameover();
        }
    }
}
