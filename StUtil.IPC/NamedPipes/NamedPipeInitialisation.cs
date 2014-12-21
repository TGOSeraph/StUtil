using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC.NamedPipes
{
    public class NamedPipeInitialisation : ICommunicationInitialisation
    {
        public string PipeId { get; private set; }

        public NamedPipeInitialisation(string pipeId)
        {
            this.PipeId = pipeId;
        }
    }
}
