using StUtil.Extensions;
using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download
{
    public abstract class PreparedDownload
    {
        public event EventHandler<ValueChangedEventArgs<DownloadState>> StateChanged;
        public event EventHandler DataReceived;

        /// <summary>
        /// The item that is being downloaded
        /// </summary>
        public Download Item { get; private set; }

        /// <summary>
        /// The path to the file on disk
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The total size of the source file
        /// </summary>
        public long FileSize { get; set; }

        private long bytesReceived = 0;
        /// <summary>
        /// The total amount of the file downloaded
        /// </summary>
        public long BytesReceived
        {
            get { return bytesReceived; }
            set
            {
                bytesReceived = value;
                DataReceived.RaiseEvent(this);
            }
        }

        /// <summary>
        /// Statistics such as average download speed, age
        /// </summary>
        public DownloadStatistics Statistics { get; private set; }

        private DownloadState state;
        /// <summary>
        /// The current state of the download
        /// </summary>
        public DownloadState State
        {
            get
            {
                return state;
            }
            set
            {
                var old = state;
                state = value;
                if (state == DownloadState.Completed)
                {
                    OnDownloadCompleted();
                }
                StateChanged.RaiseEvent(this, new ValueChangedEventArgs<DownloadState>(value, old));
            }
        }

        public Exception LastError { get; protected internal set; }

        public bool Prepared { get; private set; }

        public Task Prepare(string downloadDirectory)
        {
            return PrepareDownload(downloadDirectory).ContinueWith(t =>
            {
                this.Prepared = true;
            });
        }

        protected abstract Task PrepareDownload(string downloadDirectory);

        protected virtual void OnDownloadCompleted()
        {
        }

        public PreparedDownload(Download download)
        {
            this.Item = download;
            this.state = DownloadState.Idle;
            this.Statistics = new DownloadStatistics();
        }
    }

    public abstract class PreparedDownload<TDownload> : PreparedDownload where TDownload : Download
    {
        public new TDownload Item
        {
            get
            {
                return (TDownload)base.Item;
            }
        }

        public PreparedDownload(TDownload download)
            : base(download)
        {
        }
    }


    public enum DownloadState
    {
        [Description("The download is currently waiting to be started")]
        Queued,
        [Description("The download is being processed")]
        Processing,
        [Description("The download is currently running")]
        Active,
        [Description("The download is stopped pending start")]
        Stopped,
        [Description("The download has finished successfully")]
        Completed,
        [Description("An error occurred during the download")]
        Failed,
        [Description("The download has just been created")]
        Idle
    }
}
