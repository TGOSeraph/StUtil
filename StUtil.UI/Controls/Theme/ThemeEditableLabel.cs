using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls.Theme
{
    public class ThemeEditableLabel : StUtil.UI.Controls.EditableLabel, IThemeControl
    {
        private ThemeControlHelper themeHelper;

        private ThemeManager.Style style;
        public ThemeManager.Style Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
                themeHelper.ApplyStyles();
            }
        }

        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                Color? themeColor = ThemeManager.GetThemeColor(Style);
                if (!themeColor.HasValue || themeColor.Value != value)
                {
                    if (Style != ThemeManager.Style.Custom)
                    {
                        Style = ThemeManager.Style.Custom;
                    }
                }
                base.ForeColor = value;
            }
        }

        public ThemeEditableLabel()
        {
            this.themeHelper = new ThemeControlHelper(this);
            this.Style = ThemeManager.Style.Darker;
            this.BackColor = Color.Transparent;
        }

        public Color MainColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }

        public Color SecondaryColor
        {
            get { return this.BackColor; }
            set { }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (!IsEditable && DoubleClickToEdit)
            {
                using (Pen p = new Pen(Theme.ThemeManager.GetThemeColor(ThemeManager.Style.LightDark).Value))
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    e.Graphics.DrawLine(p, 0, 5, 0, this.Height - 5);
                }
            }
        }
    }
}
