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
        private List<String> StringList;
        private String menu;
        private Gui settings;
        private int ID, ScreenresX, ScreenresY;
		/// <summary>
		/// Initializes a new instance of the <see cref="KBSGame.Menu"/> class.
		/// </summary>
		/// <param name="ID">I.</param>
		/// <param name="ScreenresX">Screenres x.</param>
		/// <param name="ScreenresY">Screenres y.</param>
		/// <param name="Menu">Menu.</param>
        public Menu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu;
            this.ID = ID;
            this.ScreenresX = ScreenresX;
            this.ScreenresY = ScreenresY;
            
            new Gui(ID, ScreenresX, ScreenresY);
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
            StringList = new List<String>();
            StringList.Add("Resume");
            StringList.Add("Settings");
            StringList.Add("Credits");
            StringList.Add("High Score");
            StringList.Add("Quit game");

        }
		/// <summary>
		/// Sets the input. and uses this input
		/// </summary>
		/// <param name="mousePos">Mouse position.</param>
        public override void setInput(Point mousePos)
        {
            for (int i = 0; i < StringList.Count; i++)
            {
                Rectangle rect = new Rectangle(0,60+i*60, this.yRes, 60);
                if (rect.Contains(mousePos))
                {
                    if (i == 0)
                    {
                        setActive(false);
                    }
                    if (i == 1)
                    {
                        this.settings = new Settings(ID, ScreenresX, ScreenresY); switchActive();
                        settings.setActive(true);
                    }
                    if (i == 4)
                    {
                        Application.Exit();
                    }
                }
            }
        }
		/// <summary>
		/// Adds a menu item.
		/// </summary>
		/// <param name="Value">Value.</param>
        public void addMenuItem(String Value)
        {
            this.StringList.Add(Value);
        }
		/// <summary>
		/// Gets the string list.
		/// </summary>
		/// <returns>The string list.</returns>
        public List<String> getStringList()
        {
            return this.StringList;
        }
        public override void resize(int ScreenresX, int ScreenresY)
        {
            xRes = ScreenresX;
            yRes = ScreenresY;

            buffer = new Bitmap(xRes, yRes);
        }
        /// <summary>
        /// Gets the render and draws the entire Menu Screen.
        /// </summary>
        /// <returns>The render.</returns>
        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));            
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, xRes, yRes); //To do: Figure out why -40 is nessecary to have the same margin
            g.DrawString(this.menu, new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);
            for (int i = 0; i < StringList.Count; i++)
            {
             //   g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.White)), xRes / 3, 60 + i * 6, xRes / 4*2, 20); //To do: Figure out why -40 is nessecary to achieve the same margin
                g.DrawString((string)StringList[i], new Font("Arial", 16), new SolidBrush(Color.White), 0, 60 + i * 60);
            }
            return this.buffer;
        }
    }

}
