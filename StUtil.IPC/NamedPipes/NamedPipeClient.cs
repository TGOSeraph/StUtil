using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC.NamedPipes
{
    public class NamedPipeClient : ICommunicationClient
    {
        public NamedPipeClient()
        {
        }

        public ICommunicationConnection Connect(ICommunicationInitialisation initArgs)
        {
            System.IO.Pipes.NamedPipeClientStream client = new System.IO.Pipes.NamedPipeClientStream(((NamedPipeInitialisation)initArgs).PipeId);
            client.Connect();
            return new NamedPipeConnection(client);
        }
    }
}
