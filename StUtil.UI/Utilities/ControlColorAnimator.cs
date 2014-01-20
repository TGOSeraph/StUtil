using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Utilities
{
    public class ControlColorAnimator : ControlPropertyAnimator<Color>
    {
        private double stepr, stepg, stepb;
        private double progressr, progressb, progressg;

        protected override void PerformProcess()
        {
            int r = (int)EndValue.R - (int)StartValue.R;
            int g = (int)EndValue.G - (int)StartValue.G;
            int b = (int)EndValue.B - (int)StartValue.B;

            stepr = r / (double)Steps;
            stepg = g / (double)Steps;
            stepb = b / (double)Steps;

            progressr = StartValue.R;
            progressg = StartValue.G;
            progressb = StartValue.B;
            base.PerformProcess();
        }

        public ControlColorAnimator(Control ctrl, Expression<Func<Color>> property)
        {
            this.Instance = ctrl;
            base.SetProperty(property);
        }

        protected override Color ComputeStep(int step)
        {
            progressr += stepr;
            progressb += stepb;
            progressg += stepg;
            return Color.FromArgb((byte)progressr, (byte)progressg, (byte)progressb);
        }
    }
}