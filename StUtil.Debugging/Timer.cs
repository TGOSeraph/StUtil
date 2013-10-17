using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StUtil.Extensions;
namespace StUtil.Debugging
{
    public static class Timer
    {
        private static int id = 0;
        private static Dictionary<int, KeyValuePair<string, Stopwatch>> store = new Dictionary<int, KeyValuePair<string, Stopwatch>>();

        public static int Start(string tag = null)
        {
            lock (store)
            {
                Stopwatch sw = new Stopwatch();
                store.Add(++id, new KeyValuePair<string, Stopwatch>(tag, sw));
                sw.Start();
                return id;
            }
        }

        public static TimeSpan Stop(int id, bool print = true)
        {
            store[id].Value.Stop();
            TimeSpan ts = store[id].Value.Elapsed;
            if (print)
            {
                Debug.WriteLine("Timer(" + (store[id].Key ?? "N/A") + ")[" + id + "]: " + ts.ToReadableString());
            }
            store.Remove(id);
            return ts;
        }
    }
}
