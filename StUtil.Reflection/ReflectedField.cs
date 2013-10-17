using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Wrapper around a field member
    /// </summary>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public class ReflectedField : ReflectedMemberReturn<FieldInfo>
    {
        /// <summary>
        /// Create new reflected field wrapper
        /// </summary>
        /// <param name="member">The member to wrap</param>
        public ReflectedField(FieldInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Get the type of the field
        /// </summary>
        /// <returns>The type of the field</returns>
        protected override Type GetReturnType()
        {
            return base.Member.FieldType;
        }

        /// <summary>
        /// Get the value of the field
        /// </summary>
        /// <param name="target">The object to get the value from</param>
        /// <returns>The value of the field</returns>
        public object Get(object target)
        {
            return base.Member.GetValue(target);
        }

        /// <summary>
        /// Get the value of the field
        /// </summary>
        /// <typeparam name="T">The type of the value to return</typeparam>
        /// <param name="target">The object to get the value from</param>
        /// <returns>The value of the field</returns>
        public T Get<T>(object target)
        {
            return (T)Get(target);
        }

        /// <summary>
        /// Set the value of the field
        /// </summary>
        /// <typeparam name="T">The type of the value to set</typeparam>
        /// <param name="target">The object to set the value on</param>
        /// <param name="value">The value to set</param>
        public void Set<T>(object target, T value)
        {
            base.Member.SetValue(target, value);
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedField(FieldInfo propInfo)
        {
            return new ReflectedField(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator FieldInfo(ReflectedField propInfo)
        {
            return propInfo.Member;
        }
    }

}
