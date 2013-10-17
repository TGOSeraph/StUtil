using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// A wrapper around a member with parameters and a return value
    /// </summary>
    /// <typeparam name="T">The type of the wrapped member</typeparam>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public abstract class ReflectedMemberParametersReturn<T> : ReflectedMemberParameters<T> where T : MemberInfo
    {
        /// <summary>
        /// The return type of the member
        /// </summary>
        private Type _returnType;
        /// <summary>
        /// The return type of the member
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
        public ReflectedMemberParametersReturn(T member)
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
            return returnType == null || this.ReturnType == returnType;
        }
    }

}
