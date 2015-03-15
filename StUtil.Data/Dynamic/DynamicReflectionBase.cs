using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Data.Dynamic
{
    public abstract class DynamicReflectionBase : DynamicObject
    {
        private Type targetType;
        protected Type TargetType
        {
            get
            {
                return targetType;
            }
        }

        private object target;
        protected object Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                targetType = target.GetType();
            }
        }

        public DynamicReflectionBase()
        {
        }

        public DynamicReflectionBase(object target)
        {
            Target = target;
        }
    }
}
