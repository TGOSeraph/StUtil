using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace StUtil.Misc
{
    /// <summary>
    /// A state object for a Delayed Invoke
    /// </summary>
    /// <remarks>
    /// 2013-06-24  - Added HasAborted property
    /// 2013-06-24  - Initial version
    /// </remarks>
    public class DelayedInvokeState
    {
        private Thread thread;

        /// <summary>
        /// If the delayed invoke has completed
        /// </summary>
        public bool HasReturned { get; internal set; }
        /// <summary>
        /// The value returned from the delayed invoke
        /// </summary>
        public object ReturnValue { get; internal set; }

        /// <summary>
        /// If the delayed invoke has been aborted
        /// </summary>
        public bool HasAborted { get; private set; }

        /// <summary>
        /// Create a new Delayed Invoke state
        /// </summary>
        /// <param name="thread">The thread that the delayed invoke is running on</param>
        public DelayedInvokeState(Thread thread)
        {
            this.thread = thread;
        }

        /// <summary>
        /// If the timer should restart after the current timeout is completed
        /// </summary>
        public bool Restart { get; set; }

        public DelayedInvokeState(Delegate action, int timeout, bool blocking, object[] args = null)
        {
            if (blocking)
            {
                do
                {
                    Restart = false;
                    Thread.Sleep(timeout);
                } while (Restart);
                HasReturned = true;
                ReturnValue = action.DynamicInvoke(args);
            }
            else
            {
                thread = new Thread(delegate()
                {
                    do
                    {
                        Restart = false;
                        Thread.Sleep(timeout);
                    } while (Restart);
                    HasReturned = true;
                    ReturnValue = action.DynamicInvoke(args);
                });
                thread.Name = "DelayedInvoke: " + action.Method.Name;
                thread.Start();
            }
        }

        /// <summary>
        /// Abort the delayed invoke
        /// </summary>
        public void Abort()
        {
            try
            {
                thread.Abort();
            }
            catch (ThreadAbortException)
            {
            }
            this.HasAborted = true;
        }
    }
}
