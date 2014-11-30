using StUtil.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StUtil.Tasks
{
    /// <summary>
    /// Used to manage running a group of tasks
    /// </summary>
    public class TaskWorkerManager : ITaskWorkerManager
    {
        /// <summary>
        /// Fires if running tasks is cancelled
        /// </summary>
        public event EventHandler TasksCancelled;

        /// <summary>
        /// Fires once all tasks are completed
        /// </summary>
        public event EventHandler TasksCompleted;

        /// <summary>
        /// Fires if running tasks fails
        /// </summary>
        public event EventHandler TasksFailed;

        /// <summary>
        /// The current index of the task that is running
        /// </summary>
        private int currentIndex = -1;

        /// <summary>
        /// If we are currently recovering
        /// </summary>
        private bool isRecovering = false;

        /// <summary>
        /// If we should stop processing
        /// </summary>
        private bool stop = false;

        /// <summary>
        /// If tasks should be reset before being run
        /// </summary>
        public bool AutoResetTasks { get; set; }

        /// <summary>
        /// If the Task.Recover() method should be automatically called if a task fails
        /// </summary>
        public bool AutoTryRecover { get; set; }

        /// <summary>
        /// The task that is currently running
        /// </summary>
        public TaskWorker CurrentTask
        {
            get
            {
                return currentIndex > -1 ? Tasks[currentIndex] : null;
            }
        }

        /// <summary>
        /// If tasks are currently running
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// If a task is currently recovering
        /// </summary>
        public bool IsRecovering { get; private set; }

        /// <summary>
        /// The task that will be run next
        /// </summary>
        public TaskWorker NextTask
        {
            get
            {
                return (currentIndex) > -1 ?
                    (currentIndex + 1 < Tasks.Count) ? Tasks[currentIndex + 1] : null
                    : null;
            }
        }

        /// <summary>
        /// That task that just ran
        /// </summary>
        public TaskWorker PreviousTask
        {
            get
            {
                return (currentIndex - 1) > -1 ? Tasks[currentIndex - 1] : null;
            }
        }

        /// <summary>
        /// The tasks to run
        /// </summary>
        public StUtil.Data.Generic.BindingList<TaskWorker> Tasks { get; private set; }

        /// <summary>
        /// Create a new task manager
        /// </summary>
        public TaskWorkerManager()
        {
            Tasks = new StUtil.Data.Generic.BindingList<TaskWorker>();
            //Tasks.ListChanged += Tasks_ListChanged;
        }

        /// <summary>
        /// Create a new task manager
        /// </summary>
        /// <param name="tasks">The tasks to run</param>
        public TaskWorkerManager(params TaskWorker[] tasks)
            : this()
        {
            Tasks.AddRange(tasks);
        }

        /*
         * Currently unused code. I wrote it for something, but not sure what
            protected virtual void Tasks_ListChanged(object sender, ListChangedEventArgs e)
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        TaskAdded(e.NewIndex, Tasks[e.NewIndex]);
                        break;

                    case ListChangedType.ItemChanged:
                        TaskChanged(e.NewIndex, Tasks[e.NewIndex]);
                        break;

                    case ListChangedType.ItemDeleted:
                        TaskDeleted(e.OldIndex, ((ListChangedEventArgsWithRemovedItem<TaskWorker>)e).Item);
                        break;
                }
            }

            protected virtual void TaskChanged(int index, TaskWorker worker)
            {
            }
            protected virtual void TaskDeleted(int index, TaskWorker worker)
            {
            }
            protected virtual void TaskAdded(int index, TaskWorker worker)
            {
            }
        */

        /// <summary>
        /// Aborts this instance.
        /// </summary>
        public virtual void Abort()
        {
            stop = true;
            if (CurrentTask.IsActive)
            {
                CurrentTask.Abort();
            }
        }

        /// <summary>
        /// Resumes this instance from the current task
        /// </summary>
        /// <param name="moveToNext">if set to <c>true</c> the next task will be run.</param>
        public virtual void Resume(bool moveToNext)
        {
            stop = false;
            if (moveToNext)
            {
                OnTaskCompleted();
            }
            else
            {
                isRecovering = true;
                RunTask();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public virtual void Start()
        {
            IsActive = true;
            stop = false;
            currentIndex = 0;
            RunTask();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public virtual void Stop()
        {
            stop = true;
            if (CurrentTask.IsActive)
            {
                CurrentTask.Stop();
            }
        }

        /// <summary>
        /// Run the task that is selected
        /// </summary>
        protected virtual void RunTask()
        {
            TaskWorker task = CurrentTask;
            if (AutoResetTasks && !isRecovering)
            {
                CurrentTask.Reset();
            }
            isRecovering = false;
            task.StateChanged += Task_StateChanged;
            task.Start();
        }

        /// <summary>
        /// Handles the StateChanged event of the task control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void Task_StateChanged(object sender, EventArgs e)
        {
            TaskWorker task = (TaskWorker)sender;
            if (task.IsFinished)
            {
                CurrentTask.StateChanged -= Task_StateChanged;

                if (task.State == TaskWorker.WorkerState.Completed)
                {
                    OnTaskCompleted();
                }
                else if (task.State == TaskWorker.WorkerState.Cancelled)
                {
                    OnTasksCancelled();
                }
                else if (task.State == TaskWorker.WorkerState.Failed)
                {
                    if (AutoTryRecover)
                    {
                        if (task.IsRecoverable)
                        {
                            bool moveNext;
                            IsRecovering = true;
                            if (task.TryRecover(out moveNext))
                            {
                                IsRecovering = false;
                                if (stop)
                                {
                                    OnTasksCancelled();
                                    return;
                                }
                                Resume(moveNext);
                                return;
                            }
                            IsRecovering = false;
                            if (stop)
                            {
                                OnTasksCancelled();
                                return;
                            }
                        }
                    }
                    OnTasksFailed();
                }
            }
        }

        /// <summary>
        /// Called when the task is completed.
        /// </summary>
        private void OnTaskCompleted()
        {
            if (currentIndex < Tasks.Count - 1)
            {
                currentIndex++;
                RunTask();
            }
            else
            {
                IsActive = false;
                if (TasksCompleted != null) TasksCompleted(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when tasks are cancelled.
        /// </summary>
        private void OnTasksCancelled()
        {
            IsActive = false;
            if (TasksCancelled != null) TasksCancelled(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when tasks fail.
        /// </summary>
        private void OnTasksFailed()
        {
            IsActive = false;
            if (TasksFailed != null) TasksFailed(this, EventArgs.Empty);
        }
    }
}