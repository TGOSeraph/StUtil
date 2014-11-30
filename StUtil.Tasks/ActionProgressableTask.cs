using System;

namespace StUtil.Tasks
{
    /// <summary>
    /// Provides an easy way to implement a task that performs a progressable action
    /// </summary>
    public class ActionProgressableTask : ProgressableTask
    {
        /// <summary>
        /// The task to run
        /// </summary>
        public Action<ActionProgressableTask> Task { get; private set; }

        /// <summary>
        /// The action to run when the task completes
        /// </summary>
        public Action<ActionProgressableTask> Completed { get; set; }

        /// <summary>
        /// If the task acts as the body of a for loop and will be called (MaxValue-CurrentValue)/Step times
        /// </summary>
        public bool AutoIncrement { get; private set; }

        /// <summary>
        /// The step to progress by each interation if auto increment is enabled
        /// </summary>
        public double Step { get; set; }

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
        public ActionProgressableTask(Action<ActionProgressableTask> task)
        {
            this.Task = task;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">The task to perform</param>
        /// <param name="autoIncrement">If the task should act as the body of a for loop</param>
        public ActionProgressableTask(Action<ActionProgressableTask> task, bool autoIncrement)
            : this(task)
        {
            this.Step = 1;
            this.AutoIncrement = autoIncrement;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">The task to perform</param>
        /// <param name="completed">The action to perform when the task completes</param>
        public ActionProgressableTask(Action<ActionProgressableTask> task, Action<ActionProgressableTask> completed)
        {
            this.Task = task;
            this.Completed = completed;
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">The task to perform</param>
        /// <param name="autoIncrement">If the task should act as the body of a for loop</param>
        /// <param name="completed">The action to perform when the task completes</param>
        public ActionProgressableTask(Action<ActionProgressableTask> task, bool autoIncrement, Action<ActionProgressableTask> completed)
            : this(task)
        {
            this.Step = 1;
            this.AutoIncrement = autoIncrement;
            this.Completed = completed;
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

        /// <summary>
        /// Increments the current value by the specified step or if not specified, the default step
        /// </summary>
        /// <param name="step">The amount to increment the current value by</param>
        public virtual void PerformStep(double? step = null)
        {
            double s = step.GetValueOrDefault(this.Step);
            CurrentValue += s;
        }

        /// <summary>
        /// Perform the task operations
        /// </summary>
        protected override void Run()
        {
            if (AutoIncrement)
            {
                for (; CurrentValue < MaximumValue && State != WorkerState.Stopping; PerformStep())
                {
                    Task(this);
                }
            }
            else
            {
                Task(this);
            }
            if (Completed != null)
            {
                Completed(this);
            }
        }

        /// <summary>
        /// Reset the task
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            CurrentValue = 0;
        }
    }
}