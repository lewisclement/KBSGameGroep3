using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    public class GuiInventory : Gui
    {
        const int SPACING = 8;
        const int HEIGHT = 48;
        const int WIDTH = 408;
        public World world;
        private Sprite[] sprites;
		public GuiInventory(int ID, int ScreenresX, int ScreenresY, float drawRatio, World w, Sprite[] s)
			: base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            setActive(false);
		    world = w;
            sprites = s;
        }

        public override Bitmap getRender()
        {
            Player player = world.getPlayer();
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);
            
            g.FillRectangle(new SolidBrush(Color.FromArgb(90, Color.Black)), 0, 0, WIDTH, HEIGHT);
            for(int i = 0; i < player.Inventory.Count; i++)
            {
                int spriteID = player.Inventory[i].Entity.getSpriteID();
                
                g.DrawImage(sprites[spriteID].getBitmap(), SPACING + (i * (HEIGHT - SPACING)), SPACING, StaticVariables.tileSize, StaticVariables.tileSize);
            }

            return this.buffer;
        }
    }
}
