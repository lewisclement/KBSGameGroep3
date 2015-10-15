using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class GuiInventory : Gui
    {
        public GuiInventory(int ID, int ScreenresX, int ScreenresY)
            : base(ID, ScreenresX, ScreenresY)
        {
            setActive(true);
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            int width = StaticVariables.dpi * 4;

            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);
            
            g.FillRectangle(new SolidBrush(Color.FromArgb(90, Color.Black)), 0, 0, 512, 64);
//            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, StaticVariables.dpi / 4, style);
//            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, hoverPos * StaticVariables.dpi, width, StaticVariables.dpi);

            /*for (int i = 0; i < buttonList.Count; i++)
            {
                float fontSize = StaticVariables.dpi / 3;
                float x = StaticVariables.dpi / 4;
                float y = StaticVariables.dpi * i + fontSize / 2;

                g.DrawString(buttonList[i].text, new Font("Arial", fontSize), new SolidBrush(Color.White), x, y);
            }*/

            return this.buffer;
        }
    }
}
