using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace InvokeHelper.Plugin
{
    public abstract class Plugin : MarshalByRefObject, IDisposable
    {
        public event EventHandler Disposed;

        [Stateful(false)]
        private bool isDisposed = false;

        public bool IsDisposed
        {
            get
            {
                return isDisposed;
            }
        }

        public virtual void Unload() { }
        public virtual void Reload() { }

        private byte[] Serialize(object obj)
        {
            if (obj == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private object Deserialize(byte[] data)
        {
            if (data == null) return null;

            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                return bFormatter.Deserialize(ms);
            }
        }

        private byte[] GetData(string method, object[] args)
        {
            object ret = this.GetType().GetMethod(method, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Invoke(this, args);

            return Serialize(ret);
        }

        private byte[] GetData(FieldInfo field)
        {
            return Serialize(field.GetValue(this));
        }

        private void SetData(FieldInfo field, byte[] data)
        {
            field.SetValue(this, Deserialize(data));
        }

        public void Dispose()
        {
            isDisposed = true;
            if (Disposed != null) Disposed(this, EventArgs.Empty);
        }
    }
}
