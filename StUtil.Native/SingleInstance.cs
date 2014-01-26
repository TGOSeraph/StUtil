using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using StUtil.Extensions;
using StUtil.Generic;
using StUtil.Internal.Native;

namespace StUtil.Native
{
    public abstract class SingleInstance
    {
        public static event EventHandler<EventArgs<Process>> ExistingProcessFound;

        private static Mutex mutex;
        private static Process existingProcess;

        public static void Init(string GUID)
        {
            mutex = new Mutex(true, GUID);
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                OnExistingProcessFound(new EventArgs<Process>(FindProcess()));
            }
        }

        private static Process FindProcess()
        {
            Process current = Process.GetCurrentProcess();
            Process[] existing = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(current.MainModule.FileName.Replace(".vshost", "")))
                       .Where(p => p.Id != current.Id).ToArray();

            if (existing.Length == 0)
            {
                throw new ApplicationException("Existing process not found");
            }
            else if (existing.Length == 1)
            {
                current = existing[0];
            }
            else
            {
                current = existing[0];
                for (int i = 1; i < existing.Length; i++)
                {
                    if (existing[i].StartTime.Ticks > current.StartTime.Ticks)
                    {
                        current = existing[i];
                    }
                }
            }

            existingProcess = current;

            return current;
        }

        public static void BringExistingToFront()
        {
            NativeMethods.SetForegroundWindow(existingProcess.MainWindowHandle);
        }

        protected static void OnExistingProcessFound(EventArgs<Process> e)
        {
            ExistingProcessFound.RaiseEvent<Process>(null, e.Value);
        }
    }
}
