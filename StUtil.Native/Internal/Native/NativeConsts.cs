
using System;
namespace StUtil.Internal.Native
{
    public static class NativeConsts
    {
        public const int MAX_CAPTION_LENGTH = 100;
        public const int WM_SETREDRAW = 11;
        public const int WH_KEYBOARD_LL = 13;
        public static IntPtr HWND_TOPMOST = IntPtr.Zero;
        public const uint TB_GETBUTTON = 0x0417;
        public const uint TB_BUTTONCOUNT = 0x0418;

        public const int WM_NCHITTEST = 0x0084,
                           WM_NCACTIVATE = 0x0086,
                           WS_EX_TRANSPARENT = 0x00000020,
                           WS_EX_TOOLWINDOW = 0x00000080,
                           WS_EX_LAYERED = 0x00080000,
                           WS_EX_NOACTIVATE = 0x08000000,
                           WM_GETMINMAXINFO = 0x0024,
                           HTTRANSPARENT = -1,
                           HTLEFT = 10,
                           HTRIGHT = 11,
                           HTTOP = 12,
                           HTTOPLEFT = 13,
                           HTTOPRIGHT = 14,
                           HTBOTTOM = 15,
                           HTBOTTOMLEFT = 16,
                           HTBOTTOMRIGHT = 17;
        public const int WM_ACTIVATEAPP = 0x01C;

        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const uint TOKEN_QUERY = 0x0008;
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";
        public const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        public const long HSCROLL = 0x100000;
        public const long VSCROLL = 0x200000;

        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
    }
}
