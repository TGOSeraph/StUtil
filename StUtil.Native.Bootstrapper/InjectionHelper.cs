using StUtil.Native.Internal;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace StUtil.Native.Bootstrapper
{
    internal static class InjectionHelper
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            StUtil.IPC.NamedPipes.NamedPipeClient client = new IPC.NamedPipes.NamedPipeClient();
            var conn = client.Connect(new StUtil.IPC.NamedPipes.NamedPipeInitialisation(args.Last()));
            var msg = conn.Receive() as StUtil.Native.Process.InjectionMessage;

            StUtil.Native.Process.RemoteProcess proc = new Process.RemoteProcess(System.Diagnostics.Process.GetProcessById(msg.ProcessId));
            proc.Open();
            try
            {
                IntPtr val = proc.LoadDotNetModule(msg.File, msg.Type, msg.Method, msg.Args);
                conn.Send(new IPC.ValueMessage<IntPtr>(val));
            }
            catch (Exception ex)
            {
                conn.Send(new IPC.ValueMessage<Exception>(ex));
            }
        }
    }
}