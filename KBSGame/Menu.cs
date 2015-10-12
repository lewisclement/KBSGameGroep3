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
    public class Menu : Gui 
    {
		struct Button {
			public String text;

			public Button(String text)
			{
				this.text = text;
			}
		};

		private List<Button> buttonList;
        private String menu;

		int hoverPos, clickPos;

        public Menu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu;
			buttonList = new List<Button>();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

			buttonList.Add(new Button("Resume"));
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Quit"));
        }

		public override void setMouseClick(Point mousePos)
        {
			clickPos = mousePos.Y / 60;

			switch (clickPos) {
			case 0:
				setActive (false);
				break;
			case 3:
				Application.Exit();
				break;
			default:
				break;
			}
        }

		public override void setMouseHover(Point mousePos)
		{
			hoverPos = mousePos.Y / 60;
		}

		public void addMenuItem(String text)
        {
			this.buttonList.Add(new Button(text));
        }

		public String[] getButtonList()
        {
			String[] returnStrings = new String[buttonList.Count];
			for (int i = 0; i < buttonList.Count; i++) {
				returnStrings [i] = buttonList [i].text;
			}

			return returnStrings;
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

			int width = xRes / 5;

			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, width, yRes);
            g.DrawString(this.menu, new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);
			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, hoverPos * 60, width, 60);

			for (int i = 0; i < buttonList.Count; i++)
            {
				g.DrawString(buttonList[i].text, new Font("Arial", 20), new SolidBrush(Color.White), 0, i * 60);
            }

            return this.buffer;
        }
    }

}
