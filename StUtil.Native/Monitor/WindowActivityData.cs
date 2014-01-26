using System;
using System.Diagnostics;
using StUtil.Internal.Native;

namespace StUtil.Native.Monitor
{
    public class WindowActivityData
    {
       public static bool CacheData { get; set; }

        public IntPtr Handle { get; set; }

        private string caption;
        public string Caption
        {
            get 
            {
                if (!CacheData ||caption == null)
                {
                    caption = NativeUtils.GetWindowText(Handle);
                }
                return caption;
            }
        }

        private string className;
        public string ClassName
        {
            get
            {
                if (!CacheData || className == null)
                {
                    className = NativeUtils.GetClassName(Handle);
                }
                return className;
            }
        }
        
        static WindowActivityData()
        {
            CacheData = true;
        }

        public WindowActivityData(IntPtr hWnd)
        {
            this.Handle = hWnd;
        }

        public Process GetProcess()
        {
            uint pid;
            NativeMethods.GetWindowThreadProcessId(this.Handle, out pid);
            return Process.GetProcessById((int)pid);
        }
    }
}
