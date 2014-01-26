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
            return !(this.Instance.IsDisposed || !this.Instance.IsHandleCreated || this.Instance.Disposing);
        }

        protected override void InvokeUpdate(T value)
        {
            Instance.Invoke((Action)delegate()
            {
                base.UpdateValue(value);
            });
        }
    }

    public class ControlNumericPropertyAnimator<T> : ControlPropertyAnimator<T>
    {
        private double step;
        private double progress;

        public ControlNumericPropertyAnimator(Control ctrl, System.Linq.Expressions.Expression<Func<T>> property)
        {
            this.Instance = ctrl;
            base.SetProperty(property);
        }

        protected override void PerformProcess()
        {

            double v = ((double)Convert.ChangeType(EndValue, typeof(double)) - (double)Convert.ChangeType(StartValue, typeof(double)));

            step = v / Steps;

            progress = (double)Convert.ChangeType(StartValue, typeof(double));

            base.PerformProcess();
        }

        protected override T ComputeStep(int step)
        {
            progress += this.step;
            return ((T)Convert.ChangeType(progress, typeof(T)));
        }
    }
}
