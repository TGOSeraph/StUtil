using System;

namespace StUtil.Generic.Progressable
{
    public interface IProgressable
    {
        event EventHandler<ProgressableProcessStartedEventArgs> ProcessStarted;
        event EventHandler<ProgressableProgressChangedEventArgs> ProgressChanged;
        event EventHandler<EventArgs> ProcessCompleted;
        event EventHandler<EventArgs> ProcessCancelled;

        void Start();
        void Cancel();
    }

    public class ProgressableProcessStartedEventArgs : EventArgs
    {

        private object m_data;
        public object Data
        {
            get { return m_data; }
        }

        private decimal m_maximum;
        public decimal MaximumProgressValue
        {
            get { return m_maximum; }
        }

        public ProgressableProcessStartedEventArgs(decimal maximumValue, object data)
        {
            this.m_maximum = maximumValue;
            this.m_data = data;
        }
    }

    public class ProgressableProgressChangedEventArgs : EventArgs
    {

        private object m_data;
        public object Data
        {
            get { return m_data; }
        }

        private decimal m_maximum;
        public decimal MaximumProgressValue
        {
            get { return m_maximum; }
        }

        private decimal m_value;
        public decimal ProgressValue
        {
            get { return m_value; }
        }

        public double ProgressPercentDone
        {
            get { return (double)(ProgressValue / MaximumProgressValue * 100); }
        }

        public ProgressableProgressChangedEventArgs(decimal progressValue, decimal maximumValue, object data)
        {
            this.m_maximum = maximumValue;
            this.m_value = progressValue;
            this.m_data = data;
        }
    }
}