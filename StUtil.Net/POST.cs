using System.IO;
using System.Net;
using System.Text;

namespace StUtil.Net
{
	/// <summary>
	/// Description of POST.
	/// </summary>
	public abstract class POST<T> : Base<T>
	{
		public byte[] Data
		{
			get;
			private set;
		}
		protected abstract string ContentType
		{
			get;
		}
		public POST(string url) : base(url)
		{
		}
        public POST(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
		public void SetData(byte[] data)
		{
			this.Data = data;
		}
		public void SetData(string data, Encoding encoding)
		{
			this.Data = encoding.GetBytes(data);
		}
		public void SetData(string data)
		{
			this.Data = Encoding.Default.GetBytes(data);
		}
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
