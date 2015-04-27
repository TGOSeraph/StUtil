using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Reflection
{
    public abstract class DynamicReflectionBase : DynamicObject
    {
        /// <summary>
        /// The target type
        /// </summary>
        private Type targetType;
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType
        {
            get
            {
                return targetType;
            }
            set
            {
                targetType = value;
            }
        }

        /// <summary>
        /// The target
        /// </summary>
        private object target;
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        protected object Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                if (target != null)
                {
                    targetType = target.GetType();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicReflectionBase"/> class.
        /// </summary>
        public DynamicReflectionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicReflectionBase"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public DynamicReflectionBase(object target)
        {
            Target = target;
        }
    }
}
