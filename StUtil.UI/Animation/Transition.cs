using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Animation
{
    public class Transition
    {
        public AnimatedElement Element { get; private set; }
        public int Steps { get; private set; }
       
        private object start;
        private Func<AnimatedElement, object> computeStart;

        public object Start
        {
            get
            {
                if (start == null)
                {
                    if (computeStart != null)
                    {
                        start = computeStart(Element);
                    }
                }
                return start;
            }
        }
        private object finish;
        private Func<AnimatedElement, object> computeFinish;

        public object Finish
        {
            get
            {
                if (finish == null)
                {
                    if (computeFinish != null)
                    {
                        finish = computeFinish(Element);
                    }
                }
                return finish;
            }
        }

        private IEnumerable values;

        public IEnumerable Values
        {
            get
            {
                if (values == null)
                {
                    values = Element.Tweening.ComputeValues(Steps, Start, Finish);
                }
                return values;
            }
        }

        public Transition(AnimatedElement animation, int steps, Func<AnimatedElement, object> computeStart, Func<AnimatedElement, object> computeFinish)
        {
            this.Element = animation;
            this.Steps = steps;
            this.computeStart = computeStart;
            this.computeFinish = computeFinish;
        }

        public Transition(AnimatedElement animation, int steps, Func<AnimatedElement, object> computeStart, object finish)
        {
            this.Element = animation;
            this.Steps = steps;
            this.computeStart = computeStart;
            this.finish = finish;
        }

        public Transition(AnimatedElement animation, int steps, object start, Func<AnimatedElement, object> computeFinish)
        {
            this.Element = animation;
            this.Steps = steps;
            this.start = start;
            this.computeFinish = computeFinish;
        }

        public Transition(AnimatedElement animation, int steps, object start, object finish)
        {
            this.Element = animation;
            this.Steps = steps;
            this.start = start;
            this.finish = finish;
        }

        public Transition(AnimatedElement animation, int steps, object start)
            : this(animation, steps, start, animation.DefaultValue) { }

        public Transition(AnimatedElement animation, int steps)
            : this(animation, steps, animation.Member.Get(), animation.DefaultValue) { }

    }
}
