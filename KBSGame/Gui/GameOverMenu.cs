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
        int width = StaticVariables.dpi * 4;

        int hoverPos = -1, clickPos = -1;
        World map;

		public GameOverMenu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World map) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            this.map = map;
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
            int topskip = 100;

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, topskip, width, yRes / 3 + topskip);

            if (hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), xRes / 2 - width / 2, hoverPos * StaticVariables.dpi + topskip, width, StaticVariables.dpi);

            for (int i = 0; i < buttonList.Count; i++)
            {
                float x = xRes / 2 - width / 2 + 40;
                float y = StaticVariables.dpi * i + topskip;

                if(i == 0)
                {
                Image tryagain = Image.FromFile(StaticVariables.textFolder + "/gameover_try_again.png");
                g.DrawImage(tryagain, x, y, 300, 120);
                } else {
                Image quit = Image.FromFile(StaticVariables.textFolder + "/gameover_quit.png");
                g.DrawImage(quit, x, y, 300, 120);
                }
            }
            Image newImage = Image.FromFile(StaticVariables.textFolder + "/game_over.png");
            g.DrawImage(newImage, xRes / 2 - width / 2, 0, width, yRes / 5);
            return this.buffer;
        }

		public override void setActive (bool active)
		{
			if (active && StaticVariables.controller.modalActive ())
				return;

			base.setActive (active);

			if (active)
				StaticVariables.controller.setModalGui (GUI.gameover);
			else
				StaticVariables.controller.disableModalGui ();
		}
    }

}
