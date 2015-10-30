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
        //Initialising attributes
        int width = StaticVariables.dpi * 4;
        int hoverPos = -1, clickPos = -1;
        World map;

		Image tryAgain;
		Image exit;
		Image gameOver;

        /// <summary>
        /// Basic Menu constructor for screen resolution
        /// </summary>
        /// <param name="ID">Menu ID</param>
        /// <param name="ScreenresX">Width of the screen</param>
        /// <param name="ScreenresY">Length of the screen</param>
        /// <param name="drawRatio"></param>
        /// <param name="map"></param>
		public GameOverMenu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World map) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            this.map = map;
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);

			tryAgain = Image.FromFile(StaticVariables.textFolder + "/gameover_try_again.png");;
			exit = Image.FromFile(StaticVariables.textFolder + "/menu_exit.png");
			gameOver = Image.FromFile(StaticVariables.textFolder + "/game_over.png");
        }

        /// <summary>
        /// Method checks if the user has clicked on the top or bottom button.
        /// </summary>
        /// <param name="mousePos"></param>
		public override void setMouseClick(Point mousePos, bool leftClick)
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
                //If the player pressed on the button "Quit", the game will quit to the main menu.
                case 1:
                    setActive(false);
                    StaticVariables.world.loadLevel("mainmenu");
                    ((Menu)StaticVariables.renderer.getGui((int)GUI.def)).mainmenu();
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
                if (hoverPos >= 2)
                    hoverPos = -1;
            }
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
            int topskip = 100; //Topskip is necessary to allow text above the drawn Rectangles.

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), xRes / 2 - width / 2, topskip, width, yRes / 3 + topskip);
            if (hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), xRes / 2 - width / 2, hoverPos * StaticVariables.dpi + topskip, width, StaticVariables.dpi);

            //For every item inside the buttonList, draws something:
            for (int i = 0; i < 2; i++)
            {
                float x = xRes / 2 - width / 2 + 40;
                float y = StaticVariables.dpi * i + topskip;
                //If statement is here to prevent having the same image being drawn inside the For-loop.
                if(i == 0) //First button
                {
					g.DrawImage(tryAgain, x, y, 300, 120);
                } else { //Second button - Could also be (i == 1)
					g.DrawImage(exit, x, y, 300, 120);
                }
            }
            //Draws the Game Over text in the top center of the screen.
			g.DrawImage(gameOver, xRes / 2 - width / 2, 0, width, yRes / 5);
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
