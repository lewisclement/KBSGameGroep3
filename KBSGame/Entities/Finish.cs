﻿using System;
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

		public void LevelDone()
		{
			//GameOverMenu finish = new GameOverMenu(50, 200, 200, "finish", );
			//finish.addMenuItem("HE DIKKE JONKO");



			//System.Windows.Forms.Application.Exit();
			Console.WriteLine("HALLO LEKKER DING! LEWIS IS GREAT");
		}
	}
}