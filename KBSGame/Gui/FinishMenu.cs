using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame.gui
{
    public class FinishMenu : Gui
    {
        World map;
		public FinishMenu(int ID, int ScreenresX, int ScreenresY, float drawRatio, World map) : base(ID, ScreenresX, ScreenresY, drawRatio)
        {
            this.map = map;
            xRes = ScreenresX;              
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));
            int width = xRes / 2;

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, (yRes / 3) * 2, xRes, yRes / 3); //Draw a large square for content
            Image newImage = Image.FromFile(StaticVariables.textFolder + "/finished.png");
            g.DrawImage(newImage, xRes / 2 - width / 2, yRes / 3, xRes / 2, (yRes / 3));
            
            if (hoverPos >= 0)
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, Color.SandyBrown)), hoverPos * width, (yRes / 3) * 2, width, yRes / 3);
                Image home = Image.FromFile(StaticVariables.textFolder + "/finished_home.png");
                Image nextlevel = Image.FromFile(StaticVariables.textFolder + "/finished_nextlevel.png");
                g.DrawImage(home, (xRes / 15), (yRes / 4) * 3, 300, 120);
                g.DrawImage(nextlevel, (xRes / 6) * 3 , (yRes / 4) * 3, 300, 120);
                return this.buffer; 
        }

        int width = StaticVariables.dpi * 4;
        int hoverPos = -1, clickPos = -1;

        //Determine where the mouse click happens and if it happens above one of the two buttons
        //initiate a appropiate reaction.

        public override void setMouseClick(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);
            if ((mousePos.X < xRes / 2))
                clickPos = 0;
            if ((mousePos.X > xRes / 2))
                clickPos = 1;
            switch (clickPos)
            {
			case 0:
				setActive (false);
				StaticVariables.world.loadLevel ("mainmenu");
				((Menu) StaticVariables.renderer.getGui ((int)GUI.def)).mainmenu ();
                    break;
                case 1:                
                    map.reload();
                    setActive(false);   
                    break;
                default:
                    break;
            }
        }
        // 

        public override void setMouseHover(Point mousePos)
        {
			mousePos = scaleToDrawRatio (mousePos);
            if (mousePos.X < xRes / 2)
                hoverPos = 0;
            if (mousePos.X >= xRes / 2)
                    hoverPos = 1;
        }

        //Set the menu active after reaching the finish entity.

		public override void setActive (bool active)
		{
			if (active && StaticVariables.controller.modalActive ())
				return;
			base.setActive (active);
			if (active)
				StaticVariables.controller.setModalGui (GUI.finish);
			else
				StaticVariables.controller.disableModalGui ();
		}
    }
}
