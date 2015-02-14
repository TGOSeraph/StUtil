using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Animation
{
    public class NumericTweener : Tweener<double>
    {
        public static EasingAlgorithm DefaultEasing = EasingAlgorithm.EaseInOutQuad;

        public NumericTweener(EasingAlgorithm easing)
            : base(easing)
        {
        }
        public NumericTweener()
            : base(DefaultEasing)
        {
        }

        public override IEnumerable<double> ComputeValues(int steps, double start, double finish)
        {
            bool less = start > finish;
            return Enumerable.Range(0, steps).Select(i =>
            {
                if (i == steps - 1)
                {
                    return finish;
                }

                double val = PerformStep(i, start, finish - start, steps);
                if (less)
                {
                    if (val < finish)
                    {
                        val = finish;
                    }
                }
                else
                {
                    if (val > finish)
                    {
                        val = finish;
                    }
                }
                return val;
            });
        }
    }
}
