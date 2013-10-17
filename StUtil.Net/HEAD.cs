using System.Net;

namespace StUtil.Net
{
	/// <summary>
	/// Description of HEAD.
	/// </summary>
	public class HEAD : Base<WebHeaderCollection>
	{
		public HEAD(string url) : base(url)
		{
		}
        public HEAD(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
		protected override void BuildRequest(ref HttpWebRequest request)
		{
			request.Method = "HEAD";
		}
		
		protected override WebHeaderCollection HandleResponse(ref HttpWebResponse httpWebResponse)
		{
			return httpWebResponse.Headers;
		}
	}
}
