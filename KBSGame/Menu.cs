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
        public Menu(int ID, int ScreenresX, int ScreenresY, String Menu) : base(ID, ScreenresX, ScreenresY)
        {
            this.menu = Menu;
            new Gui(ID, ScreenresX, ScreenresY);
            xRes = ScreenresX;
            yRes = ScreenresY;
            buffer = new Bitmap(xRes, yRes);
            StringList = new List<String>();
            StringList.Add("Resume");
            StringList.Add("Settings");
            StringList.Add("Quit game");

        }
        public override void setInput(Point mousePos)
        {
            for (int i = 0; i < StringList.Count; i++)
            {
                Rectangle rect = new Rectangle(0,60+i*60, this.yRes, 60);
                if (rect.Contains(mousePos))
                {
                }
            }
        }
        public void addMenuItem(String Value)
        {
            this.StringList.Add(Value);
        }
        public List<String> getStringList()
        {
            return this.StringList;
        }
        public override Bitmap getRender()
        {
            var g = Graphics.FromImage(buffer);
            g.Clear(Color.FromArgb(0));            
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, Color.Black)), 0, 0, xRes, yRes); //To do: Figure out why -40 is nessecary to have the same margin
            g.DrawString(this.menu, new Font("Arial", 20, FontStyle.Bold), new SolidBrush(Color.White), xRes / 2, 20);
            for (int i = 0; i < StringList.Count; i++)
            {
                g.DrawString((string)StringList[i], new Font("Arial", 16), new SolidBrush(Color.White), 0, 60 + i * 60);
            }
            // g.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), xRes / 3, 60, xRes / 2, 25);
            //  g.DrawString("Resume", new Font("Arial", 16), new SolidBrush(Color.White), 0, 60);
            //Pen boldPen = new Pen(Color.White, 5);
            //g.DrawLine(boldPen, xRes - 70, 30, xRes - 30, 70);
            //g.DrawLine(boldPen, xRes - 70, 70, xRes - 30, 30);
            return this.buffer;
        }
    }

}
