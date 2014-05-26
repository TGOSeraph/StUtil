using StUtil.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class UISymbolLabel : Theme.ThemeLabel
    {
        private UISymbol symbol;
        public UISymbol Symbol
        {
            set
            {
                symbol = value;
                this.Text = Char.ConvertFromUtf32((int)value);
            }
            get
            {
                return symbol;
            }
        }

        public bool Bold
        {
            get
            {
                return this.Font.Bold;
            }
            set
            {
                this.Font = new Font(this.Font, value ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        public float FontSize
        {
            get
            {
                return this.Font.Size;
            }
            set
            {
                this.Font = new Font(this.Font.FontFamily, value, this.Font.Style);
            }
        }

        public UISymbolLabel()
        {
            this.Font = new Font("Segoe UI Symbol", this.Font.Size, FontStyle.Regular);
        }
    }
}
