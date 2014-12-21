using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
   public class CommunicationConnection : ICommunicationConnection
    {
        public event EventHandler Disconnected;

        public event EventHandler<IConnectionMessage> MessageReceived;

        public void Send(IConnectionMessage message)
        {
            
        }

        protected abstract void SendMessage(byte[] data);
        protected abstract void Listen();

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
