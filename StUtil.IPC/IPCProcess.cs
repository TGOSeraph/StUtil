using StUtil.Extensions;
using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public class IPCProcess : ICommunicationConnection
    {
        public event EventHandler<EventArgs<IConnectionMessage>> MessageReceived;

        private ICommunicationClient client;
        private ICommunicationServer server;
        private ICommunicationInitialisation initArgs;
        public Process ChildProcess { get; private set; }
        private ICommunicationConnection conn;

        public int ParentProcessId { get; private set; }
        public bool IsServer { get; private set; }
        public bool IsClient
        {
            get
            {
                return !IsServer;
            }
        }

        public IPCProcess(NamedPipes.NamedPipeInitialisation initArgs)
            : this(initArgs, new NamedPipes.NamedPipeServer(), new NamedPipes.NamedPipeClient())
        {
        }

        public IPCProcess(ICommunicationInitialisation initArgs, ICommunicationServer server, ICommunicationClient client)
        {
            this.initArgs = initArgs;
            this.client = client;
            this.server = server;
            string[] args = Environment.GetCommandLineArgs();
            string arg;
            if (args.Length == 1)
            {
                arg = args[0];
            }
            else
            {
                arg = args[1];
            }
            if (arg.StartsWith("$IPCParent{"))
            {
                IsServer = false;
                this.ParentProcessId = int.Parse(arg.Substring(11, arg.Length - 12));
            }
            else
            {
                IsServer = true;
            }
        }

        public void Start()
        {
            if (IsServer)
            {
                server.ConnectionRecieved += server_ConnectionRecieved;
                server.Start(initArgs);
            }
            else
            {
                conn = client.Connect(initArgs);
                while (conn.IsConnected)
                {
                    MessageReceived.RaiseEvent(this, conn.Receive());
                }
                Environment.Exit(1);
            }
        }

        public void StartChild()
        {
            if (ChildProcess != null && !ChildProcess.HasExited)
            {
                return;
            }
            using (Process curr = Process.GetCurrentProcess())
            {
                var asm = System.Reflection.Assembly.GetCallingAssembly();
                ChildProcess = Process.Start(asm.Location, "$IPCParent{" + curr.Id.ToString() + "}");
            }
        }

        private void server_ConnectionRecieved(object sender, Generic.EventArgs<ICommunicationConnection> e)
        {
            conn = e.Value;
            conn.Disconnected += conn_Disconnected;
        }

        private void conn_Disconnected(object sender, EventArgs e)
        {
            this.Disconnected.RaiseEvent(this);
        }

        public void StopChild()
        {
            ChildProcess.Kill();
            ChildProcess = null;
        }

        public event EventHandler Disconnected;

        public bool IsConnected
        {
            get { return conn.IsConnected; }
        }

        public void Send(IConnectionMessage message)
        {
            conn.Send(message);
        }

        public IConnectionMessage Receive()
        {
            return conn.Receive();
        }

        public void Disconnect()
        {
            conn.Disconnect();
        }

        public IConnectionMessage SendAndReceive(IConnectionMessage message)
        {
            Send(message);
            return Receive();
        }
    }
}
