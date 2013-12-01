using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace StUtil.Native
{
    public static class ProcessStartWatcher
    {
        private static ManagementEventWatcher watcher;
        public static event EventHandler<EventArgs<ManagementBaseObject>> ProcessStarted;

        public static void Start()
        {
            Start(new TimeSpan(0, 0, 1));
        }

        public static void Start(TimeSpan pollTime)
        {
            if (watcher != null)
            {
                watcher.Dispose();
            }
            watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("__InstanceCreationEvent", pollTime, "TargetInstance isa \"Win32_Process\"");

            watcher.Query = query;
            watcher.EventArrived += ProcessStartEvent;
            watcher.Start();
        }

        public static void Stop()
        {
            if (watcher != null)
            {
                watcher.Stop();
                watcher.EventArrived -= ProcessStartEvent;
                watcher.Dispose();
                watcher = null;
            }
        }

        private static void ProcessStartEvent(object sender, EventArrivedEventArgs e)
        {
            try
            {
                System.Management.ManagementBaseObject props = (System.Management.ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
                if (ProcessStarted != null)
                {
                    ProcessStarted(sender, new EventArgs<ManagementBaseObject>(props));
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
