using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public abstract class DownloadModule
    {
        public abstract void StartDownload(PreparedDownload download);
        public abstract void StopDownload(PreparedDownload download);
    }

    public abstract class DownloadModule<TPrepared> : DownloadModule
        where TPrepared : PreparedDownload
    {
        public abstract void StartDownload(TPrepared download);
        public abstract void StopDownload(TPrepared download);

        public override void StartDownload(PreparedDownload download)
        {
            StartDownload((TPrepared)download);
        }

        public override void StopDownload(PreparedDownload download)
        {
            StopDownload((TPrepared)download);
        }
    }
}
