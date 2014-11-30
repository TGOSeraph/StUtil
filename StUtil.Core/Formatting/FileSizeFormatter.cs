using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Formatting
{
    public static class FileSizeFormatter
    {
        /// <summary>
        /// List of size units
        /// </summary>
        public static IList<string> SizeUnits = new List<string>()
        {
            "B", "KB", "MB", "GB", "TB"
        };

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(double bytes, int precision = 2)
        {
            double pow = Math.Floor((bytes > 0 ? Math.Log(bytes) : 0) / Math.Log(1024));
            pow = Math.Min(pow, SizeUnits.Count - 1);
            double value = (double)bytes / Math.Pow(1024, pow);
            return value.ToString(pow == 0 ? "F0" : "F" + precision.ToString()) + SizeUnits[(int)pow];
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(int bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(uint bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(long bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(ulong bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(short bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(ushort bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(byte bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(sbyte bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(float bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }

        /// <summary>
        /// Formats the specified bytes into a file size string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="precision">The precision.</param>
        /// <returns>The string representation of the bytes</returns>
        public static string Format(decimal bytes, int precision = 2)
        {
            return Format((double)bytes, precision);
        }
    }
}