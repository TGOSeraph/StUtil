using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StUtil.Net.Download.Http
{
    public class HttpDownloadModule : DownloadModule<HttpPreparedDownload>
    {
        private ConcurrentDictionary<HttpPreparedDownload, DownloadRequest> downloads = new ConcurrentDictionary<HttpPreparedDownload, DownloadRequest>();
        private ConcurrentDictionary<HttpPreparedDownload, ManualResetEventSlim> blocks = new ConcurrentDictionary<HttpPreparedDownload, ManualResetEventSlim>();

        public override void StartDownload(HttpPreparedDownload download)
        {
            Task.Run(() =>
            {
                DownloadRequest req = new DownloadRequest(download.Url, download.FilePath);

                downloads.TryAdd(download, req);

                if (download.FileSize == 0)
                {
                    try
                    {
                        StUtil.Net.HeadRequest head = new HeadRequest(download.Url) { Timeout = 5000 };
                        int sz = 0;
                        int.TryParse(head.Run()["Content-Length"], out sz);
                        download.FileSize = sz;
                    }
                    catch (Exception)
                    {
                        //Ignore head error
                    }
                }

                if (File.Exists(download.FilePath))
                {
                    download.BytesReceived = new FileInfo(download.FilePath).Length;
                    if (download.BytesReceived == 0)
                    {
                        File.Delete(download.FilePath);
                    }
                }

                bool unknownFileSize = download.FileSize == 0;
                if (download.BytesReceived < download.FileSize || unknownFileSize)
                {
                    req.Timeout = 5000;
                    req.ResumePosition = download.BytesReceived;
                    req.TryIgnoreError = false;
                    req.DataReceived += (sender, e) =>
                    {
                        download.BytesReceived = e.Value;
                        if (unknownFileSize)
                        {
                            download.FileSize = e.Value + (e.Value / 100);
                        }
                    };
                    req.Run();
                }
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    download.LastError = t.Exception;
                    download.State = DownloadState.Failed;
                }
                else if (downloads[download].Stop)
                {
                }
                else
                {
                    download.FileSize = download.BytesReceived;
                    download.State = DownloadState.Completed;
                }
                Unblock(download);
            });
        }


        private void Unblock(HttpPreparedDownload download)
        {
            if (blocks.ContainsKey(download))
            {
                blocks[download].Set();
            }
        }

        public override void StopDownload(HttpPreparedDownload download)
        {
            using (var block = blocks.AddOrUpdate(download, d => new ManualResetEventSlim(), (d, b) => b))
            {
                downloads[download].Stop = true;
                block.Wait();
                ManualResetEventSlim v;
                blocks.TryRemove(download, out v);
            }
        }
    }
}
