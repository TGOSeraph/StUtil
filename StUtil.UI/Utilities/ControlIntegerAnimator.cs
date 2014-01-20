using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Utilities
{
    public class ControlIntegerAnimator : StUtil.UI.Utilities.ControlPropertyAnimator<int>
    {
        private double step;
        private double progress;

        public ControlIntegerAnimator(Control ctrl, System.Linq.Expressions.Expression<Func<int>> property)
        {
            this.Instance = ctrl;
            base.SetProperty(property);

        }

        protected override void PerformProcess()
        {
            int v = EndValue - StartValue;

            step = v / (double)Steps;

            progress = StartValue;

            base.PerformProcess();
        }

        protected override int ComputeStep(int step)
        {
            progress += this.step;
            return (int)progress;
        }
    }
}
