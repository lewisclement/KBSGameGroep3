using System;

namespace KBSGame
{
    public enum SPRITES : int {water=0, grass, sand, player, dirt, 
								sapling1, sapling2, tallgrass, waterlily, key, 
								banana, finish, berrybush, trapOpened, trapClosed, count};
	public enum ENTITIES : int {def=0, player, finish, key, plant, fruit, trap, count};
	enum TERRAIN : int {water=0, grass, sand, dirt, count}
	enum GUI : int {def=0, gameover, finish, guiinventory, count};
    
    public static class StaticVariables
	{
		public const int tileSize = 32;
		public const int minWorldSize = 20;
		public const int maxWorldSize = 1000;
		public static String execFolder = null;
		public static String levelFolder = null;
		public const Byte drawOrderSize = 16;
        
		public static int dpi;
	}

	public static class xmlVar
	{
		public const String Tile = "t";
		public const String ID = "i";
		public const String Entity = "e";
		public const String Type = "ty";
		public const String SpriteID = "s";
		public const String Solid = "so";
		public const String DrawOrder = "d";
		public const String Width = "w";
		public const String Height = "h";
	}		
}
