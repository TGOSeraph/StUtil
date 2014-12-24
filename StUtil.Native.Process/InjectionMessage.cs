using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Process
{
    [Serializable]
    public class InjectionMessage : IPC.SerializableMessage
    {
        public string File { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string Args { get; set; }
        public int ProcessId { get; set; }
    }
}
