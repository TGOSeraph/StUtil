using System;

namespace StUtil.Internal.Native
{
    public static class NativeConsts
    {
        public const byte AC_SRC_ALPHA = 0x01;
        public const byte AC_SRC_OVER = 0x00;
        public const int HC_ACTION = 0;
        public const long HSCROLL = 0x100000;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTTRANSPARENT = -1;
        public const int MAX_CAPTION_LENGTH = 100;
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";
        public const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        public const uint TB_BUTTONCOUNT = 0x0418;
        public const uint TB_GETBUTTON = 0x0417;
        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const uint TOKEN_QUERY = 0x0008;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_OPAQUE = 0x00000004;
        public const byte VK_CAPITAL = 0x14;
        public const byte VK_CONTROL = 0x11;
        public const byte VK_MENU = 0x12;
        public const byte VK_SHIFT = 0x10;
        public const long VSCROLL = 0x200000;
        public const int WH_KEYBOARD = 2;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14;
        public const int WM_ACTIVATEAPP = 0x01C;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 0x208;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_SETREDRAW = 11;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WM_XBUTTONDBLCLK = 0x20D;
        public const int WM_XBUTTONDOWN = 0x20B;
        public const int WM_XBUTTONUP = 0x20C;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_NOACTIVATE = 0x08000000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public static IntPtr HWND_TOPMOST = IntPtr.Zero;
    }
}