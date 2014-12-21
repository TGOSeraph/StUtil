using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Plugins
{
    public class PluginLoader<TPlugin> : IPluginLoader<TPlugin>
    {
        private string assemblyFilePath;
        private Assembly assembly;

        string IPluginLoader<TPlugin>.AssemblyFilePath
        {
            get { return assemblyFilePath; }
        }

        private void LoadAssembly()
        {
            if (assembly == null)
            {
                assembly = Assembly.LoadFrom(assemblyFilePath);
            }
        }

        TPlugin IPluginLoader<TPlugin>.Load(string typeName)
        {
            LoadAssembly();
            return (TPlugin)Activator.CreateInstance(assembly.GetType(typeName));
        }

        public IEnumerable<TPlugin> Load()
        {
            LoadAssembly();
            return assembly
                .GetTypes()
                .Where(t => typeof(TPlugin).IsAssignableFrom(t))
                .Select(t => (TPlugin)Activator.CreateInstance(t));
        }

        public PluginLoader(string assemblyFilePath)
        {
            this.assemblyFilePath = assemblyFilePath;
        }
    }
}
