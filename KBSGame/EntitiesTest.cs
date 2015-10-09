using NUnit.Framework;
using System;

namespace KBSGame
{
	[TestFixture ()]
	public class EntitiesTest
	{
		[Test ()]
		public void TestCase ()
		{
			World w = new World (50, 50);

			Sprite s1 = new Sprite (0, "fake.file");
			Sprite s2 = new Sprite (1, "fake.file");
			Sprite s3 = new Sprite (1, "fake.file"); //Should give error and return null

			Entity e1 = new Entity (new System.Drawing.Point(2, 5), s1.getID());
			Entity e2 = new Entity (new System.Drawing.Point(2, 2), s1.getID(), true);
			Entity e3 = new Entity (new System.Drawing.Point(1, 2), s2.getID(), true);

			w.addEntity (e1);
			w.addEntity (e2);
			w.addEntity (e3);

			w.getPlayer ().setLocation (new System.Drawing.Point (1, 1));
			w.getPlayer ().move (w, new System.Drawing.Point (0, 1));
			Assert.AreEqual (new System.Drawing.Point(1, 1), w.getPlayer ().getLocation ());
		}
	}
}

