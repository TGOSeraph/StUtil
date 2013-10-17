using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StUtil.Native
{
   public static  class Utilities
    {
       public static string GetWindowText(IntPtr hWnd)
       {
           return Internal.Native.NativeUtils.GetWindowText(hWnd);
       }
       public static Process GetProcessFromHWND(IntPtr hWnd)
       {
           uint pid;
           Internal.Native.NativeMethods.GetWindowThreadProcessId(hWnd, out pid);
           return Process.GetProcessById((int)pid);
       }
    }
}
