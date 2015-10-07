using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KBSGame
{
    public partial class Gui : Form
    {
        private int ID;
        private Boolean active;
        private Bitmap buffer;
        public Gui(int ScreenresX, int ScreenresY)
        {
            this.active = true;
            Text = "KBSGame";
            Height = ScreenresY;
            Width = ScreenresX;
        }
        public virtual Bitmap getRender()
        {
            return this.buffer;
        }

        public virtual Point getInput(MouseEventHandler k)
        {
            var relativePoint = this.PointToClient(Cursor.Position);
            return relativePoint;
           
        }
        // processes keyboard input
        public virtual void getInput(KeyEventHandler key)
        {
        }



        private void Gui_Load(object sender, EventArgs e)
        {

        }
    }
}
