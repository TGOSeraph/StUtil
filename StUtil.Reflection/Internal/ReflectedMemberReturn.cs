using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Wrapper for a member that returns a value
    /// </summary>
    /// <typeparam name="T">The type of the member to wrap</typeparam>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public abstract class ReflectedMemberReturn<T> : ReflectedMemberBase<T> where T : MemberInfo
    {
        /// <summary>
        /// The return type
        /// </summary>
        private Type _returnType;
        /// <summary>
        /// The return type
        /// </summary>
        public Type ReturnType
        {
            get
            {
                if (this._returnType == null)
                {
                    this._returnType = GetReturnType();
                }
                return this._returnType;
            }
        }

        /// <summary>
        /// Create new member wrapper
        /// </summary>
        /// <param name="member">The member to wrap</param>
        public ReflectedMemberReturn(T member)
            : base(member)
        {
        }

        /// <summary>
        /// Abstract function to get the return type
        /// </summary>
        /// <returns>The type returned from the member</returns>
        protected abstract Type GetReturnType();

        /// <summary>
        /// Check that the return type of the wrapped member matches
        /// </summary>
        /// <param name="returnType">The type to check</param>
        /// <returns>If the passed in type matches the return type of this member</returns>
        public bool CheckReturnTypeMatch(Type returnType)
        {
            return this.ReturnType == returnType;
        }
    }

}
