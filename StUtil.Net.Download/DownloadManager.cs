using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;
using StUtil.Generic;
using System.Collections.Concurrent;
using System.IO;

namespace StUtil.Net.Download
{
    public class DownloadManager
    {
        public event EventHandler<EventArgs<IEnumerable<PreparedDownload>>> DownloadsAdded;

        private BlockingCollection<DownloadProvider> providers = new BlockingCollection<DownloadProvider>();
        private HashSet<PreparedDownload> registered = new HashSet<PreparedDownload>();

        private ConcurrentQueue<PreparedDownload> queued = new ConcurrentQueue<PreparedDownload>();
        public IEnumerable<PreparedDownload> Queued
        {
            get
            {
                return queued;
            }
        }

        private BlockingCollection<PreparedDownload> active = new BlockingCollection<PreparedDownload>();
        public IEnumerable<PreparedDownload> Active
        {
            get
            {
                return active;
            }
        }

        private BlockingCollection<PreparedDownload> completed = new BlockingCollection<PreparedDownload>();
        public IEnumerable<PreparedDownload> Completed
        {
            get
            {
                return completed;
            }
        }

        private BlockingCollection<PreparedDownload> stopped = new BlockingCollection<PreparedDownload>();
        public IEnumerable<PreparedDownload> Stopped
        {
            get
            {
                return stopped;
            }
        }

        private int maxConcurrentDownloads;
        public int MaximumConcurrentDownloads
        {
            get
            {
                return maxConcurrentDownloads;
            }
            set
            {
                maxConcurrentDownloads = value;
                ProcessQueue();
            }
        }

        public string DownloadDirectory { get; set; }

        public DownloadManager(string downloadDirectory)
        {
            this.DownloadDirectory = downloadDirectory;
            this.maxConcurrentDownloads = 4;
        }

        public void RegisterProvider(DownloadProvider provider)
        {
            providers.Add(provider);
            provider.ProcessDownloads += provider_ProcessDownloads;
        }

        private void provider_ProcessDownloads(object sender, StUtil.Generic.EventArgs<IEnumerable<Download>> e)
        {
            DownloadProvider provider = (DownloadProvider)sender;
            var downloads = e.Value.Select(dl => (PreparedDownload)Activator.CreateInstance(provider.PreparedDownloadType, dl)).ToArray();
            DownloadsAdded.RaiseEvent(this, downloads);

            foreach (var dl in downloads)
            {
                dl.StateChanged += dl_StateChanged;
                registered.Add(dl);
                try
                {
                    dl.State = DownloadState.Processing;
                    dl.Prepare(DownloadDirectory).ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            dl.LastError = t.Exception.InnerException ?? t.Exception;
                            dl.State = DownloadState.Failed;
                        }
                        else
                        {
                            try
                            {
                                if (string.IsNullOrWhiteSpace(dl.FilePath))
                                {
                                    throw new FormatException("Download FilePath must be set to a valid path");
                                }
                                Path.GetFullPath(dl.FilePath);
                            }
                            catch (Exception ex)
                            {
                                dl.LastError = ex;
                                dl.State = DownloadState.Failed;
                                return;
                            }
                            dl.State = DownloadState.Queued;
                            ProcessQueue();
                        }
                    });
                }
                catch (Exception ex)
                {
                    dl.LastError = ex;
                    dl.State = DownloadState.Failed;
                }
            }
        }

        private void dl_StateChanged(object sender, ValueChangedEventArgs<DownloadState> e)
        {
            PreparedDownload download = (PreparedDownload)sender;
            switch (e.NewValue)
            {
                case DownloadState.Active:
                    break;
                case DownloadState.Completed:
                    RemoveActive(download);
                    completed.Add(download);
                    break;
                case DownloadState.Failed:
                    RemoveActive(download);
                    stopped.Add(download);
                    break;
                case DownloadState.Processing:
                    break;
                case DownloadState.Queued:
                    lock (queued)
                    {
                        if (!queued.Contains(download))
                        {
                            queued.Enqueue(download);
                        }
                    }
                    break;
                case DownloadState.Stopped:
                    RemoveActive(download);
                    stopped.Add(download);
                    break;
            }
        }

        private void RemoveActive(PreparedDownload download)
        {
            lock (active)
            {
                PreparedDownload dl;
                while (active.Contains(download))
                {
                    if (active.TryTake(out dl))
                    {
                        break;
                    }
                }
            }
            ProcessQueue();
        }

        private void ProcessQueue()
        {
            //Dont let anything touch the active downloads when we are processing the queue
            lock (active)
            {
                lock (queued)
                {
                    if (!queued.IsEmpty)
                    {
                        if (active.Count < maxConcurrentDownloads)
                        {
                            PreparedDownload dl = null;
                            while (!queued.IsEmpty && !queued.TryDequeue(out dl))
                            {
                            }
                            if (dl != null)
                            {
                                StartDownloadUnsafe(dl);
                            }
                        }
                    }
                }
            }
        }

        public void StartDownload(PreparedDownload download)
        {
            lock (active)
            {
                StartDownloadUnsafe(download);
            }
        }

        private void StartDownloadUnsafe(PreparedDownload download)
        {
            if (!registered.Contains(download))
            {
                download.StateChanged += dl_StateChanged;
                registered.Add(download);
                DownloadsAdded.RaiseEvent(this, new[] { download });
            }
            Directory.CreateDirectory(Path.GetDirectoryName(download.FilePath));
            active.Add(download);
            download.State = DownloadState.Active;
            download.Item.Module.StartDownload(download);
        }

        public void StopDownload(PreparedDownload download)
        {
            download.Item.Module.StopDownload(download);
            download.State = DownloadState.Stopped;
        }
    }
}
