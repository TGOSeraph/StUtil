using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Utilities
{
    public abstract class ControlPropertyAnimator<T> : PropertyAnimator<T>
    {
        public new Control Instance
        {
            get
            {
                return (Control)base.Instance;
            }
            set
            {
                base.Instance = value;
            }
        }

        protected override bool IsValidState()
        {
            return !(this.Instance.IsDisposed || !this.Instance.IsHandleCreated || this.Instance.Disposing || (this.Instance.Parent != null && this.Instance.Parent.IsDisposed || this.Instance.Parent.Disposing || !this.Instance.Parent.IsHandleCreated));
        }

        protected override void InvokeUpdate(T value)
        {
            Instance.Invoke((Action)delegate()
            {
                base.UpdateValue(value);
            });
        }
    }
}
