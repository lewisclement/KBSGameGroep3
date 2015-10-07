using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace KBSGame
{ 
    public class Menu : Panel
    {    
        private Panel p;

        public Menu(MainWindow f)
        {
            p = new Panel();
            p.Height = 200;
            p.Width = 200;
            p.Visible = true;
            p.Location = new Point(
            f.ClientSize.Width / 2 - p.Size.Width / 2,
            f.ClientSize.Height / 2 - p.Size.Height / 2);
            p.Anchor = AnchorStyles.None;
            f.Controls.Add(p);
            Button b1 = new Button();
            b1.Text = "hoi";
            b1.Visible = true;
            p.Controls.Add(b1);

        }
        public void transparent(Boolean t)
        {
            if (t == true)
            {
              //  this.
            }
            if (t == false)
            {
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            }
        }
    }
}
