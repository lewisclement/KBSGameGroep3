﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame.gui
{
    class FinishMenu : Gui
    {
        private List<Button> buttonList;
        private String menu;

        public FinishMenu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu; //Basic Menu class
			buttonList = new List<Button>(); //Button List

            xRes = ScreenresX;              
            yRes = ScreenresY;

            buffer = new Bitmap(xRes, yRes);
        }

        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));

            int width = StaticVariables.dpi * 4;

            StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Center;
            Font font = new Font("Arial", StaticVariables.dpi / 2, FontStyle.Bold);     //Fonttype

            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, (yRes / 3) * 2, xRes, yRes); //Draw a large square for content
            g.DrawString(this.menu, font, new SolidBrush(Color.White), xRes / 2, (yRes / 3), style);

            float fontSize = StaticVariables.dpi / 3; //Create fontSize on the basis of dpi

            g.DrawString("Home", new Font("Arial", fontSize), new SolidBrush(Color.White), (xRes / 4), (yRes / 4) * 3);     // Create button
            g.DrawString("Next Level", new Font("Arial", fontSize), new SolidBrush(Color.White), (xRes / 5) * 3, (yRes / 4) * 3); //Create button

            return this.buffer;
        }
    }
}