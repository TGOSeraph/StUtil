using System.Net;
using System.Web.Script.Serialization;

namespace StUtil.Net.JSON
{
    public class POST<T> : StUtil.Net.POST<T>
    {
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        public POST(string url)
            : base(url)
        {
        }
        public POST(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
        public void SetData(object data)
        {
            base.SetData(jss.Serialize(data));
        }

        protected override T HandleResponse(ref System.Net.HttpWebResponse httpWebResponse)
        {
            if (typeof(object) == typeof(T))
            {
                return (T)StUtil.Net.Utilities.DynamicJSONConverter.Deserialize(base.GetResponseString(ref httpWebResponse));
            }
            else
            {
                return jss.Deserialize<T>(base.GetResponseString(ref httpWebResponse));
            }
        }

        protected override string ContentType
        {
            get
            {
                return "appication/json";
            }
        }
    }

    public class POST : StUtil.Net.JSON.POST<dynamic>
    {
        public POST(string url)
            : base(url)
        {
        }
        public POST(string url, CookieContainer cookies)
            : base(url, cookies)
        {
        }
    }
}
