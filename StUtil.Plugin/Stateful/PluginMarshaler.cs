using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace StUtil.Plugins.Stateful
{
    internal class PluginMarshaler<TPlugin> : StUtil.Generic.ProxyInvoker
    {
        private static MethodInfo reloadMethod;
        private static MethodInfo unloadMethod;

        internal event EventHandler UnloadRequested;
        internal event EventHandler ReloadRequested;

        static PluginMarshaler()
        {
            Type t = typeof(Plugin);
            reloadMethod = t.GetMethod("Reload", new Type[] { });
            unloadMethod = t.GetMethod("Unload", new Type[] { });
        }

        public override bool ShouldProxy(System.Reflection.MethodInfo method)
        {
            return true;
        }

        public override object Invoke(System.Reflection.MethodInfo method, object target, object[] args, Type returnType)
        {
            object val = null;
            if (method.ReturnType == typeof(void))
            {
                method.Invoke(target, args);
            }
            else
            {
                byte[] ret = (byte[])typeof(Plugin)
                   .GetMethod("GetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(object[]) }, null)
                   .Invoke(target, new object[] { method.Name, args });

                if (ret != null)
                {
                    using (MemoryStream ms = new MemoryStream(ret))
                    {
                        BinaryFormatter bFormatter = new BinaryFormatter();
                        val = (object)bFormatter.Deserialize(ms);
                    }
                }
            }

            if (method.Name == "Unload" && method.GetParameters().Length == 0)
            {
                if (UnloadRequested != null) UnloadRequested(this, EventArgs.Empty);
            }
            else if (method.Name == "Reload" && method.GetParameters().Length == 0)
            {
                if (ReloadRequested != null) ReloadRequested(this, EventArgs.Empty);
            }

            return val;
        }
    }
}
