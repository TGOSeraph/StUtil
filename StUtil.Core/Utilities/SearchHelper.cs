using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StUtil.Utilities
{
    public class SearchHelper<TInput, TOutput>
    {
        private object SyncLock = new object();
        private int activeThreadId = 0;

        public void Search(TInput input, Func<TInput, TOutput> search, Action<TOutput, Exception> callback)
        {
            lock (SyncLock)
            {
                int id = ++this.activeThreadId;

                Thread t = new Thread(delegate()
                {
                    try
                    {
                        lock (SyncLock)
                        {
                            TOutput results = search(input);
                            if (id == this.activeThreadId)
                            {
                                activeThreadId = 0;
                                callback(results, null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        callback(default(TOutput), ex);
                    }
                });
                t.Start();
            }
        }
    }
}
