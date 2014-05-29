using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Controls.Style
{
    public class BasicStylePart : StylePart
    {
        private Style _style = new Style();
        public override Style Style
        {
            get { return _style; }
        }
    }
}
