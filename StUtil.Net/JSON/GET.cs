using System.Net;
using System.Web.Script.Serialization;

namespace StUtil.Net.JSON
{
    public class GET<T> : StUtil.Net.GET<T>
    {
        private static JavaScriptSerializer jss = new JavaScriptSerializer();

        public GET(string url)
            : base(url)
        {
        }
        public GET(string url, CookieContainer cookies)
            : base(url, cookies)
        {
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
    }

    public class GET : StUtil.Net.JSON.GET<object>
    {
        public GET(string url)
            : base(url)
        {
        }
    }


}
