using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using StUtil.Extensions;

namespace StUtil.Console
{
    /// <summary>
    /// Class for creating a console process and redirecting the output
    /// </summary>
    /// <remarks>
    /// 2013-10-17  - Initial version
    /// </remarks>
    public class ConsoleProcess
    {
        /// <summary>
        /// Event fired when the process exits
        /// </summary>
        public event EventHandler ProcessExited;
        /// <summary>
        /// Event fired when data is recieved from the program
        /// </summary>
        public event EventHandler<ConsoleDataRecievedEventArgs> DataRecieved;

        /// <summary>
        /// The underlying system process
        /// </summary>
        public Process Process { get; private set; }
        /// <summary>
        /// The path of the application being run
        /// </summary>
        public string ApplicationPath { get; private set; }

        /// <summary>
        /// Command line arguments passed to the program at startup
        /// </summary>
        public string[] Arguments { get; private set; }

        /// <summary>
        /// Tag this object with some data
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Create a new console process ready to be started
        /// </summary>
        /// <param name="applicationPath">The path of the program to run</param>
        /// <param name="arguments">The arguments to pass to the program</param>
        public ConsoleProcess(string applicationPath, params string[] arguments)
        {
            this.ApplicationPath = applicationPath;
            this.Arguments = arguments;
        }

        /// <summary>
        /// Start the progress and begin listening for messages
        /// </summary>
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

        /// <summary>
        /// Handle process exited
        /// </summary>
        /// <param name="sender">The sending process</param>
        /// <param name="e">The event args</param>
        private void proc_Exited(object sender, EventArgs e)
        {
            Process.WaitForExit();
            ProcessExited.RaiseEvent(this);
        }

        /// <summary>
        /// Error data recieved from the process
        /// </summary>
        /// <param name="sender">The sending process</param>
        /// <param name="e">The data recieved</param>
        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataRecieved.RaiseEvent(this, new ConsoleDataRecievedEventArgs(e.Data, true));
        }

        /// <summary>
        /// Data recieved from the process
        /// </summary>
        /// <param name="sender">The sending process</param>
        /// <param name="e">The data recieved</param>
        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            DataRecieved.RaiseEvent(this, new ConsoleDataRecievedEventArgs(e.Data, false));
        }
    }
}
