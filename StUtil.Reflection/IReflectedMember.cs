using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Interface used for ReflectedMemberInfo's
    /// </summary>
    /// <remarks>
    /// 2013-04-16  - Initial version
    /// </remarks>
    public interface IReflectedMember
    {
        /// <summary>
        /// The type of the member
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// The member object
        /// </summary>
        object Member { get; }
    }

    /// <summary>
    /// Interface used for ReflectedMemberInfo's
    /// </summary>
    /// <typeparam name="T">The type of the relfected member</typeparam>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public interface IReflectedMember<T> : IReflectedMember where T : MemberInfo
    {
        /// <summary>
        /// The type of the member
        /// </summary>
        new Type MemberType { get; }

        /// <summary>
        /// The member object
        /// </summary>
        new T Member { get; }
    }
}
