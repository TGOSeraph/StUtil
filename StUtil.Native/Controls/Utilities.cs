using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Native.Controls
{
    public static class Utilities
    {
        public static void BringToFront(IntPtr hWnd)
        {
            Internal.Native.NativeMethods.SetForegroundWindow(hWnd);
        }
    }
}
