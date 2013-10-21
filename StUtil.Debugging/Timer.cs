using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StUtil.Extensions;
namespace StUtil.Debugging
{
    /// <summary>
    /// Class to make the timing of events easier
    /// </summary>
    public static class Timer
    {
        /// <summary>
        /// The static ID to be given to the next started timer
        /// </summary>
        private static int id = 0;
        /// <summary>
        /// Store of IDs and their stopwatches and tagged strings
        /// </summary>
        private static Dictionary<int, KeyValuePair<string, Stopwatch>> store = new Dictionary<int, KeyValuePair<string, Stopwatch>>();

        /// <summary>
        /// Start a new timer
        /// </summary>
        /// <param name="tag">The tag or description to give the timer</param>
        /// <returns>The ID of the timer used to stop it</returns>
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
