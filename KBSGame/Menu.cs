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
			public Rectangle area;

			public Button(String text, Rectangle area)
			{
				this.text = text;
				this.area = area;
			}
		};

		private List<Button> buttonList;
        private String menu;
        public Menu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu;
			buttonList = new List<Button>();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

			buttonList.Add(new Button("sup", new Rectangle(50, 50, 50, 50)));
			//StringList.Add("Settings");
			//StringList.Add("Quit game");

        }

        public override void setInput(Point mousePos)
        {
			for (int i = 0; i < buttonList.Count; i++)
            {
				if (buttonList[i].area.Contains(mousePos))
                {
                }
            }
        }

		public void addMenuItem(String text)
        {
			this.buttonList.Add(new Button(text, new Rectangle(0,0,0,0)));
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

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, xRes, yRes);
            g.DrawString(this.menu, new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);
			for (int i = 0; i < buttonList.Count; i++)
            {
				g.DrawString(buttonList[i].text, new Font("Arial", 20), new SolidBrush(Color.White), 0, 60 + i * 60);
            }

            return this.buffer;
        }
    }

}
