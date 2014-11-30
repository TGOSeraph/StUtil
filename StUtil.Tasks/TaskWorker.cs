using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace StUtil.Tasks
{
    /// <summary>
    /// Base class for creating tasks
    /// </summary>
    public abstract class TaskWorker : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the task
        /// </summary>
        private string name;

        /// <summary>
        /// The current state of the task
        /// </summary>
        private WorkerState state = WorkerState.NotStarted;

        /// <summary>
        /// The worker thread used when in async mode
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWorker"/> class.
        /// </summary>
        public TaskWorker()
        {
            Asynchronous = true;
        }

        /// <summary>
        /// Notifies that a properties value has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies that the state of the task has changed
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// The state that the task is currently in
        /// </summary>
        public enum WorkerState
        {
            NotStarted,
            Running,
            Stopping,
            Completed,
            Cancelled,
            Recovering,
            Skipped,
            Failed
        }
        /// <summary>
        /// If the task should be run on a separate thread
        /// </summary>
        public bool Asynchronous { get; set; }

        /// <summary>
        /// If the thread should be run as a background thread
        /// </summary>
        public bool IsBackground { get; set; }

        /// <summary>
        /// The error that was caught triggering the Failed state
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// If the task is currently running
        /// </summary>
        public bool IsActive
        {
            get
            {
                return State == WorkerState.Running || State == WorkerState.Stopping || State == WorkerState.Recovering;
            }
        }

        /// <summary>
        /// If the task is in an inactive state after having started
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return State == WorkerState.Cancelled || State == WorkerState.Completed || State == WorkerState.Failed || State == WorkerState.Skipped;
            }
        }

        /// <summary>
        /// If the task has not yet started
        /// </summary>
        public bool IsPending
        {
            get
            {
                return State == WorkerState.NotStarted;
            }
        }

        /// <summary>
        /// If the task can recover from exceptions
        /// </summary>
        public virtual bool IsRecoverable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The name of the task
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetField(ref name, value); }
        }

        /// <summary>
        /// The current state of the task
        /// </summary>
        public WorkerState State
        {
            get
            {
                return state;
            }
            private set
            {
                if (value != state)
                {
                    state = value;
                    OnStateChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// If the task finished successfully
        /// </summary>
        public bool WasSuccessful
        {
            get
            {
                return State == WorkerState.Completed;
            }
        }
        /// <summary>
        /// Abort the task if stop does not work. Will only abort async tasks.
        /// </summary>
        public virtual void Abort()
        {
            if (!IsActive)
            {
                return;
            }
            if (Asynchronous)
            {
                try
                {
                    workerThread.Abort();
                }
                catch (ThreadAbortException)
                {
                }
                State = WorkerState.Cancelled;
            }
            else
            {
                throw new InvalidOperationException("Cannot abort worker if it is not run asynchronously.");
            }
        }

        /// <summary>
        /// Reset the task
        /// </summary>
        public virtual void Reset()
        {
        }

        /// <summary>
        /// Start the task
        /// </summary>
        public void Start()
        {
            if (IsActive)
            {
                throw new InvalidOperationException("Cannot start worker if is already in an active state.");
            }
            if (Asynchronous)
            {
                workerThread = CreateWorkerThread();
                workerThread.Start();
            }
            else
            {
                TaskProc();
            }
        }

        /// <summary>
        /// Stop the task
        /// </summary>
        public virtual void Stop()
        {
            if (State != WorkerState.Running)
            {
                return;
            }
            State = WorkerState.Stopping;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Try recovering from the last exception
        /// </summary>
        /// <param name="moveNext">If on a successful recovery the next task should be run</param>
        /// <returns>If the recovery was successful</returns>
        protected virtual bool DoRecover(out bool moveNext)
        {
            moveNext = false;
            return false;
        }

        /// <summary>
        /// Try recovering from the last exception
        /// </summary>
        /// <param name="moveNext">If on a successful recovery the next task should be run</param>
        /// <returns>If the recovery was successful</returns>
        public bool TryRecover(out bool moveNext)
        {
            WorkerState state = State;
            State = WorkerState.Recovering;

            bool recovered = DoRecover(out moveNext);
            if (recovered)
            {
                State = WorkerState.NotStarted;
            }
            else
            {
                State = WorkerState.Failed;
            }
            return recovered;
        }

        /// <summary>
        /// Create the worker thread when running async
        /// </summary>
        /// <returns>The worker thread</returns>
        protected virtual Thread CreateWorkerThread()
        {
            Thread thread = new Thread(TaskProc);
            thread.IsBackground = IsBackground;
            if (string.IsNullOrWhiteSpace(name))
            {
                thread.Name = "Task Worker Thread";
            }
            else
            {
                thread.Name = name + "Task Worker Thread";
            }
            return thread;
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the state changed event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnStateChanged(EventArgs e)
        {
            if (StateChanged != null)
            {
                StateChanged(this, e);
            }
        }

        /// <summary>
        /// Perform the task operations
        /// </summary>
        protected abstract void Run();

        /// <summary>
        /// Sets a field and raises the property changed event
        /// </summary>
        /// <typeparam name="T">The type of the property that has changed</typeparam>
        /// <param name="field">The current value of the property</param>
        /// <param name="value">The new value of the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>If the event was raised</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /// <summary>
        /// Internal task running method
        /// </summary>
        protected virtual void TaskProc()
        {
            State = WorkerState.Running;

            try
            {
                Run();
                if (State == WorkerState.Stopping)
                {
                    State = WorkerState.Cancelled;
                }
                else
                {
                    State = WorkerState.Completed;
                }
            }
            catch (Exception ex)
            {
                Error = ex;
                State = WorkerState.Failed;
            }
        }
    }
}
