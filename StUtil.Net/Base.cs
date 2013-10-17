using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace StUtil.Net
{
	public abstract class Base<T>
	{
        public bool TryIgnoreError { get; set; }

		public string URL
		{
			get;
			set;
		}
		public CookieContainer Cookies
		{
			get;
			set;
		}
		public Dictionary<string, string> Headers
		{
			get;
			set;
		}
		public Base(string url)
		{
            this.TryIgnoreError = true;
			this.Headers = new Dictionary<string, string>();
			this.URL = url;
		}
        public Base(string url, CookieContainer cookies) : this(url)
        {
            this.Cookies = cookies;
        }
		private void Request(object param)
		{
			T obj = this.Run();
			if (param != null)
			{
				((Action<T>)param)(obj);
			}
		}
		public void RunAsync(Action<T> callback)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.Request));
			thread.Start(new object[]
			{
				callback
			});
		}
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
                //if (e.Status == WebExceptionStatus.ProtocolError)
                //{
                //    HttpWebResponse httpWebResponse = (HttpWebResponse)e.Response;
                //    if ((int)httpWebResponse.StatusCode == 500)
                //    {
                //        using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                //        {
                //            var result = sr.ReadToEnd();
                //        }
                //    }
                //}
            }
		}
		protected virtual void SetHeaders(ref HttpWebRequest request)
		{
			foreach (KeyValuePair<string, string> current in this.Headers)
			{
				switch (current.Key) {
					case "User-Agent":
						request.UserAgent = current.Value;
						break;
					default:
						request.Headers.Add(current.Key, current.Value);
						break;
				}
			}
		}
		protected abstract void BuildRequest(ref HttpWebRequest request);
		protected abstract T HandleResponse(ref HttpWebResponse httpWebResponse);
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
	}
}
	