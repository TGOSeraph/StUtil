using StUtil.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.UI.Controls
{
    public class RichTextBoxEx : RichTextBox
    {
        public event EventHandler UserStoppedTyping;
        private DelayedInvokeState state;
        public int StopTypingInterval { get; set; }

        public RichTextBoxEx()
        {
            if (!this.InDesignMode())
            {
                this.TextChanged += RichTextBoxEx_TextChanged;
            }
            this.StopTypingInterval = 250;
        }

        private string lastText = null;
        private object changeLock = new object();
        private void RichTextBoxEx_TextChanged(object sender, EventArgs e)
        {
            lock (changeLock)
            {
                if (lastText == Text)
                {
                    return;
                }
                else
                {
                    lastText = Text;
                }
            }
            if (state != null && !state.HasReturned && !state.HasAborted)
            {
                state.Abort();
            }
            state = ((Action)(delegate()
            {
                if (!justCalled)
                {
                    this.Invoke((Action)delegate()
                    {
                        justCalled = true;
                        OnUserStoppedTyping();
                    });
                }
                else
                {
                    justCalled = false;
                }
                state = null;
            }))
            .MakeSafe(this)
            .DelayedInvoke(StopTypingInterval);
        }

        bool justCalled = false;
        protected virtual void OnUserStoppedTyping()
        {
            UserStoppedTyping.RaiseEvent(this);
        }
    }
}