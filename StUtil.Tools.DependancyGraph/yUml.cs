using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;

namespace StUtil.Tools.DependancyGraph
{
    public static class yUml
    {
        public static Bitmap GetDiagram(string graph)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData("http://yuml.me/diagram/scruffy/class/" + graph);
            return StUtil.Imaging.Utilities.BitmapFromBytes(data);
        }

        public static Bitmap GetDiagram<T>(IEnumerable<T> items, StUtil.Data.Specialised.DependancyHelper<T> dependancies, Func<T, string> itemToString = null)
        {
            if (itemToString == null)
            {
                itemToString = i => i.ToString();
            }
            List<T> remaining = items.ToList();

            string depends = "";
            foreach (var d in dependancies.Dependancies)
            {
                if (d.Value.DependsOn.Count > 0)
                {
                    foreach (T item in d.Value.DependsOn)
                    {
                        depends += ",[" + itemToString(item) + "]->[" + itemToString(d.Value.Item) + "]";
                    }
                    remaining.Remove(d.Value.Item);
                }
            }
            depends = (string.Join(",", remaining.Select(r => "[" + itemToString(r) + "]")) + depends).Trim(',');
            return GetDiagram(depends);
        }
    }
}