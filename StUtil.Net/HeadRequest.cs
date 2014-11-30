using System.Net;

namespace StUtil.Net
{
	/// <summary>
	/// Description of HEAD.
	/// </summary>
    public class HeadRequest : NetRequest<WebHeaderCollection>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="HeadRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
		public HeadRequest(string url) : base(url)
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public HeadRequest(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <param name="request">The request.</param>
		protected override void BuildRequest(ref HttpWebRequest request)
		{
			request.Method = "HEAD";
		}

        /// <summary>
        /// Handles the response.
        /// </summary>
        /// <param name="httpWebResponse">The HTTP web response.</param>
        /// <returns></returns>
		protected override WebHeaderCollection HandleResponse(ref HttpWebResponse httpWebResponse)
		{
			return httpWebResponse.Headers;
		}
	}
}
