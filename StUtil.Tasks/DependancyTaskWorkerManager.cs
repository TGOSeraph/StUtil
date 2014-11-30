using StUtil.Extensions;
using StUtil.Data.Specialised;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Tasks
{
    public class DependancyTaskWorkerManager : ITaskWorkerManager
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
        /// The completed tasks.
        /// </summary>
        private BlockingCollection<TaskWorker> completed = new BlockingCollection<TaskWorker>();

        /// <summary>
        /// If running tasks failed
        /// </summary>
        private bool failed;

        /// <summary>
        /// The managers for different tasks running in parallel
        /// </summary>
        private ConcurrentDictionary<TaskWorkerManager, TaskWorkerManager> managers = new ConcurrentDictionary<TaskWorkerManager, TaskWorkerManager>();

        /// <summary>
        /// If the Task.Recover() method should be automatically called if a task fails
        /// </summary>
        public bool AutoTryRecover { get; set; }

        /// <summary>
        /// Gets the completed tasks.
        /// </summary>
        /// <value>
        /// The completed tasks.
        /// </value>
        public IEnumerable<TaskWorker> Completed { get { return completed; } }

        /// <summary>
        /// The tasks that are currently running
        /// </summary>
        public ConcurrentDictionary<TaskWorker, TaskWorker> CurrentTasks { get; private set; }

        /// <summary>
        /// Gets or sets the dependancies between the workers.
        /// </summary>
        /// <value>
        /// The dependancies.
        /// </value>
        public DependancyHelper<TaskWorker> Dependancies { get; private set; }

        /// <summary>
        /// Gets or sets the dependancies between the workers.
        /// </summary>
        /// <value>
        /// The dependancies.
        /// </value>
        public DependancyHelper<TaskWorker> PreventSimultaneousRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return CurrentTasks.Count > 0 ? true : false;
            }
        }

        /// <summary>
        /// The tasks to run
        /// </summary>
        public StUtil.Data.Generic.BindingList<TaskWorker> Tasks { get; private set; }

        /// <summary>
        /// Create a new task manager
        /// </summary>
        public DependancyTaskWorkerManager()
        {
            Dependancies = new DependancyHelper<TaskWorker>();
            PreventSimultaneousRunning = new DependancyHelper<TaskWorker>();
            Tasks = new Data.Generic.BindingList<TaskWorker>();
        }

        /// <summary>
        /// Create a new task manager
        /// </summary>
        /// <param name="tasks">The tasks to run</param>
        public DependancyTaskWorkerManager(params TaskWorker[] tasks)
            : this()
        {
            Tasks.AddRange(tasks);
        }

        /// <summary>
        /// Aborts the tasks.
        /// </summary>
        public void Abort()
        {
            foreach (TaskWorkerManager mgr in managers.Keys)
            {
                mgr.Abort();
            }
            managers.Clear();
        }

        /// <summary>
        /// Starts the tasks.
        /// </summary>
        public void Start()
        {
            CurrentTasks = new ConcurrentDictionary<TaskWorker, TaskWorker>();
            completed = new BlockingCollection<TaskWorker>();
            failed = false;
            managers = new ConcurrentDictionary<TaskWorkerManager, TaskWorkerManager>();
            RunTasks();
        }

        /// <summary>
        /// Stops this tasks.
        /// </summary>
        public void Stop()
        {
            foreach (TaskWorkerManager mgr in managers.Keys)
            {
                mgr.Stop();
            }
            managers.Clear();
        }

        /// <summary>
        /// Handles the TasksCancelled event of the mgr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mgr_TasksCancelled(object sender, EventArgs e)
        {
            CleanupAfterTask((TaskWorkerManager)sender);

            if (!failed)
            {
                Stop();
                TasksCancelled.RaiseEvent(this);
            }
        }

        /// <summary>
        /// Handles the TasksCompleted event of the mgr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mgr_TasksCompleted(object sender, EventArgs e)
        {
            CleanupAfterTask((TaskWorkerManager)sender);

            RunTasks();
        }

        /// <summary>
        /// Handles the TasksFailed event of the mgr control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void mgr_TasksFailed(object sender, EventArgs e)
        {
            CleanupAfterTask((TaskWorkerManager)sender);

            failed = true;
            Abort();
            TasksFailed.RaiseEvent(this);
        }

        /// <summary>
        /// Cleans up after a task.
        /// </summary>
        /// <param name="mgr">The task manager.</param>
        private void CleanupAfterTask(TaskWorkerManager mgr)
        {
            mgr.TasksCancelled -= mgr_TasksCancelled;
            mgr.TasksCompleted -= mgr_TasksCompleted;
            mgr.TasksFailed -= mgr_TasksFailed;
            TaskWorker dummy;
            CurrentTasks.TryRemove(mgr.CurrentTask, out dummy);
            completed.Add(mgr.CurrentTask);
            managers.TryRemove(mgr, out mgr);
        }

        /// <summary>
        /// Runs the tasks.
        /// </summary>
        private void RunTasks()
        {
            if (completed.Count == Tasks.Count)
            {
                TasksCompleted.RaiseEvent(this);
                return;
            }
            lock (CurrentTasks)
            {
                IEnumerable<TaskWorker> available = Dependancies.GetAvailable(Tasks, Completed);
                available = available.Except(CurrentTasks.Keys);

                foreach (TaskWorker worker in available)
                {
                    List<TaskWorker> ensureNotRunning = new List<TaskWorker>();
                    //Check if we have a key saying that this cannot be run at the same time as...
                    if (PreventSimultaneousRunning.Dependancies.ContainsKey(worker))
                    {
                        ensureNotRunning = PreventSimultaneousRunning.Dependancies[worker].DependsOn.ToList();
                    }

                    //Then see if anything says x is not allowed to be run at the same time as this
                    IEnumerable<Dependacy<TaskWorker>> dependancies = PreventSimultaneousRunning.Dependancies.Where(d => d.Value.DependsOn.Contains(worker)).Select(kvp => kvp.Value);
                    ensureNotRunning.AddRange(dependancies.Select(depend =>
                    {
                        List<TaskWorker> items = new List<TaskWorker>();
                        items.Add(depend.Item);
                        return items;
                    }).SelectMany(i => i));

                    if (CurrentTasks.Any(t => ensureNotRunning.Contains(t.Value)))
                    {
                        continue;
                    }

                    CurrentTasks.TryAdd(worker, worker);
                    TaskWorkerManager mgr = new TaskWorkerManager(worker);
                    mgr.AutoTryRecover = AutoTryRecover;
                    mgr.TasksCancelled += mgr_TasksCancelled;
                    mgr.TasksCompleted += mgr_TasksCompleted;
                    mgr.TasksFailed += mgr_TasksFailed;
                    managers.TryAdd(mgr, mgr);
                    mgr.Start();
                }
            }
        }
    }
}
