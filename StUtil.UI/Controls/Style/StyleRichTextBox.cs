using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;

namespace StUtil.UI.Controls.Style
{
    public class StyleRichTextBox : RichTextBoxEx
    {

        public delegate IEnumerable<StylePart> ParserDelegate(StyleRichTextBox textBox);

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ParserDelegate Parser { get; set; }

        protected bool highlighting = false;
        protected IEnumerable<StylePart> parts;

        public StyleRichTextBox()
        {
            this.UserStoppedTyping += StyleRichTextBox_UserStoppedTyping;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!base.AutoWordSelection)
            {
                base.AutoWordSelection = true;
                base.AutoWordSelection = false;
            }
        }

        private void StyleRichTextBox_UserStoppedTyping(object sender, EventArgs e)
        {
            ApplyStyles();
        }

        protected virtual IEnumerable<StylePart> Parse()
        {
            return Parser.Invoke(this);
        }

        public void ApplyStyles()
        {
            if (Parser != null)
            {
                try
                {
                    this.SuspendDrawing();
                    highlighting = true;
                    parts = Parse();
                    //Left to right, outer to inner
                    parts = parts.OrderBy(p => p.Index).ThenByDescending(p => p.Length);

                    Queue<StylePart> queue = new Queue<StylePart>();
                    foreach (StylePart part in parts)
                    {
                        queue.Enqueue(part);
                    }
                    while (queue.Count > 0)
                    {
                        StylePart part = queue.Dequeue();
                        part.Style.Apply(part, this);
                        foreach (StylePart child in part.Children)
                        {
                            queue.Enqueue(child);
                        }
                    }
                }
                finally
                {
                    highlighting = false;
                    this.ResumeDrawing();
                }
            }
        }
    }
}
