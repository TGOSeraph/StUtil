using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Extensions
{
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

        /// <summary>
        /// Create a new instance of this type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <returns>An instance of the specified type</returns>
        public static T CreateInstance<T>(this Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Create a new instance of this type
        /// </summary>
        /// <typeparam name="T">The return type of the object</typeparam>
        /// <param name="type">The type of object to create</param>
        /// <param name="args">The arguments to pass to the constructor</param>
        /// <returns>An instance of the specified type</returns>
        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            return (T)Activator.CreateInstance(type, args);
        }
    }
}