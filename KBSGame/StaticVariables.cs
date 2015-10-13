using System;

namespace KBSGame
{
    public enum SPRITES : int {water=0, grass, sand, player, dirt, sapling1, sapling2, tallgrass, waterlily, key, banana, fisnish, count};
	enum TERRAIN : int {water=0, grass, sand, dirt, count}
	enum GUI : int {def=0, gameover, count};
    
    public static class StaticVariables
	{
		public const int tileSize = 32;
		public const int minWorldSize = 20;
		public const int maxWorldSize = 1000;
		public static String execFolder = null;
		public const Byte drawOrderSize = 16;
        
		public static int dpi;
	}


			
}

