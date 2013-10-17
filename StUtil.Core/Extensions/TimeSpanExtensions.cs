using System;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for TimeSpans
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts a timespan to a readable string
        /// </summary>
        /// <param name="span">The timespan to format</param>
        /// <returns>A formatted string equivalent of the timespan</returns>
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
                span.Days > 0 ? string.Format("{0:0}d ", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0}m ", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0}s ", span.Seconds) : string.Empty,
                span.Milliseconds > 0 ? string.Format("{0:0}ms", span.Milliseconds) : string.Empty);

            //if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            return formatted;
        }

        public static string ToTimeString(this TimeSpan span)
        {
            return String.Format("{0:dd\\.hh\\:mm\\:ss\\:fff}", span).Replace("00.00:", "").Replace("00.", "");
        }

    }
}
