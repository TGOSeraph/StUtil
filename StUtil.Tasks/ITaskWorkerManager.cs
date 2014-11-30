using System;

namespace StUtil.Tasks
{
    public interface ITaskWorkerManager
    {
        /// <summary>
        /// Fires if running tasks is cancelled
        /// </summary>
        event EventHandler TasksCancelled;

        /// <summary>
        /// Fires once all tasks are completed
        /// </summary>
        event EventHandler TasksCompleted;

        /// <summary>
        /// Fires if running tasks fails
        /// </summary>
        event EventHandler TasksFailed;

        /// <summary>
        /// Aborts the tasks.
        /// </summary>
        void Abort();

        /// <summary>
        /// Starts the tasks.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this tasks.
        /// </summary>
        void Stop();
    }
}