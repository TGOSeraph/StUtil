using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Controls.Theme
{
    public interface IThemeControl
    {
        ThemeManager.Style Style { get; set; }
        Color BackColor { get; set; }
        Color ForeColor { get; set; }

        Color MainColor { get;  set; }
        Color SecondaryColor { get;  set; }
    }
}
