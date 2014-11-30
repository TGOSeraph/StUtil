using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for objects implmenting IComparable
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Check if the value lies between a lower (inclusive) and upper (exclusive) bound.
        /// </summary>
        /// <typeparam name="T">The type of object to check</typeparam>
        /// <param name="actual">The value to check</param>
        /// <param name="lowerInclusive">The inclusive lower bound to check from</param>
        /// <param name="upperExclusive">The exclusive upper bound to check to</param>
        /// <returns>True if the value lies between the upper and lower bounds</returns>
        public static bool Between<T>(this T actual, T lowerInclusive, T upperExclusive) where T : IComparable<T>
        {
            return actual.CompareTo(lowerInclusive) >= 0 && actual.CompareTo(upperExclusive) < 0;
        }
    }
}