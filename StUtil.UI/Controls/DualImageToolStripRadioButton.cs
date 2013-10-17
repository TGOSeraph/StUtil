using StUtil.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Controls
{
    public class DualImageToolStripRadioButton : ToolStripRadioButton
    {
        private Bitmap image2;
        public Bitmap Image2
        {
            get
            {
                return image2;
            }
            set
            {
                image2 = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            if (image2 != null)
            {
                e.Graphics.DrawImage(image2, 0, 0);
            }
        }
    }
}
