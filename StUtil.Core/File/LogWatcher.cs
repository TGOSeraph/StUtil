using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace StUtil.File
{
    class LogWatcher
    {
        public event EventHandler WatchingFinished;
        public event EventHandler<EventArgs<string, double>> LineRead;

        private bool cancel = false;
        private Thread workerThread;

        public bool WatchingFile { get; private set; }
        public string LogFile { get; private set; }
        public bool ExitOnStreamEnd { get; set; }
        public int WaitTime { get; set; }

        public LogWatcher(string logFile)
        {
            this.WaitTime = 250;
            LogFile = logFile;
        }

        public void StartWatching()
        {
            cancel = false;
            workerThread = new Thread(delegate()
            {
                WatcherThreadProc();
            });
            workerThread.Start();
            this.WatchingFile = true;
        }

        private void WatcherThreadProc()
        {
            if (String.IsNullOrEmpty(LogFile))
            {
                throw new NullReferenceException();
            }
            using (FileStream fs = new FileStream(LogFile, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
            {
                long lastFileSize = fs.Length;
                StreamReader sr = null;
                while (!cancel)
                {
                    sr = new StreamReader(fs);
                    while (!cancel && sr.Peek() > -1)
                    {
                        string line = sr.ReadLine();
                        if (LineRead != null)
                        {
                            LineRead(this, new EventArgs<string, double>(line, ((double)fs.Position / fs.Length) * 100));
                        }
                    }
                    if (ExitOnStreamEnd)
                    {
                        break;
                    }
                    while (!ExitOnStreamEnd && !cancel && fs.Length == lastFileSize)
                    {
                        Thread.Sleep(this.WaitTime);
                    }
                }
                try
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                catch (Exception)
                {
                }
            }
            WatchingFile = false;
            if (WatchingFinished != null)
            {
                WatchingFinished(this, new EventArgs());
            }
        }

        public void StopWatching()
        {
            cancel = true;
            while (WatchingFile)
            {
                Thread.Sleep(0);
            }
        }
    }
}
