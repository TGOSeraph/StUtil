using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Animation
{
    class FontTweener: Tweener<Font>
    {
        public static EasingAlgorithm DefaultEasing = EasingAlgorithm.EaseInOutQuad;

        public FontTweener(EasingAlgorithm easing)
            : base(easing)
        {
        }
        public FontTweener()
            : base(DefaultEasing)
        {
        }

        private int Clamp(double value)
        {
            return double.IsNaN(value) || double.IsPositiveInfinity(value) ? 255 :
                double.IsNegativeInfinity(value) ? 0 :
                (int)(value > 255 ? 255 : value < 0 ? 0 : value);
        }
       
        public override IEnumerable<Font> ComputeValues(int steps, Font start, Font finish)
        {
            throw new NotImplementedException();
        }

        public override System.Collections.IEnumerable ComputeValues(int steps, object start, object finish)
        {
            Font s = (Font)start;
            double startv = s.Size;
            double finishv;
            if (finish is Font)
            {
                finishv = (double)((Font)finish).Size;
            }
            else
            {
                finishv = (double)Convert.ChangeType(finish, typeof(double));
            }

            bool less = startv > finishv;
            return Enumerable.Range(0, steps).Select(i =>
            {
                if (i == steps - 1)
                {
                    return new Font(s.FontFamily, (float)finishv);
                }

                double val = PerformStep(i, startv, finishv - startv, steps);
                if (less)
                {
                    if (val < finishv)
                    {
                        val = finishv;
                    }
                }
                else
                {
                    if (val > finishv)
                    {
                        val = finishv;
                    }
                }
                return new Font(s.FontFamily, (float)val);
            });
        }
    }
}
