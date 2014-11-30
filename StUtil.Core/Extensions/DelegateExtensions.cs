using StUtil.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Delegates
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// Invoke a delegate after a set period of time
        /// </summary>
        /// <param name="action">The delegate to invoke</param>
        /// <param name="timeout">The period of time to wait until invoking the method</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <returns>A state object allowing you access to the state of the delayed invoke</returns>
        public static DelayedInvoke DelayedInvoke(this Delegate action, int timeout, object[] args = null)
        {
            return DelayedInvoke(action, timeout, false, args);
        }

        /// <summary>
        /// Invoke a delegate after a set period of time
        /// </summary>
        /// <param name="action">The delegate to invoke</param>
        /// <param name="timeout">The period of time to wait until invoking the method</param>
        /// <param name="blocking">If the method should be invoked on the thread it was called from</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <returns>A state object allowing you access to the state of the delayed invoke</returns>
        public static DelayedInvoke DelayedInvoke(this Delegate action, int timeout, bool blocking, object[] args = null)
        {
            return new DelayedInvoke(action, timeout, blocking, args);
        }

        /// <summary>
        /// Convert a delegate to a thread safe delegate
        /// </summary>
        /// <param name="Action">The delegate to make safe</param>
        /// <param name="owner">The control to invoke the delegate from</param>
        /// <returns>A safe delegate that will be called from the specified control</returns>
        public static Delegate MakeSafe(this Delegate Action, System.Windows.Forms.Control owner)
        {
            return (Action)(() =>
            {
                if (owner.InvokeRequired)
                {
                    owner.Invoke(Action);
                }
                else
                {
                    Action.DynamicInvoke();
                }
            });
        }

        /// <summary>
        /// Convert a delegate to a thread safe delegate
        /// </summary>
        /// <param name="Action">The delegate to make safe</param>
        /// <param name="owner">The control to invoke the delegate from</param>
        /// <param name="args">The arguments to pass to the method</param>
        /// <returns>A safe delegate that will be called from the specified control</returns>
        public static Delegate MakeSafe(this Delegate Action, System.Windows.Forms.Control owner, params object[] args)
        {
            return (Action)(() =>
            {
                if (owner.InvokeRequired)
                {
                    owner.Invoke(Action, args);
                }
                else
                {
                    Action.DynamicInvoke(args);
                }
            });
        }

        /// <summary>
        /// Convert a delegate to a thread safe delegate
        /// </summary>
        /// <param name="Action">The delegate to make safe</param>
        /// <param name="owner">The control to invoke the delegate from</param>
        /// <param name="args">A function that returns a list of parameters to pass when the method is called</param>
        /// <returns>A safe delegate that will be called from the specified control</returns>
        public static Delegate MakeSafe(this Delegate Action, System.Windows.Forms.Control owner, Func<object[]> args)
        {
            return (Action)(() =>
            {
                if (owner.InvokeRequired)
                {
                    owner.Invoke(Action, args());
                }
                else
                {
                    Action.DynamicInvoke(args());
                }
            });
        }

        /// <summary>
        /// Run a delegate for a set period of time
        /// </summary>
        /// <typeparam name="T">The type of the result object from the action</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="milliseconds">How many milliseconds the function should run for</param>
        /// <param name="cancel">A pointer to a boolean specifying if the executions should be stopped</param>
        /// <param name="args">The arguments to pass to the function</param>
        /// <param name="sleep">How long to sleep for between each invocation of the delegate</param>
        /// <returns>A list of each of the results from the delegate invocations</returns>
        public static List<T> RunForXTime<T>(this Delegate action, int milliseconds, ref bool cancel, object[] args = null, int sleep = -1)
        {
            return RunForXTime<T>(action, TimeSpan.FromMilliseconds(milliseconds), ref cancel, args);
        }

        /// <summary>
        /// Run a delegate for a set period of time
        /// </summary>
        /// <typeparam name="T">The type of the result object from the action</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="timespan">How long the function should be looped for</param>
        /// <param name="cancel">A pointer to a boolean specifying if the executions should be stopped</param>
        /// <param name="args">The arguments to pass to the function</param>
        /// <param name="sleep">How long to sleep for between each invocation of the delegate</param>
        /// <returns>A list of each of the results from the delegate invocations</returns>
        public static List<T> RunForXTime<T>(this Delegate action, TimeSpan timespan, ref bool cancel, object[] args = null, int sleep = -1)
        {
            return RunUntilXTime<T>(action, DateTime.Now.Add(timespan), ref cancel, args);
        }

        /// <summary>
        /// Runs a delegate on a new thread
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <param name="arg">The paramater to pass to the delegate</param>
        /// <returns>The newly created thread</returns>
        public static Thread RunOnNewThread(this Delegate action, object arg, bool isBackground = true)
        {
            Thread t = new Thread((ParameterizedThreadStart)action);
            t.Start(arg);
            t.IsBackground = isBackground;
            return t;
        }

        /// <summary>
        /// Run a delegate on a new thread
        /// </summary>
        /// <param name="action">The method to run</param>
        /// <returns>The newly created thread</returns>
        public static Thread RunOnNewThread(this Delegate action)
        {
            Thread t = new Thread(delegate()
            {
                action.DynamicInvoke();
            });
            t.Start();
            return t;
        }

        /// <summary>
        /// Run a delegate until a set time
        /// </summary>
        /// <typeparam name="T">The type of the result object from the action</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="endTime">When the execution loop should end</param>
        /// <param name="cancel">A pointer to a boolean specifying if the executions should be stopped</param>
        /// <param name="args">The arguments to pass to the function</param>
        /// <param name="sleep">How long to sleep for between each invocation of the delegate</param>
        /// <returns>A list of each of the results from the delegate invocations</returns>
        public static List<T> RunUntilXTime<T>(this Delegate action, DateTime endTime, ref bool cancel, object[] args = null, int sleep = -1)
        {
            List<T> results = new List<T>();
            while (DateTime.Now <= endTime && !cancel)
            {
                results.Add((T)action.DynamicInvoke(args));
                if (sleep > -1 && !cancel)
                {
                    Thread.Sleep(sleep);
                }
            }
            return results;
        }

        /// <summary>
        /// Runs a delegate a set number of times
        /// </summary>
        /// <typeparam name="T">The type of the result from the action</typeparam>
        /// <param name="action">The action to execute</param>
        /// <param name="times">The number of times it should be executed</param>
        /// <param name="cancel">A pointer to a boolean specifying if the loop should be stopped</param>
        /// <param name="cancel">A pointer to a boolean specifying if the executions should be stopped</param>
        /// <param name="args">The arguments to pass to the function</param>
        /// <param name="sleep">How long to sleep for between each invocation of the delegate</param>
        /// <returns>A list of each of the results from the delegate invocations</returns>
        public static List<T> RunXTimes<T>(this Delegate action, int times, ref bool cancel, object[] args = null, int sleep = -1)
        {
            List<T> results = new List<T>();
            while (times-- > 0 && !cancel)
            {
                results.Add((T)action.DynamicInvoke(args));
                if (sleep > -1 && !cancel)
                {
                    Thread.Sleep(sleep);
                }
            }
            return results;
        }

        /// <summary>
        /// Invoke a delegate safely on a speficied control
        /// </summary>
        /// <param name="Action">The delegate to execute</param>
        /// <param name="owner">The control to execute on</param>fasdas
        /// <param name="args">The arguments to pass to the delegate</param>
        public static void SafeInvoke(this Delegate Action, System.Windows.Forms.Control owner, params object[] args)
        {
            if (owner.InvokeRequired)
            {
                owner.Invoke(Action, args);
            }
            else
            {
                Action.DynamicInvoke(args);
            }
        }
    }
}