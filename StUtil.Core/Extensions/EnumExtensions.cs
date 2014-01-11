using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Enumerable objects
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class EnumExtensions
    {


        /// <summary>
        /// Gets the Description attribute from an enum
        /// </summary>
        /// <typeparam name="T">The type of object to get the description of</typeparam>
        /// <param name="source">The object to get the description of</param>
        /// <returns>The description of the object</returns>
        public static string GetDescription<T>(this T source)
            where T : struct, IConvertible
        {
            return GetAttribute<T, DescriptionAttribute>(source).Description;
        }

        /// <summary>
        /// Get an attribute from an enum
        /// </summary>
        /// <typeparam name="T">The type of the object to get the attribute from</typeparam>
        /// <typeparam name="U">The type of attribute to get</typeparam>
        /// <param name="source">The object to get the attribute from</param>
        /// <returns>The attribute of the specified type on the source object</returns>
        public static U GetAttribute<T, U>(this T source)
            where U : Attribute
            where T : struct, IConvertible
        {
            return GetAttributes<T, U>(source).FirstOrDefault();
        }

        /// <summary>
        /// Get an array attributes from an enum
        /// </summary>
        /// <typeparam name="T">The type of the object to get the attributes from</typeparam>
        /// <typeparam name="U">The type of attributes to get</typeparam>
        /// <param name="source">The object to get the attributes from</param>
        /// <returns>An array of attributes matching the specified type on the object</returns>
        public static U[] GetAttributes<T, U>(this T source)
            where U : Attribute
            where T : struct, IConvertible
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            U[] attributes = (U[])fi.GetCustomAttributes(typeof(U), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the next enum in an enumeration
        /// </summary>
        /// <typeparam name="T">The type of enumeration</typeparam>
        /// <param name="value">The value to get the following element from</param>
        /// <returns>The value int the enum following the specified value</returns>
        public static T? GetNext<T>(this T value)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            bool next = false;
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (next)
                {
                    return item;
                }
                if (item.Equals(value))
                {
                    next = true;
                }
            }
            return null;
        }
    }
}
