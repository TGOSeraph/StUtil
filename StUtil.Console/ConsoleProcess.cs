using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using StUtil.Extensions;

namespace StUtil.Console
{
    public class ConsoleProcess
    {
        public event EventHandler ProcessExited;
        public event EventHandler<ConsoleDataRecievedEventArgs> DataRecieved;

        public Process Process { get; private set; }
        public string ApplicationPath { get; private set; }
        public string[] Arguments { get; private set; }

        public object Tag { get; set; }

        public ConsoleProcess(string applicationPath, params string[] arguments)
        {
            this.ApplicationPath = applicationPath;
            this.Arguments = arguments;
        }

        public void Start()
        {
            Process = Process.Start(new ProcessStartInfo
            {
                FileName = ApplicationPath,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                Arguments = Arguments.Length == 0 ? "" : string.Join(" ", Arguments.ToArray())
            });
            Process.OutputDataReceived += proc_OutputDataReceived;
            Process.ErrorDataReceived += proc_ErrorDataReceived;
            Process.Exited += proc_Exited;
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
            Process.EnableRaisingEvents = true;
        }

        private void proc_Exited(object sender, EventArgs e)
        {
            Process.WaitForExit();
            ProcessExited.RaiseEvent(this);
        }

        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataRecieved.RaiseEvent(this, new ConsoleDataRecievedEventArgs(e.Data, true));
        }

        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataRecieved.RaiseEvent(this, new ConsoleDataRecievedEventArgs(e.Data, false));
        }
    }
}
