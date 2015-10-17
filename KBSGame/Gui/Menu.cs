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

		public enum STATE : int {main=0, pause, editor}

		private List<List<Button>> menus;
		private List<Button> buttonList;
        private String menu;
		private int width = StaticVariables.dpi * 4;
		private STATE currentState;
		private World world;

		int hoverPos = -1, clickPos = -1;

		public Menu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World world) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
			this.world = world;
			menus = new List<List<Button>> ();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

			List<Button> buttonList = new List<Button> ();
			buttonList.Add(new Button("Start"));
			buttonList.Add(new Button("Editor"));
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Quit"));
			menus.Insert ((int)STATE.main, buttonList);

			buttonList = new List<Button> ();
			buttonList.Add(new Button("Resume"));
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Exit to main menu"));
			menus.Insert ((int)STATE.pause, buttonList);

			buttonList = new List<Button> ();
			buttonList.Add(new Button("Settings"));
			buttonList.Add(new Button("Help"));
			buttonList.Add(new Button("Save & exit"));
			buttonList.Add(new Button("Exit (no save)"));
			menus.Insert ((int)STATE.editor, buttonList);

			changeState (STATE.main);
			this.setActive (true);
        }

		public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);

			if (mousePos.X > width)
				clickPos = -1;
			else
				clickPos = mousePos.Y / StaticVariables.dpi;

			if (currentState == STATE.main) {
				switch (clickPos) {
				case 0:
					setActive (false);
					world.loadLevel ("testworld");
					changeState (STATE.pause);
					break;
				case 4:
					Application.Exit ();
					break;
				default:
					break;
				}
			} else if (currentState == STATE.pause) {
				switch (clickPos) {
				case 0:
					setActive (false);
					break;
				case 3:
					changeState (STATE.main);
					break;
				default:
					break;
				}
			} else if (currentState == STATE.editor) {

			}
        }

		public override void setMouseHover(Point mousePos)
		{
			mousePos = scaleToDrawRatio (mousePos);

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

			int width = Math.Min(StaticVariables.dpi * 3, xRes / 2);

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

			g.Dispose ();
            return this.buffer;
        }

		public void changeState(STATE state)
		{
			if (state == STATE.main) {
				world.loadLevel ("mainmenu");
				world.setFocusEntity(world.getEntitiesByType (ENTITIES.plant)[40]);
			}

			currentState = state;
			buttonList = menus [(int)state];
		}
    }

}
