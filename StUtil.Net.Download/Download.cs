using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public abstract class Download
    {
        public abstract DownloadModule Module { get; }
    }

    public abstract class Download<TModule> : Download where TModule : DownloadModule
    {
        public override DownloadModule Module
        {
            get { return DownloadModuleFactory.GetModule<TModule>(); }
        }
    }
}
