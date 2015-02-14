using StUtil.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Animation
{
    public class AnimatedElement
    {
        public MemberAccess Member { get; private set; }
        public Tweener Tweening { get; private set; }

        public object DefaultValue { get; set; }

        public AnimatedElement(MemberAccess member)
            : this(member, member.Get())
        {
        }

        public AnimatedElement(MemberAccess member, object defaultValue)
        {
            this.Member = member;
            this.DefaultValue = defaultValue;

            Type t = defaultValue.GetType();
            if (t == typeof(Color))
            {
                this.Tweening = new ColorTweener();
            }
            else if (t == typeof(Font))
            {
                this.Tweening = new FontTweener();
            }
            else
            {
                this.Tweening = new NumericTweener();
            }
        }

        public AnimatedElement(Tweener animator, MemberAccess member)
            : this(animator, member, member.Get())
        {
        }

        public AnimatedElement(Tweener animator, MemberAccess member, object defaultValue)
        {
            this.Tweening = animator;
            this.Member = member;
            this.DefaultValue = defaultValue;
        }
    }
}
