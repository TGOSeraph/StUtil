using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    [Serializable]
    public class TextMessage : SerializableMessage
    {
        public string Text { get; set; }

        public TextMessage(byte[] data)
        {
            Text = System.Text.Encoding.Unicode.GetString(data);
        }

        public TextMessage(string methodName)
        {
            this.Text = methodName;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
