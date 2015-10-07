using System;

namespace KBSGame
{
	enum SPRITES : int {water=0, grass, sand, player, count};
	enum TERRAIN : int {water=0, grass, sand, count}
	enum GUI : int {def=0, main, count};

	public static class StaticVariables
	{
		public const int tileSize = 32;
		public const int minWorldSize = 20;
		public const int maxWorldSize = 1000;
		public static String execFolder = null;
	}
}

