using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBSGame
{
    class GuiInventory : Gui
    {

        public GuiInventory(int ID, int ScreenresX, int ScreenresY) : base(ID, ScreenresX, ScreenresY)
        {
            xRes = ScreenresX;
            yRes = ScreenresY;
        }
    }
}
