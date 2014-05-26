using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls.Theme
{
    public class ThemeControlHelper
    {
        public IThemeControl Control { get; private set; }

        public ThemeControlHelper(IThemeControl control)
        {
            ThemeManager.ColorizationChanged += ThemeManager_ColorizationChanged;
            this.Control = control;
        }

        private void ThemeManager_ColorizationChanged(object sender, EventArgs e)
        {
            if (Control.Style != ThemeManager.Style.Custom)
            {
                ApplyStyles();
            }
        }

        public void ApplyStyles()
        {
            Control.MainColor = ThemeManager.GetThemeColor(Control.Style, Control.BackColor);
            Control.SecondaryColor = ThemeManager.GetContrasting(Control.BackColor);
        }

    }
}
