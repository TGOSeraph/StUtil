using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// A reflected method wrapper
    /// </summary>
    public class ReflectedMethod : ReflectedMemberParametersReturn<MethodInfo>
    {
        /// <summary>
        /// Create a wrapper around a MethodInfo
        /// </summary>
        /// <param name="member">The method to wrap</param>
        public ReflectedMethod(MethodInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Get the parameters of the method
        /// </summary>
        /// <returns>The parameters required by the method</returns>
        protected override IEnumerable<Parameter> GetParameters()
        {
            return base.Member.GetParameters().Select(p => new Parameter(p.Name, p.ParameterType));
        }

        /// <summary>
        /// Gets the return type of the method
        /// </summary>
        /// <returns>The type of object returned by the method</returns>
        protected override Type GetReturnType()
        {
            return base.Member.ReturnType;
        }

        /// <summary>
        /// Invoke the method with no parameters
        /// </summary>
        /// <param name="target">The target to invoke the method on</param>
        /// <returns>The return value of the methods invocation</returns>
        public object Invoke(object target)
        {
            return Invoke<object>(target, new object[] { });
        }

        /// <summary>
        /// Invoke the method
        /// </summary>
        /// <param name="target">The target to invoke the method on</param>
        /// <param name="args">The parameters to pass to the function</param>
        /// <returns>The return value of the methods invocation</returns>
        public object Invoke(object target, object[] args)
        {
            return Invoke<object>(target, args);
        }

        /// <summary>
        /// Invoke the method with no parameters
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="target">The target to invoke the method on</param>
        /// <returns>The return value of the methods invocation</returns>
        public T Invoke<T>(object target)
        {
            return Invoke<T>(target, new object[]{});
        }

        /// <summary>
        /// Invoke the method
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="target">The target to invoke the method on</param>
        /// <param name="args">The parameters to pass to the function</param>
        /// <returns>The return value of the methods invocation</returns>
        public T Invoke<T>(object target, object[] args)
        {
            return (T)base.Member.Invoke(target, args);
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedMethod(MethodInfo propInfo)
        {
            return new ReflectedMethod(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator MethodInfo(ReflectedMethod propInfo)
        {
            return propInfo.Member;
        }
    }

}
