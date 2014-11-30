using System.Net;
using System.Web.Script.Serialization;

namespace StUtil.Net.JSON
{
    /// <summary>
    /// Create a JSON POST request
    /// </summary>
    /// <typeparam name="T">The type of object to return</typeparam>
    public class PostRequest<T> : StUtil.Net.PostRequest<T>
    {
        /// <summary>
        /// The json serializer
        /// </summary>
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        protected override string ContentType
        {
            get
            {
                return "appication/json";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public PostRequest(string url)
            : base(url)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public PostRequest(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
        /// <summary>
        /// Sets the data to post.
        /// </summary>
        /// <param name="data">The data to post.</param>
        public void SetData(object data)
        {
            base.SetData(jss.Serialize(data));
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
    /// Create a JSON POST request
    /// </summary>
    public class PostRequest : StUtil.Net.JSON.PostRequest<dynamic>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public PostRequest(string url)
            : base(url)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRequest"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public PostRequest(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
    }
}
