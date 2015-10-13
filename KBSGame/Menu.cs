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
		int width = StaticVariables.dpi * 4;

		int hoverPos = -1, clickPos = -1;

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
			if (mousePos.X > width)
				clickPos = -1;
			else
				clickPos = mousePos.Y / StaticVariables.dpi;

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
			if (mousePos.X > width)
				hoverPos = -1;
			else {
				hoverPos = mousePos.Y / StaticVariables.dpi;
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
			for (int i = 0; i < buttonList.Count; i++) {
				returnStrings [i] = buttonList [i].text;
			}

			return returnStrings;
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

			int width = StaticVariables.dpi * 4;

			StringFormat style = new StringFormat ();
			style.Alignment = StringAlignment.Center;
			Font font = new Font ("Arial", StaticVariables.dpi / 2, FontStyle.Bold);

			g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, width, yRes);
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
