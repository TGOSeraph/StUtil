using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StUtil.Reflection
{
    /// <summary>
    /// Create a pointer to a property of an object
    /// </summary>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <typeparam name="U">The type of the property</typeparam>
    public class PropertyPointer<T, U>
    {
        /// <summary>
        /// The object
        /// </summary>
        private T obj;
        /// <summary>
        /// The property (if applicable)
        /// </summary>
        private PropertyInfo prop;
        /// <summary>
        /// The field (if applicable)
        /// </summary>
        private FieldInfo field;

        /// <summary>
        /// Get or set the value of the property
        /// </summary>
        public U Value
        {
            get
            {
                if (prop != null)
                {
                    return (U)prop.GetValue(obj, null);
                }
                else
                {
                    return (U)field.GetValue(obj);
                }
            }
            set
            {
                if (prop != null)
                {
                    prop.SetValue(obj, value, null);
                }
                else
                {
                    field.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// Create a new property pointer
        /// </summary>
        /// <param name="obj">The object to get the property pointer of</param>
        /// <param name="expr">The property to use as the pointer</param>
        public PropertyPointer(T obj, Expression<Func<T, U>> expr)
        {
            this.obj = obj;
            MemberInfo mem = ((MemberExpression)expr.Body).Member;
            if (mem is PropertyInfo)
            {
                prop = mem as PropertyInfo;
            }
            else
            {
                field = mem as FieldInfo;
            }
        }
    }
}
