using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Represents a wrapper that includes parameters
    /// </summary>
    /// <typeparam name="T">The type of member info object wrapped</typeparam>
    /// <remarks>
    /// 2013-07-16  - Switched from using a Tuple for parameter info to the Parameter class
    /// 2013-04-15  - Initial version
    /// </remarks>
    public abstract class ReflectedMemberParameters<T> : ReflectedMemberBase<T> where T : MemberInfo
    {
        /// <summary>
        /// The parameters of the member
        /// </summary>
        private IEnumerable<Parameter> _parameters;
        /// <summary>
        /// The parameters of the member
        /// </summary>
        public IEnumerable<Parameter> Parameters
        {
            get
            {
                if (this._parameters == null)
                {
                    this._parameters = GetParameters();
                }
                return this._parameters;
            }
        }

        /// <summary>
        /// Create new parameter wrapper
        /// </summary>
        /// <param name="member">The member to wrap</param>
        public ReflectedMemberParameters(T member)
            : base(member)
        {
        }

        /// <summary>
        /// Abstract function to get the parameters
        /// </summary>
        /// <returns>The parameters of the MemberInfo</returns>
        protected abstract IEnumerable<Parameter> GetParameters();

        /// <summary>
        /// Check if the passed in parameters types match the parameters of the wrapped member
        /// </summary>
        /// <param name="parameters">The types to check</param>
        /// <returns>If the parameters matched</returns>
        public bool CheckParametersMatch(params Type[] parameters)
        {
            return parameters == null
                || (parameters.Length == 1 && parameters[0] == typeof(void))
                    ? (this.Parameters == null || this.Parameters.Count() == 0)
                    : (this.Parameters.Count() == parameters.Length && this.Parameters.Select(p => p.Type).SequenceEqual(parameters));
        }

        /// <summary>
        /// Check if the passed in parameters types match the parameters of the wrapped member
        /// </summary>
        /// <param name="parameters">The types to check</param>
        /// <returns>If the parameters matched</returns>
        public bool CheckParametersMatch(params string[] parameters)
        {
            return parameters == null && this.Parameters == null
                || (this.Parameters.Count() == parameters.Length && this.Parameters.Select(p => p.Name).SequenceEqual(parameters));
        }

        /// <summary>
        /// Check if the passed in parameters types match the parameters of the wrapped member
        /// </summary>
        /// <param name="parameters">The objects to check the types of</param>
        /// <returns>If the parameters matched</returns>
        public bool CheckParametersMatch(params object[] parameters)
        {
            return CheckParametersMatch(parameters.Select(p => p.GetType()).ToArray());
        }
    }

}
