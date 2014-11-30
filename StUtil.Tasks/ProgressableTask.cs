using System;
using System.ComponentModel;

namespace StUtil.Tasks
{
    /// <summary>
    /// Base class for implementing tasks that have a measurable progress
    /// </summary>
    public abstract class ProgressableTask : TaskWorker
    {
        /// <summary>
        /// Indicates that the progress of the task has changed
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Indicates that the maximum value of the progressable task has changed
        /// </summary>
        public event EventHandler MaximumValueChanged;

        /// <summary>
        /// The maximum value of the task
        /// </summary>
        protected double maximumValue = 0;

        /// <summary>
        /// The maximum value of the task
        /// </summary>
        public double MaximumValue
        {
            get
            {
                return maximumValue;
            }
            set
            {
                maximumValue = value;
                OnMaximumValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// The current value of the task
        /// </summary>
        protected double currentValue = 0;

        /// <summary>
        /// The current value of the task
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return currentValue;
            }
            set
            {
                currentValue = value;
                OnProgressChanged(new ProgressChangedEventArgs((int)((currentValue / MaximumValue) * 100), null));
            }
        }

        /// <summary>
        /// Raises the maximum value changed event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnMaximumValueChanged(EventArgs e)
        {
            if (MaximumValueChanged != null)
            {
                MaximumValueChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the progress changed event
        /// </summary>
        /// <param name="e">The event args</param>
        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }
    }
}