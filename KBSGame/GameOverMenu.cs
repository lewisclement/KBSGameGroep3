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

        int hoverPos, clickPos;

        public GameOverMenu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu;
            buttonList = new List<Button>();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

            buttonList.Add(new Button("Retry"));
            buttonList.Add(new Button("Quit"));
        }

        public override void setMouseClick(Point mousePos)
        {
            clickPos = mousePos.Y / StaticVariables.dpi;

            switch (clickPos)
            {
                case 0:
                    setActive(false);
                    //Here comes a method that needs to reload the current map
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
            hoverPos = mousePos.Y / StaticVariables.dpi;
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

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, 0, width, yRes); //The 150 needs to be changed to the middle value of the screen
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, StaticVariables.dpi / 4, style);
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, hoverPos * StaticVariables.dpi, width, StaticVariables.dpi);

            for (int i = 0; i < buttonList.Count; i++)
            {
                float fontSize = StaticVariables.dpi / 3;
                float x = StaticVariables.dpi / 4;
                float y = StaticVariables.dpi * i + fontSize / 2;

                g.DrawString(buttonList[i].text, new Font("Arial", fontSize), new SolidBrush(Color.White), x, y);
            }

            return this.buffer;
        }
    }

}
