using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC.NamedPipes
{
    public class NamedPipeServer : CommunicationServer
    {
        private NamedPipeServerStream server;

        public override bool MultipleStreams
        {
            get { return false; }
        }

        protected override void OnStart(ICommunicationInitialisation initArgs)
        {
            base.OnStart(initArgs);
            server = new NamedPipeServerStream(((NamedPipeInitialisation)initArgs).PipeId);
        }

        protected override ICommunicationConnection WaitForConnection()
        {
            server.WaitForConnection();
            return new NamedPipeConnection(server);
        }
    }
}
