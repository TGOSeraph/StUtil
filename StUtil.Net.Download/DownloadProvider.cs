using StUtil.Extensions;
using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public abstract class DownloadProvider
    {
        public event EventHandler<EventArgs<IEnumerable<Download>>> ProcessDownloads;

        public abstract Type PreparedDownloadType { get; }

        protected void OnProcessDownloads(IEnumerable<Download> downloads)
        {
            ProcessDownloads.RaiseEvent(this, downloads);
        }
    }

    public abstract class DownloadProvider<TDownload, TPrepared> : DownloadProvider
        where TDownload : Download
        where TPrepared : PreparedDownload
    {
        public override Type PreparedDownloadType
        {
            get { return typeof(TPrepared); }
        }

        protected void OnProcessDownloads(IEnumerable<TDownload> downloads)
        {
            base.OnProcessDownloads(downloads);
        }
    }

    public class BasicDownloadProvider<TDownload, TPrepared> : DownloadProvider<TDownload, TPrepared>
        where TDownload : Download
        where TPrepared : PreparedDownload
    {
        public void TriggerProcessDownloads(IEnumerable<TDownload> downloads)
        {
            OnProcessDownloads(downloads);
        }
    }
}
