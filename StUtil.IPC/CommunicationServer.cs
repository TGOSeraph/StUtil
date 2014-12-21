using StUtil.Extensions;
using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public abstract class CommunicationServer : ICommunicationServer
    {
        public event EventHandler<EventArgs<ICommunicationConnection>> ConnectionRecieved;

        private bool listening = false;
        public bool IsListening
        {
            get { return listening; }
        }

        public bool Stopping { get; private set; }
        private bool _stop = false;

        private List<ICommunicationConnection> connections;
        public IEnumerable<ICommunicationConnection> Connections
        {
            get
            {
                return connections;
            }
        }

        public abstract bool MultipleStreams
        {
            get;
        }

        public virtual void Start(ICommunicationInitialisation initArgs)
        {
            if (listening)
            {
                return;
            }
            OnStart(initArgs);
            while (!_stop)
            {
                ICommunicationConnection conn = WaitForConnection();
                connections.Add(conn);
                conn.Disconnected += conn_Disconnected;
                if (ShouldFireConnectionReceived())
                {
                    OnConnectionReceived(conn);
                }
            }
            Stopping = false;
        }

        protected virtual void OnStart(ICommunicationInitialisation initArgs)
        {
        }

        void conn_Disconnected(object sender, EventArgs e)
        {
            ICommunicationConnection conn = (ICommunicationConnection)sender;
            connections.Remove(conn);
            conn.Disconnected -= conn_Disconnected;
        }

        protected abstract ICommunicationConnection WaitForConnection();

        protected virtual void OnConnectionReceived(ICommunicationConnection connection)
        {
            ConnectionRecieved.RaiseEvent(this, connection);
        }

        protected virtual bool ShouldFireConnectionReceived()
        {
            if (MultipleStreams)
            {
                return true;
            }
            else
            {
                return connections.Count == 1;
            }
        }

        public void Stop()
        {
            Stopping = true;
            _stop = true;
        }
    }
}
