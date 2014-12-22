using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace StUtil.Dev.ConsoleTest
{
    internal class Program
    {
        static IPC.IPCProcess proc;

        private static void Main(string[] args)
        {

            proc = new IPC.IPCProcess(new IPC.NamedPipes.NamedPipeInitialisation("TestPipeezx"));
            Console.WriteLine(proc.IsClient ? "Client" : "Server");
            if (proc.IsClient)
            {
                proc.MessageReceived += proc_MessageReceived;
                proc.Start();
            }
            else
            {
                proc.StartChild();
                proc.Start();
                Console.WriteLine(proc.SendAndReceive(new IPC.TextMessage("Test1")));
                Console.WriteLine(proc.SendAndReceive(new IPC.TextMessage("Test2")));
                Console.WriteLine(proc.SendAndReceive(new IPC.TextMessage("Test3")));
                proc.StopChild();
                Console.ReadKey(true);
            }
        }

        static void proc_MessageReceived(object sender, StUtil.Generic.EventArgs<IPC.IConnectionMessage> e)
        {
            if (e.Value != null)
            {
                MessageBox.Show(((IPC.TextMessage)e.Value).Text);
                proc.Send(new IPC.TextMessage(((IPC.TextMessage)e.Value).Text + ":"));
            }
        }

    }
}