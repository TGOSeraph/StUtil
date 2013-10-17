using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Func's
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class FuncExtensions
    {
        /// <summary>
        /// Run a function for a set amount of time, or until the method returns true
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <param name="milliseconds">The amount of time to run for</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <param name="sleep">The amount of time to sleep for between each call</param>
        /// <returns>If the function returned valid or not</returns>
        public static bool RunForXOrTrue(this Func<object[], bool> action, int milliseconds, object[] args, int sleep = -1)
        {
            return RunForXOrTrue(action, TimeSpan.FromMilliseconds(milliseconds), args);
        }

        /// <summary>
        /// Run a function for a set amount of time, or until the method returns true
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <param name="timespan">The amount of time to run for</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <param name="sleep">The amount of time to sleep for between each call</param>
        /// <returns>If the function returned valid or not</returns>
        public static bool RunForXOrTrue(this Func<object[], bool> action, TimeSpan timespan, object[] args, int sleep = -1)
        {
            return RunUntilXOrTrue(action, DateTime.Now.Add(timespan), args);
        }

        /// <summary>
        /// Run a function until a specific time, or until the method returns true
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <param name="endTime">The time to run until</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <param name="sleep">The amount of time to sleep for between each call</param>
        /// <returns>If the function returned valid or not</returns>
        public static bool RunUntilXOrTrue(this Func<object[], bool> action, DateTime endTime, object[] args, int sleep = -1)
        {
            while (DateTime.Now <= endTime)
            {
                if (action(args))
                    return true;
                if (sleep > -1)
                {
                    Thread.Sleep(sleep);
                }
            }
            return false;
        }

        /// <summary>
        /// Run a function a specific number of times, or until the method returns true
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <param name="endTime">The number of times to run the method</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <param name="sleep">The amount of time to sleep for between each call</param>
        /// <returns>If the function returned valid or not</returns>
        public static bool RunXTimesOrTrue(this Func<object[], bool> action, int times, object[] args, int sleep = -1)
        {
            while (times-- > 0)
            {
                if (action(args))
                    return true;
                if (sleep > -1)
                {
                    Thread.Sleep(sleep);
                }
            }
            return false;
        }
    }
}
