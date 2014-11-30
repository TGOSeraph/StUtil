using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace StUtil.Net
{
    /// <summary>
    /// The base class for creating web requests
    /// </summary>
    /// <typeparam name="T">The type of object to return</typeparam>
	public abstract class NetRequest<T>
	{
        /// <summary>
        /// Gets or sets the cookies.
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        public CookieContainer Cookies
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public Dictionary<string, string> Headers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to try and ignore errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it should try to ignore error status codes; otherwise, <c>false</c>.
        /// </value>
        public bool TryIgnoreError { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
		public string URL
		{
			get;
			set;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="NetRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
		public NetRequest(string url)
		{
            this.TryIgnoreError = true;
			this.Headers = new Dictionary<string, string>();
			this.URL = url;
		}
        /// <summary>
        /// Initializes a new instance of the <see cref="NetRequest{T}"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="cookies">The cookies.</param>
        public NetRequest(string url, CookieContainer cookies)
            : this(url)
        {
            this.Cookies = cookies;
        }
        /// <summary>
        /// Runs the web request
        /// </summary>
        /// <returns></returns>
        public T Run()
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.URL);
                httpWebRequest.CookieContainer = this.Cookies;
                this.SetHeaders(ref httpWebRequest);
                this.BuildRequest(ref httpWebRequest);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                return HandleResponse(ref httpWebResponse);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)e.Response;
                    return HandleResponse(ref httpWebResponse);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Runs the request asynchronously.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void RunAsync(Action<T> callback)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(this.Request));
            thread.Start(new object[]
			{
				callback
			});
        }

        /// <summary>
        /// Builds the web request.
        /// </summary>
        /// <param name="request">The request.</param>
        protected abstract void BuildRequest(ref HttpWebRequest request);

        /// <summary>
        /// Gets the response string.
        /// </summary>
        /// <param name="httpWebResponse">The HTTP web response.</param>
        /// <returns></returns>
        protected virtual string GetResponseString(ref HttpWebResponse httpWebResponse)
        {
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(responseStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Handles the response from the request.
        /// </summary>
        /// <param name="httpWebResponse">The HTTP web response.</param>
        /// <returns></returns>
        protected abstract T HandleResponse(ref HttpWebResponse httpWebResponse);

        /// <summary>
        /// Sets the headers.
        /// </summary>
        /// <param name="request">The request.</param>
        protected virtual void SetHeaders(ref HttpWebRequest request)
        {
            foreach (KeyValuePair<string, string> current in this.Headers)
            {
                switch (current.Key)
                {
                    case "User-Agent":
                        request.UserAgent = current.Value;
                        break;
                    default:
                        request.Headers.Add(current.Key, current.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Performs the web request.
        /// </summary>
        /// <param name="param">The parameter.</param>
		private void Request(object param)
		{
			T obj = this.Run();
			if (param != null)
			{
				((Action<T>)param)(obj);
			}
		}
	}
}
	