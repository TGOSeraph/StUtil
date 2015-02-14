using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Animation
{
    public abstract class Tweener
    {
        public enum EasingAlgorithm
        {
            Linear,
            EaseInQuad,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInSine,
            EaseOutSine,
            EaseInOutSine,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce
        }

        public EasingAlgorithm Easing { get; set; }


        public Tweener()
            : this(EasingAlgorithm.Linear)
        {
        }

        public Tweener(EasingAlgorithm easing)
        {
            this.Easing = easing;
        }

        public abstract IEnumerable ComputeValues(int steps, object start, object finish);

        protected double PerformStep(double currentTime, double startValue, double change, double duration)
        {
            switch (Easing)
            {
                case EasingAlgorithm.Linear: return Linear(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInQuad: return EaseInQuad(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutQuad: return EaseOutQuad(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutQuad: return EaseInOutQuad(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInCubic: return EaseInCubic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutCubic: return EaseOutCubic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutCubic: return EaseInOutCubic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInQuart: return EaseInQuart(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutQuart: return EaseOutQuart(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutQuart: return EaseInOutQuart(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInQuint: return EaseInQuint(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutQuint: return EaseOutQuint(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutQuint: return EaseInOutQuint(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInSine: return EaseInSine(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutSine: return EaseOutSine(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutSine: return EaseInOutSine(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInExpo: return EaseInExpo(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutExpo: return EaseOutExpo(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutExpo: return EaseInOutExpo(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInCirc: return EaseInCirc(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutCirc: return EaseOutCirc(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutCirc: return EaseInOutCirc(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInElastic: return EaseInElastic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutElastic: return EaseOutElastic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutElastic: return EaseInOutElastic(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInBack: return EaseInBack(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutBack: return EaseOutBack(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutBack: return EaseInOutBack(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInBounce: return EaseInBounce(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseOutBounce: return EaseOutBounce(currentTime, startValue, change, duration);
                case EasingAlgorithm.EaseInOutBounce: return EaseOutBounce(currentTime, startValue, change, duration);
            }
            return 0;
        }

        #region Easing
        private double p;
        private double s = 1.70158;

        private double Linear(double t, double b, double c, double d)
        {
            return c * t / d + b;
        }

        private double EaseInQuad(double t, double b, double c, double d)
        {
            return c * (t /= d) * t + b;
        }

        private double EaseOutQuad(double t, double b, double c, double d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        private double EaseInOutQuad(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t + b;
            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        private double EaseInCubic(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t + b;
        }

        private double EaseOutCubic(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        private double EaseInOutCubic(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        private double EaseInQuart(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        private double EaseOutQuart(double t, double b, double c, double d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        private double EaseInOutQuart(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        private double EaseInQuint(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        private double EaseOutQuint(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        private double EaseInOutQuint(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        private double EaseInSine(double t, double b, double c, double d)
        {
            return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }

        private double EaseOutSine(double t, double b, double c, double d)
        {
            return c * Math.Sin(t / d * (Math.PI / 2)) + b;
        }

        private double EaseInOutSine(double t, double b, double c, double d)
        {
            return -c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b;
        }

        private double EaseInExpo(double t, double b, double c, double d)
        {
            return (t == 0) ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b;
        }

        private double EaseOutExpo(double t, double b, double c, double d)
        {
            return (t == d) ? b + c : c * (-Math.Pow(2, -10 * t / d) + 1) + b;
        }

        private double EaseInOutExpo(double t, double b, double c, double d)
        {
            if (t == 0) return b;
            if (t == d) return b + c;
            if ((t /= d / 2) < 1) return c / 2 * Math.Pow(2, 10 * (t - 1)) + b;
            return c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b;
        }

        private double EaseInCirc(double t, double b, double c, double d)
        {
            return -c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        private double EaseOutCirc(double t, double b, double c, double d)
        {
            return c * Math.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        private double EaseInOutCirc(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return -c / 2 * (Math.Sqrt(1 - t * t) - 1) + b;
            return c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        private double EaseInElastic(double t, double b, double c, double d)
        {
            float s = 1.70158f;
            if (p == 0) p = d * .3;
            double a = c;
            if (t == 0) return b; if ((t /= d) == 1) return b + c;
            if (a < Math.Abs(c)) { a = c; s = (float)(p / 4); }
            else s = (float)(p / (2 * Math.PI) * Math.Asin(c / a));
            return (double)-(a * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
        }

        private double EaseOutElastic(double t, double b, double c, double d)
        {
            var s = 1.70158; var a = c;
            if (t == 0) return b; if ((t /= d) == 1) return b + c; if (p == 0) p = d * .3;
            if (a < Math.Abs(c)) { a = c; s = p / 4; }
            else s = p / (2 * Math.PI) * Math.Asin(c / a);
            return a * Math.Pow(2, -10 * t) * Math.Sin((t * d - s) * (2 * Math.PI) / p) + c + b;
        }

        private double EaseInOutElastic(double t, double b, double c, double d)
        {
            var a = c;
            if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (p == 0) p = d * (.3 * 1.5);
            if (a < Math.Abs(c)) { a = c; s = p / 4; }
            else s = p / (2 * Math.PI) * Math.Asin(c / a);
            if (t < 1) return -.5 * (a * Math.Pow(2, 10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p)) + b;
            return a * Math.Pow(2, -10 * (t -= 1)) * Math.Sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
        }

        private double EaseInBack(double t, double b, double c, double d)
        {
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        }

        private double EaseOutBack(double t, double b, double c, double d)
        {
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }

        private double EaseInOutBack(double t, double b, double c, double d)
        {
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        }

        private double EaseInBounce(double t, double b, double c, double d)
        {
            return c - EaseOutBounce(d - t, 0, c, d) + b;
        }

        private double EaseOutBounce(double t, double b, double c, double d)
        {
            if ((t /= d) < (1 / 2.75))
            {
                return c * (7.5625 * t * t) + b;
            }
            else if (t < (2 / 2.75))
            {
                return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
            }
            else
            {
                return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
            }
        }

        private double EaseInOutBounce(double t, double b, double c, double d)
        {
            if (t < d / 2) return EaseInBounce(t * 2, 0, c, d) * .5 + b;
            return EaseOutBounce(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
        }
        #endregion
    }

    public abstract class Tweener<T> : Tweener
    {
        public Tweener(EasingAlgorithm easing)
            : base(easing)
        {
        }
        public Tweener()
        {
        }

        public abstract IEnumerable<T> ComputeValues(int steps, T start, T finish);

        public override IEnumerable ComputeValues(int steps, object start, object finish)
        {
            return ComputeValues(steps, (T)Convert.ChangeType(start, typeof(T)), (T)Convert.ChangeType(finish, typeof(T)));
        }
    }
}
