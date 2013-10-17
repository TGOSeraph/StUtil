using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Base class for a reflected member wrapper
    /// </summary>
    /// <typeparam name="T">The type of MemberInfo</typeparam>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public abstract class ReflectedMemberBase<T> : IReflectedMember<T> where T : MemberInfo
    {
        /// <summary>
        /// The type of the member
        /// </summary>
        private Type _memberType;
        /// <summary>
        /// The type of the member
        /// </summary>
        public Type MemberType
        {
            get { return this._memberType; }
        }

        /// <summary>
        /// The member object
        /// </summary>
        private T _member;
        /// <summary>
        /// The member object
        /// </summary>
        public T Member
        {
            get { return this._member; }
        }

        /// <summary>
        /// Create new wrapper
        /// </summary>
        /// <param name="member"></param>
        public ReflectedMemberBase(T member)
        {
            this._memberType = typeof(T);
            this._member = member;
        }

        /// <summary>
        /// If this member's name matches
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <returns></returns>
        public bool Equals(string name)
        {
            return this.Member.Name == name;
        }

        /// <summary>
        /// If two reflected member wrappers are identical
        /// </summary>
        /// <param name="member">The other wrapper to compare</param>
        /// <returns></returns>
        public bool Equals(ReflectedMemberBase<T> member)
        {
            return this.Member == member.Member;
        }

        object IReflectedMember.Member
        {
            get { return this._member; }
        }
    }

}
