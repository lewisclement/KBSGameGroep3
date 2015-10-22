using System;

namespace KBSGame
{
    public enum SPRITES : int {water=0, grass, sand, player, dirt, 
								sapling1, sapling2, tallgrass, waterlily, key, 
								banana, finish, berrybush, trapOpened, trapClosed,
								brick, clay, farmland, planks_birch, 
								red_sand, sandstone, stone, stonebrick, stone_diorite, 
								stone_granite, 
								flower_allium, flower_blue_orchid, flower_dandelion, flower_houstonia, flower_oxeye_daisy, 
								flower_rose, flower_tulip_orange, flower_tulip_pink, flower_tulip_red, flower_tulip_white, 
								carrots_stage_0, carrots_stage_1, carrots_stage_2, carrots_stage_3, 
								sapling_acacia, sapling_birch, sapling_jungle, sapling_oak, sapling_roofed_oak, sapling_spruce, 
								wheat_stage_7, deadbush, count};
	
	public enum ENTITIES : int {def=0, player, finish, key, plant, fruit, trap,
								carrots, flower, sapling, count};

	public enum TERRAIN : int {water=0, grass, sand, dirt, brick, clay, farmland, planks_birch, 
								red_sand, sandstone, stone, stonebrick, stone_diorite, 
								stone_granite, count};
	
	public enum GUI : int {def=0, gameover, finish, guiinventory, editor, count};

    
    public static class StaticVariables
	{
		public const int tileSize = 32;
		public const int minWorldSize = 20;
		public const int maxWorldSize = 1000;
		public static int viewWidth, viewHeight;
		public static String execFolder = null;
		public static String levelFolder = null;
		public static String spriteFolder = null;
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
