using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;

namespace StUtil.Native.Monitor
{
    public class ProcessActivityMonitor : IDisposable
    {
        public event EventHandler<ProcessActivityEventArgs> ProcessCreated;
        public event EventHandler<ProcessActivityEventArgs> ProcessTerminated;

        private ManagementEventWatcher processMonitor;

        private BindingList<string> processes = new BindingList<string>();
        public BindingList<string> Processes
        {
            get
            {
                return this.processes;
            }
        }

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (value && !this.enabled)
                {
                    this.enabled = true;
                    this.processMonitor = WatchForProcessOperation();
                }
                else if (!value && this.enabled)
                {
                    this.enabled = false;
                    this.DisposeEventWatcher();
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
                    this.processMonitor = WatchForProcessOperation();
                }
            }
        }

        public ProcessActivityMonitor()
        {
            this.processes.ListChanged += new ListChangedEventHandler(processes_ListChanged);
        }

        private void processes_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.enabled)
            {
                this.WatchForProcessOperation();
            }
        }

        private ManagementEventWatcher WatchForProcessOperation()
        {
            this.DisposeEventWatcher();

            if (this.processes == null || this.processes.Count == 0)
            {
                return null;
            }

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (this.processes.Contains(p.ProcessName + ".exe"))
                    {
                        if (ProcessCreated != null)
                        {
                            ProcessCreated(this, new ProcessActivityEventArgs(new ProcessActivityData(
                                p.Id,
                                p.ProcessName,
                                p.MainModule.FileName)));
                        }
                    }
                }
                catch (Exception)
                {
                    //Ignore errors
                }
            }
            string queryString = @"SELECT TargetInstance " +
                "FROM __InstanceOperationEvent " +
                "WITHIN " + pollInterval.ToString() + " " +
                "WHERE TargetInstance ISA 'Win32_Process' " +
                "AND (TargetInstance.Name = '" +
                String.Join("'\n   OR TargetInstance.Name = '", processes) +
                (processes.Count > 1 ? "')" : "')");

            string scope = @"\\.\root\CIMV2";

            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessOperation;
            watcher.Start();
            return watcher;
        }

        private void ProcessOperation(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string processName = targetInstance.Properties["Name"].Value.ToString();
            switch (e.NewEvent.ClassPath.ClassName)
            {
                case "__InstanceCreationEvent":
                    if (ProcessCreated != null)
                    {
                        ProcessCreated(this, new ProcessActivityEventArgs(new ProcessActivityData(
                            int.Parse(targetInstance.Properties["ProcessID"].Value.ToString()),
                            targetInstance.Properties["Name"].Value.ToString(),
                            targetInstance.Properties["ExecutablePath"].Value.ToString())));
                    }
                    break;
                case "__InstanceDeletionEvent":
                    if (ProcessTerminated != null)
                    {
                        ProcessTerminated(this, new ProcessActivityEventArgs(new ProcessActivityData(
                            int.Parse(targetInstance.Properties["ProcessID"].Value.ToString()),
                            targetInstance.Properties["Name"].Value.ToString(),
                            targetInstance.Properties["ExecutablePath"].Value.ToString())));
                    }
                    break;
            }
            e.NewEvent.Dispose();
        }

        private void DisposeEventWatcher()
        {
            if (this.processMonitor != null)
            {
                this.processMonitor.EventArrived -= ProcessOperation;
                this.processMonitor.Dispose();
                this.processMonitor = null;
            }
        }

        public void Dispose()
        {
            this.DisposeEventWatcher();
        }
    }
}
