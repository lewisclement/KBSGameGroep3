using System;

namespace KBSGame
{
    //Assigns an ID to every single thing for drawing purpose
    public enum SPRITES : int
    { 
        //Tiles
        water = 0, dirt, light_dirt, dark_dirt,
        clay, red_sand, sand, sandstone,
        grass_normal, grass_dark, grass_noisy, grass_path,
        stone_cracked_light, stone_cracked_dark, stone_mossy_light, stone_mossy_dark, 
        stone, stone_wall, stonebrick, stone_diorite,
        lava, lava_stones, stone_granite,
        //Trees and Plants
        tree1, tree2, tree3,
        sapling_acacia, sapling_jungle, sapling_oak, sapling_roofed_oak,
        shrub1, shrub2, tallgrass, 
        waterlily, bamboo,
        flower_allium, flower_blue_orchid, flower_dandelion, flower_houstonia, flower_oxeye_daisy,
        flower_rose, flower_tulip_orange, flower_tulip_pink, flower_tulip_red, flower_tulip_white,
        //Objects
        hut1, hut2, hut3, hut4, cage, logpile,
        rock, mountain,
        key, door_opened, door_closed,
        trap_opened, trap_closed,
        banana, tiki1, tiki2, peerbomb, player,
        //Icons
        finish, icon_world, folder, save, load, count
    };

    //Assigns an ID to every single entity.
    public enum ENTITIES : int
    {
        def = 0, player, finish, key, plant,
        fruit, trap, carrots, flower, sapling,
        door, peerbomb, tiki1, tiki2, enemy, count
    };

    //Assigns an ID to every single terraintile.
    public enum TERRAIN : int {
        water = 0, dirt, light_dirt, dark_dirt,
        clay, red_sand, sand, sandstone,
        grass_normal, grass_dark, grass_noisy, grass_path,
        stone_cracked_light, stone_cracked_dark, stone_mossy_light, stone_mossy_dark, 
        stone, stone_wall, stonebrick, stone_diorite,
        lava, lava_stones, stone_granite,
        count };
	
    //Gives every menu it's own ID so it can set to active.
	public enum GUI : int {def=0, gameover, finish, guiinventory, editor, count};
    //The state the main menu can be in.
	public enum STATE : int {main=0, pause, editor, levelloader}

    //Static class which contains every static data.
    public static class StaticVariables
	{
		public const int tileSize = 32;
		public const int minWorldSize = 20;
		public const int maxWorldSize = 1000;
		public static int viewWidth, viewHeight;
		public static String execFolder = null;
		public static String levelFolder = null;
		public static String spriteFolder = null;
        public static String textFolder = null;
		public const Byte drawOrderSize = 16;
		public static STATE currentState = STATE.main;
		public static int dpi;
		public static Controller controller;
		public static DrawEngine renderer;
		public static World world;
	}

    //Sets a static abbrevation to commonly used strings. 
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
		public const String BoudingBox = "b";
	}		
}
