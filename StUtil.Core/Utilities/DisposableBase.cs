using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Misc
{
    public abstract class DisposableBase : IDisposable
    {
        public bool Disposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    CleanUpNativeResources();
                }
                Disposed = true;
            }
            CleanUpManagedResources();
        }

        protected virtual void CleanUpManagedResources()
        {
        }
        protected virtual void CleanUpNativeResources()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
