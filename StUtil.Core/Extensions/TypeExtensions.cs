using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Types
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class TypeExtensions
    {
        /// <summary>
        /// Create a new instance of this type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <returns>An instance of the specified type</returns>
        public static object CreateInstance(this Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Create a new instance of this type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <param name="args">The arguments to pass to the constructor</param>
        /// <returns>An instance of the specified type</returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }
    }
}
