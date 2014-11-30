using System.IO;
using System.Net;
using System.Text;

namespace StUtil.Net
{
    /// <summary>
    /// Description of POST.
    /// </summary>
    public abstract class PostRequest<T> : NetRequest<T>
    {
        /// <summary>
        /// Gets the data to post.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        protected abstract string ContentType
        {
            get;
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
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(byte[] data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="encoding">The encoding.</param>
        public void SetData(string data, Encoding encoding)
        {
            this.Data = encoding.GetBytes(data);
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetData(string data)
        {
            this.Data = Encoding.Default.GetBytes(data);
        }
        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void BuildRequest(ref HttpWebRequest request)
        {
            request.Method = "POST";
            request.ContentLength = (long)this.Data.Length;
            request.ContentType = this.ContentType;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(this.Data, 0, this.Data.Length);
            requestStream.Close();
        }
    }
}
