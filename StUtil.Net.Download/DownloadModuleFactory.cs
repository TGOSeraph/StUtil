using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public static class DownloadModuleFactory
    {
        private static Dictionary<Type, DownloadModule> instances = new Dictionary<Type, DownloadModule>();

        public static DownloadModule GetModule<TModule>() where TModule : DownloadModule
        {
            if (instances.ContainsKey(typeof(TModule)))
            {
                return instances[typeof(TModule)];
            }
            else
            {
                var mod = (DownloadModule)Activator.CreateInstance<TModule>();
                instances.Add(typeof(TModule), mod);
                return mod;
            }
        }
    }
}
