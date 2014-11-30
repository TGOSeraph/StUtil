using System.Net;

namespace StUtil.Net
{
    /// <summary>
    /// Description of GET.
    /// </summary>
    public abstract class GetRequest<T> : NetRequest<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public GetRequest(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public GetRequest(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }

        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void BuildRequest(ref HttpWebRequest request)
        {
            request.Method = "GET";
        }
    }

    public class GetRequest : GetRequest<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public GetRequest(string url)
            : base(url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public GetRequest(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }

        /// <summary>
        /// Handles the response.
        /// </summary>
        /// <param name="httpWebResponse">The HTTP web response.</param>
        /// <returns></returns>
        protected override string HandleResponse(ref HttpWebResponse httpWebResponse)
        {
            return base.GetResponseString(ref httpWebResponse);
        }
    }
}