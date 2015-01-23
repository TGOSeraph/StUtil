using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StUtil.IO
{
    public class FileWatcher
    {
        public event EventHandler<EventArgs<string, double>> LineRead;

            public event EventHandler WatchingFinished;
            private bool cancel = false;
            private FileStream fileStream;
            private Thread workerThread;
            public bool ExitOnStreamEnd { get; set; }

            public int Length
            {
                get
                {
                    return fileStream == null ? 0 : (int)fileStream.Length;
                }
            }

            public string LogFile { get; private set; }

            public int Position
            {
                get
                {
                    return fileStream == null ? 0 : (int)fileStream.Position;
                }
            }

            public int WaitTime { get; set; }

            public bool WatchingFile { get; private set; }
            public FileWatcher(string logFile)
            {
                this.WaitTime = 250;
                LogFile = logFile;
            }
            public void StartWatching(int startPos = 0)
            {
                cancel = false;
                workerThread = new Thread(WatcherThreadProc);
                workerThread.Start(startPos);
                this.WatchingFile = true;
            }

            public void StopWatching()
            {
                cancel = true;
                while (WatchingFile)
                {
                    Thread.Sleep(0);
                }
            }

            private void WatcherThreadProc(object arg)
            {
                int start = (int)arg;

                if (String.IsNullOrEmpty(LogFile))
                {
                    throw new NullReferenceException();
                }
                StreamReader sr = null;
                fileStream = null;
                try
                {
                    fileStream = new FileStream(LogFile, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite);
                    fileStream.Seek(start, SeekOrigin.Begin);
                    long lastFileSize = fileStream.Length;
                    while (!cancel)
                    {
                        sr = new StreamReader(fileStream);
                        while (!cancel && sr.Peek() > -1)
                        {
                            string line = sr.ReadLine();
                            if (LineRead != null)
                            {
                                LineRead(this, new EventArgs<string, double>(line, ((double)fileStream.Position / fileStream.Length) * 100));
                            }
                        }
                        if (ExitOnStreamEnd)
                        {
                            break;
                        }
                        while (!ExitOnStreamEnd && !cancel && fileStream.Length == lastFileSize)
                        {
                            Thread.Sleep(this.WaitTime);
                        }
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                    if (sr != null)
                    {
                        sr.Close();
                        sr.Dispose();
                        sr = null;
                    }
                }
                WatchingFile = false;
                if (WatchingFinished != null)
                {
                    WatchingFinished(this, new EventArgs());
                }
            }
    }
}
