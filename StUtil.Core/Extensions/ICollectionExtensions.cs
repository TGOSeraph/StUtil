using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Extensions
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Returns the collection as an enumerable
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable AsEnumerable(this ICollection collection)
        {
            return collection.OfType<object>();
        }

        /// <summary>
        /// Returns the collection as an enumerable
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this ICollection collection)
        {
            return collection.OfType<T>();
        }

        /// <summary>
        /// Returns the collection as an enumerable
        /// </summary>
        /// <typeparam name="T">The type of the collection</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this ICollection<T> collection)
        {
            return collection.OfType<T>();
        }
    }
}