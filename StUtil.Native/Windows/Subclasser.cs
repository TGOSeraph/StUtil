using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Windows
{
    public class Subclasser
    {
        public delegate bool WndProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_WNDPROC = -4;

        public IntPtr Handle { get; private set; }
        private IntPtr oldWndProc;
        private WndProc proc;

        public Subclasser(IntPtr hWnd, WndProc proc)
        {
            Handle = hWnd;
            this.proc = proc;
        }

        public void Hook()
        {
            oldWndProc = NativeMethods.SetWindowLong(Handle, GWL_WNDPROC, new NativeCallbacks.MessageProc(WndProcHandler));
        }

        public void Restore()
        {
            NativeMethods.SetWindowLong(Handle, GWL_WNDPROC, oldWndProc);
        }

        private IntPtr WndProcHandler(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
        {
            if (proc(hWnd, Msg, wParam, lParam))
            {
                return IntPtr.Zero;
            }
            return NativeMethods.DefWindowProc(Handle, Msg, wParam, lParam);
        }
    }
}
