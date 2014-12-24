using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    [Serializable]
    public class TextMessage : ValueMessage<string>
    {
        public TextMessage(string methodName) 
            : base(methodName)
        {
        }
    }
}
