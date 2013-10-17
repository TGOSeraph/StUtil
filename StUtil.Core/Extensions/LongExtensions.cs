using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Longs
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class LongExtensions
    {
        /// <summary>
        /// Formats the value as a filesize in bytes (KB, MB, etc.)
        /// </summary>
        /// <param name="bytes">This value.</param>
        /// <returns>Filesize and quantifier formatted as a string.</returns>
        public static string ToFileSizeFormat(this long bytes, int precision = 2)
        {
            return ((float)bytes).ToFileSizeFormat(precision);
        }
    }
}
