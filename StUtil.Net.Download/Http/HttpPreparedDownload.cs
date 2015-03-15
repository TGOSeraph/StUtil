using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download.Http
{
    public class HttpPreparedDownload : PreparedDownload<HttpDownload>
    {
        public bool AllowResume { get; set; }
        public string Url { get; set; }

        public HttpPreparedDownload(HttpDownload download)
            : base(download)
        {
            this.AllowResume = true;

            this.Url = download.Url;
        }

        protected override Task PrepareDownload(string downloadDirectory)
        {
            base.FilePath = Path.Combine(downloadDirectory, Path.GetFileName(Item.Url));
            return Task.FromResult(true);
        }
    }

    public class HttpPreparedDownload<TDownload> : HttpPreparedDownload where TDownload : HttpDownload
    {
        public new TDownload Item
        {
            get
            {
                return (TDownload)base.Item;
            }
        }

        public HttpPreparedDownload(TDownload download)
            : base(download)
        {
            this.Url = download.Url;
        }
    }
}
