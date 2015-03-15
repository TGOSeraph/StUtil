using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net.Download.Http
{
    public class HttpDownload : Download<HttpDownloadModule>
    {
        public string Url { get; set; }

        public HttpDownload(string url)
        {
            this.Url = url;
        }
    }
}
