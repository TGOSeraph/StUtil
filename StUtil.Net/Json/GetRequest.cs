using System.Net;
using System.Web.Script.Serialization;

namespace StUtil.Net.JSON
{
    /// <summary>
    /// Create a JSON GET request
    /// </summary>
    /// <typeparam name="T">The type of object to return</typeparam>
    public class GetRequest<T> : StUtil.Net.GetRequest<T>
    {
        /// <summary>
        /// The json serializer
        /// </summary>
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

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
        /// Handles the response.
        /// </summary>
        /// <param name="httpWebResponse">The HTTP web response.</param>
        /// <returns></returns>
        protected override T HandleResponse(ref System.Net.HttpWebResponse httpWebResponse)
        {
            if (typeof(object) == typeof(T))
            {
                return (T)StUtil.Data.Dynamic.JsonConverter.Deserialize(base.GetResponseString(ref httpWebResponse));
            }
            else
            {
                return jss.Deserialize<T>(base.GetResponseString(ref httpWebResponse));
            }
        }
    }

    /// <summary>
    /// Create a JSON GET request
    /// </summary>
    public class GetRequest : StUtil.Net.JSON.GetRequest<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public GetRequest(string url)
            : base(url)
        {
        }
    }
}