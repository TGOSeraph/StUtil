using System;
namespace StUtil.Generic.Progressable
{
    public abstract class ProgressableProcess : IProgressable
    {
        public event EventHandler<ProgressableProcessStartedEventArgs> ProcessStarted;
        public event EventHandler<ProgressableProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<EventArgs> ProcessCompleted;
        public event EventHandler<EventArgs> ProcessCancelled;

        private bool maxSet = false;
        private decimal maximum;
        public decimal Maximum
        {
            get
            {
                if (!this.maxSet)
                {
                    this.maximum = this.GetMaximumProgressValue();
                    this.maxSet = true;
                }
                return this.maximum;
            }
            set
            {
                this.maximum = value;
                this.maxSet = true;
            }
        }
        public decimal Current { get; set; }
        public decimal Step { get; set; }

        private bool cancel;
        public bool IsCancelling
        {
            get { return cancel; }
        }

        private bool isRunning;
        public virtual bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public ProgressableProcess()
        {
            this.Step = 1;
        }

        public virtual void Cancel()
        {
            cancel = true;
        }

        public virtual void Start()
        {
            if (UseNewThread())
            {
                System.Threading.Thread thread = new System.Threading.Thread(RunProcess);
                thread.Start();
            }
            else
            {
                RunProcess();
            }
        }

        protected virtual void RunProcess()
        {
            IsRunning = true;
            if (ProcessStarted != null)
            {
                ProcessStarted(this, new ProgressableProcessStartedEventArgs(Maximum, GetStartEventData()));
            }

            PerformProcess();

            IsRunning = false;
            if (!cancel)
            {
                if (ProcessCompleted != null)
                {
                    ProcessCompleted(this, new EventArgs());
                }
            }
            else
            {
                cancel = false;
                if (ProcessCancelled != null)
                {
                    ProcessCancelled(this, new EventArgs());
                }
            }
        }

        protected virtual void OnProgressChanged(object sender, ProgressableProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(sender, e);
            }
        }
        protected virtual void OnCancel(object sender, EventArgs e)
        {
            cancel = false;
            if (ProcessCancelled != null)
            {
                ProcessCancelled(sender, e);
            }
        }
        protected virtual void PerformedStep()
        {
            this.PerformedStep(null);
        }
        protected virtual void PerformedStep(object data)
        {
            this.Current += this.Step;
            this.OnProgressChanged(this, new ProgressableProgressChangedEventArgs(this.Current, this.maximum, data));
        }

        protected abstract void PerformProcess();
        protected abstract object GetStartEventData();
        protected abstract decimal GetMaximumProgressValue();
        protected abstract bool UseNewThread();
    }
}