using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    [Serializable]
    public class ValueMessage<T> : SerializableMessage
    {
        public T Value { get; set; }

        public ValueMessage()
        {
        }

        public ValueMessage(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return "{" + Value.ToString() + "}";
        }
    }
}
