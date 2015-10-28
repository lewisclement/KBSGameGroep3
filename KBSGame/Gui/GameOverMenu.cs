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
    /// <summary>
    /// Basic code for extending the GUI class and allows text to be written inside the buttons.
    /// </summary>
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

        //Initialising attributes
        private List<Button> buttonList;
        int width = StaticVariables.dpi * 4;
        int hoverPos = -1, clickPos = -1;
        World map;

        /// <summary>
        /// Basic Menu constructor - Contains mostly solid data and creates a buttonList.
        /// </summary>
        /// <param name="ID">Menu ID</param>
        /// <param name="ScreenresX">Width of the screen</param>
        /// <param name="ScreenresY">Length of the screen</param>
        /// <param name="drawRatio"></param>
        /// <param name="map"></param>
		public GameOverMenu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World map) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            this.map = map;
            buttonList = new List<Button>();

            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

            //Adds two buttons to the buttonList.
            buttonList.Add(new Button("Try again!"));
            buttonList.Add(new Button("Quit"));
        }

        /// <summary>
        /// Method checks if the user has clicked on the top or bottom button.
        /// </summary>
        /// <param name="mousePos"></param>
        public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);

            //The two parameters for the buttons, clicking inbetween gives a respond back.
            if (mousePos.X < xRes / 2 - width / 2 || mousePos.X > xRes / 2 + width / 2)
                clickPos = -1;
            else
                clickPos = (mousePos.Y - 100) / StaticVariables.dpi;

            switch (clickPos)
            {
                //If the player pressed on the button "Try again", the map will reload and the GUI will close.
                case 0:
                    map.reload();
                    setActive(false);
                    break;
                //If the player pressed on the button "Quit", the game will quit.
                case 1:
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The method setMouseHover checks if the mouse comes within the two parameters of the mousePos. If it doesn't, the value of hoverPos changes.
        /// hoverPos gives another method the value to change colors when the mouse hovers within the parameters.
        /// </summary>
        /// <param name="mousePos"></param>
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
        /// <summary>
        /// Allows to add text to the buttonList.
        /// </summary>
        /// <param name="text"></param>
        public void addMenuItem(String text)
        {
            this.buttonList.Add(new Button(text));
        }

        /// <summary>
        /// Simple getter for a buttonList.
        /// </summary>
        /// <returns></returns>
        public String[] getButtonList()
        {
            String[] returnStrings = new String[buttonList.Count];
            for (int i = 0; i < buttonList.Count; i++)
            {
                returnStrings[i] = buttonList[i].text;
            }

            return returnStrings;
        }

        /// <summary>
        /// This method paints everything for this menu on the screen.
        /// </summary>
        /// <returns>Returns the GameOverGUI</returns>
        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            int width = StaticVariables.dpi * 4;
            //Topskip is necessary to allow text above the drawn Rectangles.
            int topskip = 100;

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, topskip, width, yRes / 3 + topskip);

            if (hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), xRes / 2 - width / 2, hoverPos * StaticVariables.dpi + topskip, width, StaticVariables.dpi);

            //For every item inside the buttonList, draws something:
            for (int i = 0; i < buttonList.Count; i++)
            {
                float x = xRes / 2 - width / 2 + 40;
                float y = StaticVariables.dpi * i + topskip;
                //If statement is here to prevent having the same image being drawn inside the For-loop.
                if(i == 0) //First button
                {
                Image tryagain = Image.FromFile(StaticVariables.textFolder + "/gameover_try_again.png");
                g.DrawImage(tryagain, x, y, 300, 120);
                } else { //Second button - Could also be (i == 1)
                Image quit = Image.FromFile(StaticVariables.textFolder + "/gameover_quit.png");
                g.DrawImage(quit, x, y, 300, 120);
                }
            }
            //Draws the Game Over text in the top center of the screen.
            Image newImage = Image.FromFile(StaticVariables.textFolder + "/game_over.png");
            g.DrawImage(newImage, xRes / 2 - width / 2, 0, width, yRes / 5);
            return this.buffer;
        }

        /// <summary>
        /// Simple method to activate the Menu onto the screen.
        /// </summary>
        /// <param name="active">Whenever the GameOverMenu is set to active, it will show on the screen.</param>
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
