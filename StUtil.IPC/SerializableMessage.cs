using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    [Serializable]
    public abstract class SerializableMessage : IConnectionMessage
    {
        private static IFormatter formatter = new BinaryFormatter();

        public byte[] ToRawData()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        public static T FromRawData<T>(byte[] data) where T : SerializableMessage
        {
             using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                return (T)formatter.Deserialize(ms);
            }
        }

        public static SerializableMessage FromRawData(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (SerializableMessage)formatter.Deserialize(ms);
            }
        }
    }
}
