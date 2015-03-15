using StUtil.Extensions;
using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Net
{
    public class DownloadRequest : GetRequest<string>
    {
        public event EventHandler<EventArgs<long>> DataReceived;

        public string FileName { get; private set; }
        public long ResumePosition { get; set; }
        public bool Stop { get; set; }

        public DownloadRequest(string url, CookieContainer cookies, string fileName)
            : base(url, cookies)
        {
            this.FileName = fileName;
            this.ResumePosition = 0;
            this.Stop = false;
        }

        public DownloadRequest(string url, string fileName)
            : this(url, null, fileName)
        {
        }

        protected override void BuildRequest(ref HttpWebRequest request)
        {
            if (ResumePosition > 0)
            {
                request.AddRange(ResumePosition);
            }
            base.BuildRequest(ref request);
        }

        protected override string HandleResponse(ref System.Net.HttpWebResponse httpWebResponse)
        {
            long bytesReceived = ResumePosition;
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (Stream responseStream = GetResponseStream(ref httpWebResponse))
                {
                    //Read bytes to buffer and write them to the file
                    byte[] downBuffer = new byte[8192];
                    int bytesRead = 0;
                    while ((bytesRead = responseStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        fs.Write(downBuffer, 0, bytesRead);
                        bytesReceived += bytesRead;
                        DataReceived.RaiseEvent(this, bytesReceived);

                        if (Stop) return null;
                    }
                }
            }
            return FileName;
        }
    }
}
