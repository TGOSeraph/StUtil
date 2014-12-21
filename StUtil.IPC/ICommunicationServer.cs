using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public interface ICommunicationServer
    {
        event EventHandler<EventArgs<ICommunicationConnection>> ConnectionRecieved;

        bool MultipleStreams { get; }
        bool IsListening { get; }
        void Start(ICommunicationInitialisation initArgs);
        void Stop();
    }
}
