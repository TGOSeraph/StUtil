using StUtil.Extensions;
using StUtil.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StUtil.Dev.WinForm.Controls
{
    public partial class TaskWorkerItem : UserControl
    {
        /// <summary>
        /// Gets or sets the state images.
        /// </summary>
        /// <value>
        /// The state images.
        /// </value>
        public Dictionary<TaskWorker.WorkerState, Bitmap> StateImages
        {
            get;
            set;
        }

        private TaskWorker worker;

        /// <summary>
        /// Gets or sets the task that this control represents.
        /// </summary>
        /// <value>
        /// The task that this control represents.
        /// </value>
        public TaskWorker Task
        {
            get
            {
                return worker;
            }
            set
            {
                if (worker != null)
                {
                    worker.StateChanged -= worker_StateChanged;
                    worker.PropertyChanged -= worker_PropertyChanged;
                }
                worker = value;
                TaskName.Text = worker.Name;
                TaskIcon.Image = StateImages[worker.State].Duplicate();
                worker.StateChanged += worker_StateChanged;
                worker.PropertyChanged += worker_PropertyChanged;
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void worker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                try
                {
                    this.Invoke((Action)delegate()
                    {
                        TaskName.Text = worker.Name;
                    });
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Handles the StateChanged event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void worker_StateChanged(object sender, EventArgs e)
        {
            lock (StateImages)
            {
                Bitmap image = (Bitmap)TaskIcon.Image;
                TaskIcon.Image = StateImages[((TaskWorker)sender).State].Duplicate();
                if (image != null)
                {
                    image.Dispose();
                }
            }
            try
            {
                this.Invoke((Action)delegate()
                {
                    toolTip1.SetToolTip(TaskIcon, "Task " + ((TaskWorker)sender).State.ToString());
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWorkerItem"/> class.
        /// </summary>
        public TaskWorkerItem()
        {
            InitializeComponent();
            StateImages = new Dictionary<TaskWorker.WorkerState, Bitmap>();
            StateImages.Add(TaskWorker.WorkerState.Cancelled, StUtil.Tasks.Properties.Resources.task_stop);
            StateImages.Add(TaskWorker.WorkerState.Completed, StUtil.Tasks.Properties.Resources.task_complete);
            StateImages.Add(TaskWorker.WorkerState.Failed, StUtil.Tasks.Properties.Resources.task_error);
            StateImages.Add(TaskWorker.WorkerState.NotStarted, StUtil.Tasks.Properties.Resources.task_idle);
            StateImages.Add(TaskWorker.WorkerState.Recovering, StUtil.Tasks.Properties.Resources.task_recovering);
            StateImages.Add(TaskWorker.WorkerState.Running, StUtil.Tasks.Properties.Resources.task_running);
            StateImages.Add(TaskWorker.WorkerState.Skipped, StUtil.Tasks.Properties.Resources.task_complete);
            StateImages.Add(TaskWorker.WorkerState.Stopping, StUtil.Tasks.Properties.Resources.task_recovering);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWorkerItem"/> class.
        /// </summary>
        /// <param name="task">The task to represent.</param>
        public TaskWorkerItem(TaskWorker task)
            : this()
        {
            this.Task = task;
        }
    }
}