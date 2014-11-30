using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StUtil.Plugins
{
    public class PluginLoader<TPlugin> : MarshalByRefObject where TPlugin : Plugin
    {
        private PluginMarshaler<TPlugin> marshaler;
        private PluginDomain<TPlugin> remotePlugin;
        private AppDomain appDomain;

        private Dictionary<TPlugin, Proxy<TPlugin>> plugins;

        private static List<FieldInfo> maintainFields;
        private static MethodInfo getFieldData;
        private static MethodInfo setFieldData;

        public string AssemblyFilePath { get; private set; }
        public bool IsDomainCreated
        {
            get
            {
                return appDomain != null;
            }
        }

        public PluginLoader(string assemblyFilePath)
        {
            AssemblyFilePath = assemblyFilePath;
            plugins = new Dictionary<TPlugin, Proxy<TPlugin>>();
            marshaler = new PluginMarshaler<TPlugin>();

            marshaler.ReloadRequested += marshaler_ReloadRequested;
            marshaler.UnloadRequested += marshaler_UnloadRequested;

            Type t = typeof(TPlugin);

            getFieldData = typeof(Plugin).GetMethod("GetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(FieldInfo) }, null);
            setFieldData = typeof(Plugin).GetMethod("SetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(FieldInfo), typeof(byte[]) }, null);

            var attrs = t.GetCustomAttributes(typeof(StatefulAttribute), true);
            bool all = attrs.Length > 0 && ((StatefulAttribute)attrs[0]).MaintainsState;

            maintainFields = new List<FieldInfo>();

            foreach (FieldInfo f in t.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                attrs = f.GetCustomAttributes(typeof(StatefulAttribute), true);
                bool add = all;
                if (attrs.Length > 0)
                {
                    add = ((StatefulAttribute)attrs[0]).MaintainsState;
                }
                if (add)
                {
                    maintainFields.Add(f);
                }
            }
        }

        void marshaler_UnloadRequested(object sender, EventArgs e)
        {
            UnloadDomain();
        }

        void marshaler_ReloadRequested(object sender, EventArgs e)
        {
            ReloadDomain();
        }

        public TPlugin Create(string typeName = null)
        {
            if (!IsDomainCreated)
            {
                CreateDomain();
            }
            Proxy<TPlugin> proxy = new Proxy<TPlugin>(marshaler, remotePlugin.Create(typeName));
            TPlugin plugin = (TPlugin)proxy.GetTransparentProxy();
            plugins.Add(plugin, proxy);
            plugin.Disposed += plugin_Disposed;
            return plugin;
        }

        private void plugin_Disposed(object sender, EventArgs e)
        {
            TPlugin plugin = (TPlugin)sender;
            plugin.Disposed -= plugin_Disposed;
            plugins.Remove(plugin);
        }

        private void CreateDomain()
        {
            appDomain = AppDomain.CreateDomain(typeof(TPlugin).FullName);
            remotePlugin = (PluginDomain<TPlugin>)appDomain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName,
                typeof(PluginDomain<TPlugin>).FullName,
                false,
                System.Reflection.BindingFlags.Default,
                null,
                new object[] { AssemblyFilePath },
                System.Globalization.CultureInfo.CurrentCulture,
                null);
        }

        public void UnloadDomain()
        {
            foreach (TPlugin p in plugins.Keys.ToArray())
            {
                p.Dispose();
            }
            AppDomain.Unload(appDomain);
            remotePlugin = null;
            appDomain = null;
        }

        public void ReloadDomain()
        {
            var proxies = plugins.Values.ToArray();
            Dictionary<Proxy<TPlugin>, Dictionary<FieldInfo, object>> fieldValues = new Dictionary<Proxy<TPlugin>, Dictionary<FieldInfo, object>>();

            foreach (var proxy in proxies)
            {
                Dictionary<FieldInfo, object> fields = new Dictionary<FieldInfo, object>();

                foreach (FieldInfo fi in maintainFields)
                {
                    fields.Add(fi, getFieldData.Invoke(proxy.Target, new object[] { fi }));
                }

                fieldValues.Add(proxy, fields);
            }

            UnloadDomain();
            CreateDomain();

            foreach (var proxy in proxies)
            {
                proxy.Target = remotePlugin.Create();
                foreach (var kvp in fieldValues[proxy])
                {
                    setFieldData.Invoke(proxy.Target, new object[] { kvp.Key, kvp.Value });
                }
            }
        }
    }
}
