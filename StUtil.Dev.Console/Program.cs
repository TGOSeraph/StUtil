using System.Linq;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace StUtil.Dev.ConsoleTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Process p = Process.GetProcessesByName("notepad").FirstOrDefault() ?? Process.Start(@"C:\Windows\system32\notepad.exe");
            StUtil.Native.Hook.ApplicationMessageHook hook = new b(p);
            hook.Hook();


            StUtil.Native.Process.RemoteProcess rp = new Native.Process.RemoteProcess(p);
            rp.Open();
            IPC.NamedPipes.NamedPipeServer server = new IPC.NamedPipes.NamedPipeServer();

            string id = Guid.NewGuid().ToString();

            Thread t = new Thread(() =>
            {
                server.ConnectionRecieved += (s, e) =>
                {
                    int i = 0;
                    while (e.Value.IsConnected)
                    {
                        var msg = e.Value.Receive();
                        if (e.Value.IsConnected && msg != null)
                        {
                            Console.WriteLine((++i).ToString() + msg.ToString());
                        }
                    }
                    Console.WriteLine("Done");
                    Console.ReadKey(true);
                };
                server.Start(new IPC.NamedPipes.NamedPipeInitialisation(id));
            });
            t.Start();

            IntPtr v = rp.LoadDotNetModule(Application.ExecutablePath, typeof(Program).FullName, "Main2", id);
            if (v.ToInt32() < 0)
            {
                Marshal.ThrowExceptionForHR(v.ToInt32());
            }
        }

        public static int Main2(string args)
        {
            IPC.NamedPipes.NamedPipeClient client = new IPC.NamedPipes.NamedPipeClient();
            var x = new c(client.Connect(new IPC.NamedPipes.NamedPipeInitialisation(args)));
            x.Hook();
            return 1;
        }

        class b : StUtil.Native.Hook.ApplicationMessageHook
        {
            public b(Process p)  : base(p)
            {
            }

            protected override void MessageReceived(object sender, Generic.EventArgs<Message> e)
            {
                throw new NotImplementedException();
            }
        }

        class c : StUtil.Native.Hook.ApplicationMessageHook
        {
            private IPC.ICommunicationConnection conn;

            public c(IPC.ICommunicationConnection conn)
                : base(Process.GetCurrentProcess())
            {
                this.conn = conn;
            }

            protected override void MessageReceived(object sender, Generic.EventArgs<Message> e)
            {
                if (e.Value.Msg < 0x150 && e.Value.Msg > 0x100)
                {
                    this.conn.Send(new IPC.ValueMessage<string>(e.Value.ToString()));
                }
            }
        }
    }
}