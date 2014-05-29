using StUtil.Extensions;
using StUtil.Generic;
using StUtil.UI.Controls.Style;
using StUtilEx.RegexParser.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StUtilEx.RegexParser.Controls
{
    public class RegexTextBox : StyleRichTextBox
    {
        public event EventHandler<EventArgs<StylePart>> PartSelected;

        private Parser parser = new Parser();
        private bool suppressSelectionChanged = false;

        public RegexTextBox()
            : base()
        {
            base.Parser = new ParserDelegate(Parse);
            base.StopTypingInterval = 0;
            this.SelectionChanged += RegexTextBox_SelectionChanged;
        }

        public IEnumerable<StylePart> Parse(StyleRichTextBox textBox)
        {
            GroupStyle.Id = 0;
            RegexPart part = parser.Parse(textBox.Text);
            return new StylePart[] { ConvertPart(part) };
        }

        public void ResumeSelectionChanged()
        {
            suppressSelectionChanged = false;
        }

        public void SuppressSelectionChanged()
        {
            suppressSelectionChanged = true;
        }

        private StylePart ConvertPart(RegexPart part)
        {
            RegexStylePart p = new RegexStylePart(part) { Index = part.Index, Length = part.ToString().Length };
            foreach (RegexPart c in part.Parts)
            {
                p.Children.Add(ConvertPart(c));
            }
            return p;
        }

        private RegexStylePart GetPartAtIndex(int index, IEnumerable<StylePart> parts = null)
        {
            if (parts == null)
            {
                parts = this.parts;
            }
            if (parts == null)
            {
                return null;
            }
            RegexStylePart part = parts.FirstOrDefault() as RegexStylePart;
            if (part == null)
            {
                return null;
            }
            while (true)
            {
                bool found = false;
                foreach (RegexStylePart child in part.Children)
                {
                    if (child.Index <= index && child.Index + child.Part.ToString().Length > index)
                    {
                        found = true;
                        part = child;
                        break;
                    }
                }
                if (!found)
                {
                    return part;
                }
            }
        }

        private RegexStylePart GetPartUnderMouse()
        {
            if (parts == null)
            {
                return null;
            }
            Point pt = this.PointToClient(Cursor.Position);
            int index = this.GetCharIndexFromPosition(pt);
            return GetPartAtIndex(index);
        }

        private void RegexTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (!highlighting && !suppressSelectionChanged)
            {
                StylePart p = GetPartAtIndex(SelectionStart);
                if (p != null)
                {
                    ((Action)delegate()
                    {
                        PartSelected.RaiseEvent(this, p);
                    }).MakeSafe(this).DelayedInvoke(70);
                }
            }
        }
    }
}