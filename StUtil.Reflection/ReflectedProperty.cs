using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Wrapper around a property member
    /// </summary>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public class ReflectedProperty : ReflectedMemberReturn<PropertyInfo>
    {
        /// <summary>
        /// Cached Get method
        /// </summary>
        private ReflectedMethod GetMethod;
        /// <summary>
        /// Cached Set method
        /// </summary>
        private ReflectedMethod SetMethod;

        /// <summary>
        /// Create new reflected property wrapper
        /// </summary>
        /// <param name="member">The member to wrap</param>
        public ReflectedProperty(PropertyInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Get the type of the property
        /// </summary>
        /// <returns>The type of the property</returns>
        protected override Type GetReturnType()
        {
            return base.Member.PropertyType;
        }

        /// <summary>
        /// Get the value of the property
        /// </summary>
        /// <param name="target">The object to get the value from</param>
        /// <returns>The value of the property</returns>
        public object Get(object target)
        {
            if (this.GetMethod == null)
            {
                this.GetMethod = new ReflectedMethod(base.Member.GetGetMethod(true));
            }
            return GetMethod.Invoke(target);
        }

        /// <summary>
        /// Get the value of the property
        /// </summary>
        /// <typeparam name="T">The type of the value to return</typeparam>
        /// <param name="target">The object to get the value from</param>
        /// <returns>The value of the property</returns>
        public T Get<T>(object target)
        {
            return (T)Get(target);
        }

        /// <summary>
        /// Set the value of the property
        /// </summary>
        /// <typeparam name="T">The type of the value to set</typeparam>
        /// <param name="target">The object to set the value on</param>
        /// <param name="value">The value to set</param>
        public void Set<T>(object target, T value)
        {
            if (this.SetMethod == null)
            {
                this.SetMethod = new ReflectedMethod(base.Member.GetSetMethod(true));
            }
            this.SetMethod.Invoke(target, new object[] { value });
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedProperty(PropertyInfo propInfo)
        {
            return new ReflectedProperty(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator PropertyInfo(ReflectedProperty propInfo)
        {
            return propInfo.Member;
        }

        public override string ToString()
        {
            return Member.ToString();
        }
    }

}
