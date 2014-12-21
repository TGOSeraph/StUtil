using System;
using System.Collections.Generic;
namespace StUtil.Plugins
{
   public interface IPluginLoader<TPlugin>
    {
        string AssemblyFilePath { get; }
        IEnumerable<TPlugin> Load();
        TPlugin Load(string typeName);
    }
}
