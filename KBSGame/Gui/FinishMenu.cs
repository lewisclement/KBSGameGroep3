using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame.gui
{
    class FinishMenu : Gui
    {
        private List<Button> buttonList;
        private String menu;

        World map;

        

        public FinishMenu(int ID, int ScreenresX, int ScreenresY, String Menu, World map) : base(ID, ScreenresX, ScreenresY)
        {
            this.map = map;
            this.menu = Menu; //Basic Menu class
			buttonList = new List<Button>(); //Button List

            xRes = ScreenresX;              
            yRes = ScreenresY;

            buffer = new Bitmap(xRes, yRes);
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            int width = xRes / 2;

            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);     //Fonttype

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, (yRes / 3) * 2, xRes, yRes / 3); //Draw a large square for content
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, (yRes / 3), style);

            float fontSize = StaticVariables.dpi / 3; //Create fontSize on the basis of dpi

            if (hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), hoverPos * width, (yRes / 3) * 2, width, yRes / 3);

            g.DrawString("Home", new Font("Arial", fontSize), new SolidBrush(Color.White), (xRes / 4), (yRes / 4) * 3);     // Create button
            g.DrawString("Next Level", new Font("Arial", fontSize), new SolidBrush(Color.White), (xRes / 5) * 3, (yRes / 4) * 3); //Create button

            return this.buffer;

            
        }

        int width = StaticVariables.dpi * 4;
        int hoverPos = -1, clickPos = -1;

        public override void setMouseClick(Point mousePos)
        {
            if ((mousePos.X < xRes / 2))
                clickPos = 0;
            //else
            //    clickPos = (mousePos.Y - ((yRes /4)* 3)) / StaticVariables.dpi;

            if ((mousePos.X > xRes / 2))
                clickPos = 1;

            switch (clickPos)
            {
                case 0:
                    setActive(false);
                    
                    Console.WriteLine("Exit to main menu");
                    Application.Exit();
                    break;
                case 1:
                    
                    map.reload();
                    setActive(false);
                    
                    break;
                default:
                    break;
            }
        }

        public override void setMouseHover(Point mousePos)
        {
            if (mousePos.X < xRes / 2)
                hoverPos = 0;
            if (mousePos.X >= xRes / 2)
                    hoverPos = 1;
        }
    }
}
