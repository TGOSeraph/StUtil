using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Floats
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class FloatExtensions
    {
        /// <summary>
        /// List of size units
        /// </summary>
        public static IList<string> SizeUnits = new List<string>()
        {
            "B", "KB", "MB", "GB", "TB"
        };

        /// <summary>
        /// Formats the value as a filesize in bytes (KB, MB, etc.)
        /// </summary>
        /// <param name="bytes">This value.</param>
        /// <returns>Filesize and quantifier formatted as a string.</returns>
        public static string ToFileSizeFormat(this float bytes, int precision = 2)
        {
            double pow = Math.Floor((bytes > 0 ? Math.Log(bytes) : 0) / Math.Log(1024));
            pow = Math.Min(pow, SizeUnits.Count - 1);
            double value = (double)bytes / Math.Pow(1024, pow);
            return value.ToString(pow == 0 ? "F0" : "F" + precision.ToString()) + SizeUnits[(int)pow];
        }
    }
}
