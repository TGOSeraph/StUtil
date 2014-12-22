using StUtil.Generic;
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
        bool IsConnected { get; }
        void Send(IConnectionMessage message);
        IConnectionMessage Receive();
        IConnectionMessage SendAndReceive(IConnectionMessage message);
        void Disconnect();
    }
}
