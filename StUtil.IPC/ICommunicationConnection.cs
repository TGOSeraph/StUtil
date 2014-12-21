using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public interface ICommunicationConnection
    {
        event EventHandler Disconnected;

        event EventHandler<IConnectionMessage> MessageReceived;
        void Send(IConnectionMessage message);
        void Disconnect();
    }
}
