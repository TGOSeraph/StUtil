using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Animation
{
    public class Animator
    {
        public event EventHandler Animated;

        private Thread worker;
        public int Steps { get; private set; }
        public TimeSpan Duration { get; private set; }
        public int Interval { get; set; }
        public List<Transition> Transitions { get; private set; }
        public Control Invoker { get; private set; }

        public Animator(TimeSpan duration, Control invoker = null)
        {
            this.Invoker = invoker;
            this.Duration = duration;
            this.Interval = 13;
            this.Transitions = new List<Transition>();
            this.Steps = (int)Math.Ceiling(Duration.TotalMilliseconds / (Interval + 3));
        }

        public void Start()
        {
            if (worker != null)
            {
                Stop();
            }
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
        }

        public void AddTransition(AnimatedElement element, Func<AnimatedElement, object> computeFinish)
        {
            this.Transitions.Add(new Transition(element, this.Steps, o => element.Member.Get(), computeFinish));
        }

        public void AddTransition(AnimatedElement element, object finish)
        {
            this.Transitions.Add(new Transition(element, this.Steps, o => element.Member.Get(), finish));
        }

        public void AddTransition(AnimatedElement element, object start, object finish)
        {
            this.Transitions.Add(new Transition(element, this.Steps, start, finish));
        }

        protected virtual void Animate()
        {
            int interval = Interval;
            double steps = Duration.TotalMilliseconds / (interval + 3);
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            int subt = 0;

            var transitions = this.Transitions.ToDictionary(t => t, t => t.Values.GetEnumerator());

            for (int i = 0; i < steps; i++)
            {
                sw.Reset();
                sw.Start();

                foreach (var kvp in transitions)
                {
                    kvp.Value.MoveNext();
                    Transition trans = kvp.Key;
                    if (Invoker == null)
                    {
                        trans.Element.Member.Set(kvp.Value.Current);
                    }
                    else
                    {
                        Invoker.BeginInvoke((Action)delegate()
                        {
                            trans.Element.Member.Set(kvp.Value.Current);
                        });
                    }
                }

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
            worker = null;
            if (Animated != null) Animated(this, EventArgs.Empty);
        }

    }
}
