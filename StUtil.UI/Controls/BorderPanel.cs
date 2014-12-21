using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class BorderPanel : FlickerFree.Panel
    {
        private Padding borderThickness;
        public Padding BorderThickness
        {
            get
            {
                return borderThickness;
            }
            set
            {
                borderThickness = value;
                this.Refresh();
            }
        }

        private BorderStyle borderStyle;
        public new BorderStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                this.borderStyle = value;
                if (value == System.Windows.Forms.BorderStyle.Fixed3D)
                {
                    base.BorderStyle = value;
                }
                else
                {
                    base.BorderStyle = System.Windows.Forms.BorderStyle.None;
                }
                this.Refresh();
            }
        }

        private Brush borderBrush;
        public Brush BorderBrush
        {
            get
            {
                return borderBrush;
            }
            set
            {
                borderBrush = value;
                this.Refresh();
            }
        }

        public Color BorderColor
        {
            set
            {
                if (borderBrush != null)
                {
                    borderBrush.Dispose();
                }
                borderBrush = new SolidBrush(value);
                this.Refresh();
            }
            get
            {
                if (borderBrush is SolidBrush)
                {
                    return ((SolidBrush)borderBrush).Color;
                }
                else
                {
                    return Color.Transparent;
                }
            }
        }

        public BorderPanel()
        {
            this.BorderColor = Color.Black;
            this.BorderThickness = new Padding(1);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            if (this.BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
            {
                e.Graphics.FillRectangle(borderBrush, new Rectangle(0, 0, BorderThickness.Left, this.Height));
                e.Graphics.FillRectangle(borderBrush, new Rectangle(this.Width - BorderThickness.Right, 0, BorderThickness.Right, this.Height));
                e.Graphics.FillRectangle(borderBrush, new Rectangle(0, 0, this.Width, BorderThickness.Top));
                e.Graphics.FillRectangle(borderBrush, new Rectangle(0, this.Height - BorderThickness.Bottom, this.Width, BorderThickness.Bottom));
            }
        }
    }
}
