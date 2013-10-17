using System.Net;

namespace StUtil.Net
{
    /// <summary>
    /// Description of GET.
    /// </summary>
    public abstract class GET<T> : Base<T>
    {
        public GET(string url)
            : base(url)
        {
        }
        public GET(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }

        protected override void BuildRequest(ref HttpWebRequest request)
        {
            request.Method = "GET";
        }
    }

    public class GET : GET<string>
    {
        public GET(string url)
            : base(url)
        {
        }
        public GET(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }

        protected override string HandleResponse(ref HttpWebResponse httpWebResponse)
        {
            return base.GetResponseString(ref httpWebResponse);
        }
    }
}
