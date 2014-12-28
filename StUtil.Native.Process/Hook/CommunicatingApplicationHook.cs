using StUtil.Native.Hook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StUtil.Extensions;
using StUtil.Generic;

namespace StUtil.Native.Hook
{
    public abstract class CommunicatingApplicationHook : ApplicationHook
    {
        public event EventHandler<EventArgs<IPC.IConnectionMessage>> MessageReceived;
        public event EventHandler ConnectionClosed;

        private StUtil.Native.Process.RemoteProcess remoteProcess;
        private StUtil.IPC.ICommunicationConnection connection;

        public string PipeId { get; private set; }

        public CommunicatingApplicationHook(System.Diagnostics.Process targetProcess, IEnumerable<string> args)
            : base(targetProcess)
        {
            PipeId = args.First();
            IPC.NamedPipes.NamedPipeClient client = new IPC.NamedPipes.NamedPipeClient();
            connection = client.Connect(new IPC.NamedPipes.NamedPipeInitialisation(PipeId));
        }

        public CommunicatingApplicationHook(System.Diagnostics.Process targetProcess)
            : base(targetProcess)
        {
            remoteProcess = new Native.Process.RemoteProcess(targetProcess);
            IPC.NamedPipes.NamedPipeServer server = new IPC.NamedPipes.NamedPipeServer();

            PipeId = Guid.NewGuid().ToString();

            Thread t = new Thread(() =>
            {
                server.ConnectionRecieved += (s, e) =>
                {
                    connection = e.Value;
                    while (e.Value.IsConnected)
                    {
                        var msg = e.Value.Receive();
                        if (e.Value.IsConnected && msg != null)
                        {
                            MessageReceived.RaiseEvent(this, msg);
                        }
                    }
                    ConnectionClosed.RaiseEvent(this);
                };
                server.Start(new IPC.NamedPipes.NamedPipeInitialisation(PipeId));
            });
            t.Start();
        }

        public override void Hook(string args = "")
        {
            base.Hook(PipeId);
        }

        bool first = true;
        protected void SendMessage(IPC.IConnectionMessage message)
        {
            if (first)
            {
                first = false;
            }
            connection.Send(message);
        }
    }
}
