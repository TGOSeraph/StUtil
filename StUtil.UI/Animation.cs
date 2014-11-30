using StUtil.Core;
using StUtil.UI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI
{
    public class Animation
    {
        public readonly static TimeSpan Slow = TimeSpan.FromMilliseconds(300);

        public enum Easing
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

        public event EventHandler Animated;

        public Easing EasingType { get; set; }
        public int Interval { get; set; }

        public bool IsRunning { get; private set; }
        public TimeSpan Duration { get; private set; }
        public double StartValue { get; private set; }
        public double EndValue { get; private set; }

        private Action<double> progressed;
        private double p;
        private double s = 1.70158;
        private Thread worker;

        public Animation(Easing easing)
        {
            EasingType = easing;
            Interval = 13;
        }
        public Animation()
        {
            EasingType = Easing.Linear;
            Interval = 13;
        }

        public double PerformStep(double currentTime, double startValue, double change, double duration)
        {
            switch (EasingType)
            {
                case Easing.Linear: return Linear(currentTime, startValue, change, duration);
                case Easing.EaseInQuad: return EaseInQuad(currentTime, startValue, change, duration);
                case Easing.EaseOutQuad: return EaseOutQuad(currentTime, startValue, change, duration);
                case Easing.EaseInOutQuad: return EaseInOutQuad(currentTime, startValue, change, duration);
                case Easing.EaseInCubic: return EaseInCubic(currentTime, startValue, change, duration);
                case Easing.EaseOutCubic: return EaseOutCubic(currentTime, startValue, change, duration);
                case Easing.EaseInOutCubic: return EaseInOutCubic(currentTime, startValue, change, duration);
                case Easing.EaseInQuart: return EaseInQuart(currentTime, startValue, change, duration);
                case Easing.EaseOutQuart: return EaseOutQuart(currentTime, startValue, change, duration);
                case Easing.EaseInOutQuart: return EaseInOutQuart(currentTime, startValue, change, duration);
                case Easing.EaseInQuint: return EaseInQuint(currentTime, startValue, change, duration);
                case Easing.EaseOutQuint: return EaseOutQuint(currentTime, startValue, change, duration);
                case Easing.EaseInOutQuint: return EaseInOutQuint(currentTime, startValue, change, duration);
                case Easing.EaseInSine: return EaseInSine(currentTime, startValue, change, duration);
                case Easing.EaseOutSine: return EaseOutSine(currentTime, startValue, change, duration);
                case Easing.EaseInOutSine: return EaseInOutSine(currentTime, startValue, change, duration);
                case Easing.EaseInExpo: return EaseInExpo(currentTime, startValue, change, duration);
                case Easing.EaseOutExpo: return EaseOutExpo(currentTime, startValue, change, duration);
                case Easing.EaseInOutExpo: return EaseInOutExpo(currentTime, startValue, change, duration);
                case Easing.EaseInCirc: return EaseInCirc(currentTime, startValue, change, duration);
                case Easing.EaseOutCirc: return EaseOutCirc(currentTime, startValue, change, duration);
                case Easing.EaseInOutCirc: return EaseInOutCirc(currentTime, startValue, change, duration);
                case Easing.EaseInElastic: return EaseInElastic(currentTime, startValue, change, duration);
                case Easing.EaseOutElastic: return EaseOutElastic(currentTime, startValue, change, duration);
                case Easing.EaseInOutElastic: return EaseInOutElastic(currentTime, startValue, change, duration);
                case Easing.EaseInBack: return EaseInBack(currentTime, startValue, change, duration);
                case Easing.EaseOutBack: return EaseOutBack(currentTime, startValue, change, duration);
                case Easing.EaseInOutBack: return EaseInOutBack(currentTime, startValue, change, duration);
                case Easing.EaseInBounce: return EaseInBounce(currentTime, startValue, change, duration);
                case Easing.EaseOutBounce: return EaseOutBounce(currentTime, startValue, change, duration);
                case Easing.EaseInOutBounce: return EaseOutBounce(currentTime, startValue, change, duration);
            }
            return 0;
        }
        public void Start(double startValue, double endValue, int steps, Action<double> progress)
        {
            Start(startValue, endValue, TimeSpan.FromMilliseconds(steps / (double)Interval), progress);
        }

        public void Start(double startValue, double endValue, double durationMs, Action<double> progress)
        {
            Start(startValue, endValue, TimeSpan.FromMilliseconds(durationMs), progress);
        }

        public void Start(double startValue, double endValue, TimeSpan duration, Action<double> progress)
        {
            Duration = duration;
            progressed = progress;
            StartValue = startValue;
            EndValue = endValue;
            if (worker != null)
            {
                Stop();
            }
            IsRunning = true;

            worker = new Thread(Animate);
            worker.IsBackground = true;
            worker.Start();
        }

        public void Stop()
        {
            try
            {
                worker.Abort();
            }
            catch (Exception)
            {
            }
            worker = null;
            IsRunning = false;
        }

        protected virtual void Animate()
        {
            int interval = Interval;
            double steps = Duration.TotalMilliseconds / (interval + 3);
            double change = EndValue - StartValue;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            int subt = 0;
            for (int i = 0; i < steps; i++)
            {
                sw.Reset();
                sw.Start();
                progressed(PerformStep(i, StartValue, change, steps));
                sw.Stop();
                int v = (int)(interval - sw.ElapsedMilliseconds);
                if (v < 0)
                {
                    v = Math.Abs(v);
                    subt += v;
                }
                else
                {
                    if (subt > 0)
                    {
                        int sleep = interval - v;
                        if (sleep > 0)
                        {
                            Thread.Sleep(sleep);
                            subt -= sleep;
                        }
                        else
                        {
                            subt -= interval;
                        }
                    }
                    else
                    {
                        Thread.Sleep(interval);
                    }
                    if (subt < 0) subt = 0;
                }
            }
            progressed(PerformStep(1, StartValue, change, 1));
            IsRunning = false;

            if (Animated != null) Animated(this, EventArgs.Empty);
        }

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

        public static Animation Animate(Easing easing, double startValue, double finishValue, TimeSpan duration, Action<double> progress)
        {
            Animation anim = new Animation(easing);
            anim.Start(startValue, finishValue, duration, progress);
            return anim;
        }

        public static Animation Animate(Easing easing, double finishValue, TimeSpan duration, object obj, PropertyAccessor property)
        {
            Animation anim = new Animation(easing);
            anim.Start(Convert.ToDouble(property.Property.GetValue(obj)), finishValue, duration, (v) =>
            {
                property.Property.SetValue(obj, Convert.ChangeType(v, property.Property.PropertyType));
            });
            return anim;
        }

        public static Animation Animate(Easing easing, double finishValue, TimeSpan duration, Control ctrl, PropertyAccessor property)
        {
            Animation anim = new Animation(easing);
            anim.Start(Convert.ToDouble(property.Property.GetValue(ctrl)), finishValue, duration, (v) =>
            {
                try
                {
                    ctrl.BeginInvoke(new MethodInvoker(delegate()
                    {
                        try
                        {
                            property.Property.SetValue(ctrl, Convert.ChangeType(v, property.Property.PropertyType));
                        }
                        catch (Exception)
                        {
                        }
                    }));
                }
                catch (Exception)
                {
                }
            });
            return anim;
        }

        public static Animation Animate(Easing easing, double finishValue, TimeSpan duration, Control ctrl, string property, bool hostControl = false, Action<Control> preProcessControl = null)
        {
            ControlHostForm form = null;
            if (hostControl)
            {
                form = new ControlHostForm(ctrl);
                form.Show();
                ctrl = form;
            }
            if (preProcessControl != null)
            {
                preProcessControl(ctrl);
            }
            Animation anim = Animate(easing, finishValue, duration, ctrl, new PropertyAccessor(ctrl, property));
            if (hostControl)
            {
                anim.Animated += (sender, e) =>
                {
                    ctrl.Invoke((Action)delegate()
                    {
                        form.Close();
                    });
                };
            }
            return anim;
        }

        private static Animation Fade(Control ctrl, double end, TimeSpan? duration = null)
        {
            ControlHostForm form = new ControlHostForm(ctrl);
            form.Opacity = (int)end ^ 1;
            form.Show();
            if (!ctrl.Visible)
            {
                ctrl.Visible = true;
            }
            Animation anim = Fade(form, end, duration);
            anim.Animated += (sender, e) =>
            {
                form.Invoke((Action)delegate()
                {
                    form.Close();
                });
            };
            return anim;
        }

        private static Animation Fade(Form form, double end, TimeSpan? duration)
        {
            return Animate(Easing.Linear, end, duration.HasValue ? duration.Value : Slow, form, new PropertyAccessor(() => form.Opacity));
        }

        public static Animation FadeIn(Control ctrl, TimeSpan? duration = null)
        {
            return Fade(ctrl, 1, duration);
        }

        public static Animation FadeIn(Form form, TimeSpan? duration)
        {
            return Fade(form, 1, duration);
        }

        public static Animation FadeOut(Control ctrl, TimeSpan? duration = null)
        {
            return Fade(ctrl, 0, duration);
        }

        public static Animation FadeOut(Form form, TimeSpan? duration)
        {
            return Fade(form, 0, duration);
        }
    }
}
