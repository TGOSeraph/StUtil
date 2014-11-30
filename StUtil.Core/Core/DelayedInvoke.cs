using System;
using System.Threading;

namespace StUtil.Core
{
    /// <summary>
    /// A state object for a Delayed Invoke
    /// </summary>
    public class DelayedInvoke
    {
        /// <summary>
        /// The thread the delayed invoke will be run on if set to non-blocking
        /// </summary>
        private Thread thread;

        /// <summary>
        /// If the delayed invoke has been aborted
        /// </summary>
        public bool HasAborted { get; private set; }

        /// <summary>
        /// If the delayed invoke has completed
        /// </summary>
        public bool HasReturned { get; internal set; }

        /// <summary>
        /// If the timer should restart after the current timeout is completed
        /// </summary>
        public bool Restart { get; set; }

        /// <summary>
        /// The value returned from the delayed invoke
        /// </summary>
        public object ReturnValue { get; internal set; }

        /// <summary>
        /// Create a new Delayed Invoke state
        /// </summary>
        /// <param name="thread">The thread that the delayed invoke is running on</param>
        public DelayedInvoke(Thread thread)
        {
            this.thread = thread;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayedInvoke"/> class.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="timeout">The timeout before running.</param>
        /// <param name="blocking">if set to <c>true</c> then the delay will block.</param>
        /// <param name="args">The arguments to pass to the action.</param>
        public DelayedInvoke(Delegate action, int timeout, bool blocking, object[] args = null)
        {
            if (timeout == 0)
            {
                action.DynamicInvoke(args);
            }
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