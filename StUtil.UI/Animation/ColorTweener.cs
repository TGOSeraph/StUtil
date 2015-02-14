using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Animation
{
    public class ColorTweener : Tweener<Color>
    {
        public static EasingAlgorithm DefaultEasing = EasingAlgorithm.EaseInOutQuad;

        public ColorTweener(EasingAlgorithm easing)
            : base(easing)
        {
        }
        public ColorTweener()
            : base(DefaultEasing)
        {
        }

        private int Clamp(double value)
        {
            return double.IsNaN(value) || double.IsPositiveInfinity(value) ? 255 :
                double.IsNegativeInfinity(value) ? 0 :
                (int)(value > 255 ? 255 : value < 0 ? 0 : value);
        }
        public override IEnumerable<Color> ComputeValues(int steps, Color start, Color finish)
        {
            return Enumerable.Range(0, steps).Select(i => Color.FromArgb(
                (int)Clamp(PerformStep(i, start.A, finish.A - start.A, steps)),
                (int)Clamp(PerformStep(i, start.R, finish.R - start.R, steps)),
                (int)Clamp(PerformStep(i, start.G, finish.G - start.G, steps)),
                (int)Clamp(PerformStep(i, start.B, finish.B - start.B, steps))));
        }
    }
}
