using System;

namespace StUtil.Formatting
{
    public static class TimeSpanFormatter
    {
        /// <summary>
        /// Returns two timespans formatted to show the progress in the format {ts1}/{ts2}
        /// </summary>
        /// <param name="currentTime">The current value</param>
        /// <param name="maxTime">The end value</param>
        /// <param name="includeLetters">If letters such as d,h,m,s should be displayed</param>
        /// <returns>A string with the two timespans formatted and concat'ed</returns>
        public static string ProgressFormat(TimeSpan currentTime, TimeSpan endTime, bool includeLetters = true)
        {
            string start = "";
            string end = "";

            if (endTime.TotalDays > 1)
            {
                start = currentTime.Days.ToString().PadLeft(endTime.Days, '0') + (includeLetters ? "d" : "") + ":";
                end = endTime.Days.ToString() + (includeLetters ? "d" : "") + ":";
            }
            if (endTime.TotalHours > 1)
            {
                start += currentTime.Hours.ToString().PadLeft(2, '0') + (includeLetters ? "h" : "") + ":";
                end += endTime.Hours.ToString().PadLeft(2, '0') + (includeLetters ? "h" : "") + ":";
            }
            if (endTime.TotalMinutes > 1)
            {
                start += currentTime.Minutes.ToString().PadLeft(2, '0') + (includeLetters ? "m" : "") + ":";
                end += endTime.Minutes.ToString().PadLeft(2, '0') + (includeLetters ? "m" : "") + ":";
            }

            start += currentTime.Seconds.ToString().PadLeft(2, '0') + (includeLetters ? "s" : "");
            end += endTime.Seconds.ToString().PadLeft(2, '0') + (includeLetters ? "s" : "");
            return start + "/" + end;
        }

        /// <summary>
        /// Converts a timespan to a readable string
        /// </summary>
        /// <param name="span">The timespan to format</param>
        /// <returns>A formatted string equivalent of the timespan</returns>
        public static string ReadableFormat(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
                span.Days > 0 ? string.Format("{0:0}d ", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0}m ", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0}s ", span.Seconds) : string.Empty,
                span.Milliseconds > 0 ? string.Format("{0:0}ms", span.Milliseconds) : string.Empty);

            return formatted;
        }

        /// <summary>
        /// Convert the timespan to a time formatted string
        /// </summary>
        /// <param name="span">The timespan.</param>
        /// <returns></returns>
        public static string TimeFormat(TimeSpan span)
        {
            return String.Format("{0:dd\\.hh\\:mm\\:ss\\:fff}", span).Replace("00.00:", "").Replace("00.", "").Replace(":000", "");
        }
    }
}