using System;

namespace StUtil.Native.Internal
{
    public static partial class NativeCallbacks
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate IntPtr MessageProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    }
}