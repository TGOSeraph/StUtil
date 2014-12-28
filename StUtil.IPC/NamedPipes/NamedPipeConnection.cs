using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;
using System.Threading;
using StUtil.Generic;
using System.IO;
using System.Diagnostics;

namespace StUtil.IPC.NamedPipes
{
    public class NamedPipeConnection : ICommunicationConnection
    {
        public event EventHandler Disconnected;

        private PipeStream stream;

        public NamedPipeConnection(System.IO.Pipes.PipeStream stream)
        {
            this.stream = stream;
        }

        private IConnectionMessage ToMessage(byte[] data)
        {
            if (data.Length == 0)
            {
                return null;
            }
            return SerializableMessage.FromRawData(data);
        }

        public void Send(IConnectionMessage message)
        {
            byte[] data = message.ToRawData();
            byte[] header = BitConverter.GetBytes(data.Length);
            try
            {
                stream.Write(header, 0, header.Length);
                stream.Write(data, 0, data.Length);
            }
            catch (IOException ioEx)
            {
                System.Windows.Forms.MessageBox.Show(ioEx.ToString());
                if (ioEx.HResult == -2147024664)
                {
                    Disconnect();
                }
                else
                {
                    throw;
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                stream.Close();
            }
            finally
            {
                stream.Dispose();
            }
            this.Disconnected.RaiseEvent(this);
        }

        public IConnectionMessage Receive()
        {
            byte[] buffer = new byte[sizeof(int)];
            stream.Read(buffer, 0, buffer.Length);
            byte[] data = new byte[BitConverter.ToInt32(buffer, 0)];
            stream.Read(data, 0, data.Length);
            return ToMessage(data);
        }

        public bool IsConnected
        {
            get { return stream.IsConnected; }
        }

        public IConnectionMessage SendAndReceive(IConnectionMessage message)
        {
            Send(message);
            return Receive();
        }
    }
}
