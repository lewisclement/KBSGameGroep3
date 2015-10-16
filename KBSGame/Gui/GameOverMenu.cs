using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame
{
    public class GameOverMenu : Gui
    {
        struct Button
        {
            public String text;

            public Button(String text)
            {
                this.text = text;
            }
        };

        private List<Button> buttonList;
        private String menu;
        int width = StaticVariables.dpi * 4;

        int hoverPos = -1, clickPos = -1;
        World map;

		public GameOverMenu(int ID, int ScreenresX, int ScreenresY, float drawRatio, String Menu, World map) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            this.map = map;
            this.menu = Menu;
            buttonList = new List<Button>();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

            buttonList.Add(new Button("Try again!"));
            buttonList.Add(new Button("Quit"));
        }

        public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);

            if (mousePos.X < xRes / 2 - width / 2 || mousePos.X > xRes / 2 + width / 2)
                clickPos = -1;
            else
                clickPos = (mousePos.Y - 100) / StaticVariables.dpi;

            switch (clickPos)
            {
                case 0:
                    map.reload();
                    
                    setActive(false);
                    break;
                case 1:
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        public override void setMouseHover(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);

            if (mousePos.X < xRes / 2 - width / 2 || mousePos.X > xRes / 2 + width / 2)
                hoverPos = -1;
            else
            {
                hoverPos = (mousePos.Y - 100) / StaticVariables.dpi;
                if (hoverPos >= buttonList.Count)
                    hoverPos = -1;
            }
        }

        public void addMenuItem(String text)
        {
            this.buttonList.Add(new Button(text));
        }

        public String[] getButtonList()
        {
            String[] returnStrings = new String[buttonList.Count];
            for (int i = 0; i < buttonList.Count; i++)
            {
                returnStrings[i] = buttonList[i].text;
            }

            return returnStrings;
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            int width = StaticVariables.dpi * 4;

            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, 100, width, yRes / 2);
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, StaticVariables.dpi / 4, style);
            
            if(hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, hoverPos * StaticVariables.dpi + 100, width, StaticVariables.dpi);

            for (int i = 0; i < buttonList.Count; i++)
            {
                float fontSize = StaticVariables.dpi / 4;
                float x = xRes / 2 - width / 2;
                float y = StaticVariables.dpi * i + fontSize / 2 + 100;

                g.DrawString(buttonList[i].text, new Font("Arial", fontSize), new SolidBrush(Color.White), x, y);
            }

            return this.buffer;
        }
    }

}
