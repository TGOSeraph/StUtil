using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Controls.Style
{
    public class Style
    {
        public virtual Color ForeColor { get; set; }
        public bool ForeColorOverrides { get; set; }

        public virtual Color BackColor { get; set; }
        public bool BackColorOverrides { get; set; }

        public virtual bool Bold { get; set; }
        public bool BoldOverrides { get; set; }

        public virtual bool Italic { get; set; }

        public Style()
        {
            ForeColor = Color.Black;
            BackColor = Color.White;
            Bold = false;
        }

        public virtual void Apply(StylePart part, StyleRichTextBox textBox)
        {
            bool foreColorOverriden = false;
            bool backColorOverriden = false;
            bool boldOverriden = false;

            StylePart temp = part.Parent;
            while (temp != null)
            {
                if (temp.Style.BackColorOverrides)
                {
                    backColorOverriden = true;
                }
                if (temp.Style.ForeColorOverrides)
                {
                    foreColorOverriden = true;
                }
                if (temp.Style.BoldOverrides)
                {
                    boldOverriden = true;
                }
                temp = temp.Parent;
            }

            int pos = textBox.SelectionStart;
            textBox.SelectionStart = part.Index;
            textBox.SelectionLength = part.Length;

            Color fgColor = textBox.SelectionColor;
            Color bgColor = textBox.SelectionBackColor;
            Font font = textBox.SelectionFont;

            if (!foreColorOverriden || ForeColorOverrides)
            {
                textBox.SelectionColor = ForeColor;
            }
            if (!backColorOverriden || BackColorOverrides)
            {
                textBox.SelectionBackColor = BackColor;
            }

            FontStyle style = FontStyle.Regular;
            if (!boldOverriden || BoldOverrides)
            {
                if (Bold)
                {
                    style |= FontStyle.Bold;
                }
            }
            if (Italic)
            {
                style |= FontStyle.Italic;
            }

            textBox.SelectionFont = new Font(textBox.SelectionFont, style);

            textBox.SelectionStart = pos;
            textBox.SelectionLength = 0;

            textBox.SelectionColor = fgColor;
            textBox.SelectionBackColor = bgColor;
            textBox.SelectionFont = font;

        }
    }
}
