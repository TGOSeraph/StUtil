using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Utilities
{
    public class ControlLongAnimator : StUtil.UI.Utilities.ControlPropertyAnimator<long>
    {
        private double step;
        private double progress;

        public ControlLongAnimator(Control ctrl, System.Linq.Expressions.Expression<Func<long>> property)
        {
            this.Instance = ctrl;
            base.SetProperty(property);
        }

        protected override void PerformProcess()
        {
            long v = EndValue - StartValue;

            step = v / (double)Steps;

            progress = StartValue;

            base.PerformProcess();
        }

        protected override long ComputeStep(int step)
        {
            progress += this.step;
            return (long)progress;
        }
    }
}
