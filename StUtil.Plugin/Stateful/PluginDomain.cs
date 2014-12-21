using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StUtil.Plugins.Stateful
{
    public class PluginDomain<TPlugin> : MarshalByRefObject where TPlugin : MarshalByRefObject
    {
        private Assembly assembly;
        private string assemblyFilePath;

        public PluginDomain(string assemblyFilePath)
        {
            this.assemblyFilePath = assemblyFilePath;
        }

        public TPlugin Create(string name = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.Load(System.IO.File.ReadAllBytes(assemblyFilePath));
            }
            var plugins = assembly.GetTypes().Where(t => (name == null || name == t.Name || name == t.FullName) && typeof(TPlugin).IsAssignableFrom(t));
            if (plugins.Count() > 1)
            {
                throw new ApplicationException("Multiple plugins found in assembly, you must specify the type name of the plugin to load");
            }
            else
            {
                return (TPlugin)Activator.CreateInstance(plugins.First());
            }
        }

        public IEnumerable<TPlugin> Create()
        {
            if (assembly == null)
            {
                assembly = Assembly.Load(System.IO.File.ReadAllBytes(assemblyFilePath));
            }
            return assembly
                .GetTypes()
                .Where(t => typeof(TPlugin).IsAssignableFrom(t))
                .Select(t => (TPlugin)Activator.CreateInstance(t));
        }
    }
}
