using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Tasks
{
    /// <summary>
    /// Runs the speficied action as a task
    /// </summary>
    public class ActionTask : TaskWorker
    {
        /// <summary>
        /// The task to run
        /// </summary>
        public Action<ActionTask> Task { get; private set; }

        /// <summary>
        /// The function to recover the task in the even of an exception
        /// </summary>
        public Func<TaskWorker, bool> Recover { get; set; }

        /// <summary>
        /// If the task is recoverable
        /// </summary>
        public override bool IsRecoverable
        {
            get
            {
                return Recover != null;
            }
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">The task to perform</param>
        public ActionTask(Action<ActionTask> task)
        {
            this.Task = task;
        }

        /// <summary>
        /// Perform the task operations
        /// </summary>
        protected override void Run()
        {
            Task(this);
        }

        /// <summary>
        /// Try recovering from the last exception
        /// </summary>
        /// <param name="moveNext">If on a successful recovery the next task should be run</param>
        /// <returns>
        /// If the recovery was successful
        /// </returns>
        protected override bool DoRecover(out bool moveNext)
        {
            moveNext = false;
            return Recover(this);
        }
    }
}
