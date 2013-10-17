using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Wrapper around a ConstructorInfo
    /// </summary>
    /// <typeparam name="M">The type of class this constructor is for</typeparam>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public class ReflectedConstructor<M> : ReflectedMemberParameters<ConstructorInfo>
    {
        /// <summary>
        /// Create a new reflected constructor wrapper
        /// </summary>
        /// <param name="member">The reflected constructor</param>
        public ReflectedConstructor(ConstructorInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Gets the type of the parameters for the constructor
        /// </summary>
        /// <returns>A list of the types of the parameters for the constructor</returns>
        protected override IEnumerable<Parameter> GetParameters()
        {
            return base.Member.GetParameters().Select(p => new Parameter(p.Name, p.ParameterType));
        }

        /// <summary>
        /// Creates a new instance of the class by invoking the constructor
        /// </summary>
        /// <param name="args">The parameter objects to pass to the constructor</param>
        /// <returns>An instance of the type represented by this constructor</returns>
        public M Construct(params object[] args)
        {
            return (M)base.Member.Invoke(args);
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedConstructor<M>(ConstructorInfo propInfo)
        {
            return new ReflectedConstructor<M>(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator ConstructorInfo(ReflectedConstructor<M> propInfo)
        {
            return propInfo.Member;
        }
    }

    /// <summary>
    /// Typeless version of the wrapper around a ConstructorInfo
    /// </summary>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public class ReflectedConstructor : ReflectedConstructor<object>
    {
        /// <summary>
        /// Create a new reflected constructor wrapper
        /// </summary>
        /// <param name="member">The reflected constructor</param>
        public ReflectedConstructor(ConstructorInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedConstructor(ConstructorInfo propInfo)
        {
            return new ReflectedConstructor(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator ConstructorInfo(ReflectedConstructor propInfo)
        {
            return propInfo.Member;
        }
    }
}
