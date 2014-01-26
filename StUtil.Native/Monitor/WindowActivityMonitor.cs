using System;
using System.Threading;
using StUtil.Internal.Native;

namespace StUtil.Native.Monitor
{
    public class WindowActivityMonitor : IDisposable
    {
        public event EventHandler<WindowActivityEventArgs> ActiveWindowChanged;

        private Thread activityWatcher;

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (value && !enabled)
                {
                    enabled = true;
                    WatchForWindowActivity();
                } 
            }
        }

        private int pollInterval = 2;
        public int PollInterval
        {
            get
            {
                return this.pollInterval;
            }
            set
            {
                if (value != pollInterval)
                {
                    this.pollInterval = value;
                }
            }
        }
        
        public WindowActivityData ActiveWindow { get; private set; }

        private void WatchForWindowActivity()
        {
            activityWatcher = new Thread(new ThreadStart(ActivityThreadProc));
            activityWatcher.Name = "Activity Watcher";
            activityWatcher.Start();
        }

        private void ActivityThreadProc()
        {
            while (enabled)
            {
                IntPtr activeWindow = NativeMethods.GetForegroundWindow();
                if (ActiveWindow == null || ActiveWindow.Handle != activeWindow)
                {
                    if (ActiveWindowChanged != null)
                    {
                        ActiveWindowChanged(this, new WindowActivityEventArgs(new WindowActivityData(activeWindow)));
                    }
                }
                if (enabled)
                {
                    Thread.Sleep(pollInterval * 1000);
                }
            }
        }

        public void Dispose()
        {
            if (activityWatcher != null && activityWatcher.IsAlive)
            {
                this.enabled = false;
                activityWatcher.Join();
            }
            
        }
    }
}
