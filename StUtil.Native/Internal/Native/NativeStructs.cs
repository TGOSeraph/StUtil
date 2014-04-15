using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace StUtil.Internal.Native
{
    public static class NativeStructs
    {
        /// <summary>
        /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public int vkCode;

            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// </summary>
            public int scanCode;

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int flags;

            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public int dwExtraInfo;
        }

        /// <summary>
        /// The MOUSEHOOKSTRUCT structure contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public class MOUSEHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;

            /// <summary>
            /// Handle to the window that will receive the mouse message corresponding to the mouse event.
            /// </summary>
            public int hwnd;

            /// <summary>
            /// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message.
            /// </summary>
            public int wHitTestCode;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class MSLLHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;

            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta.
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward,
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
            /// One wheel click is defined as WHEEL_DELTA, which is 120.
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used.
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public int mouseData;

            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose
            ///LLMHF_INJECTED Test the event-injected flag.
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
            /// </summary>
            public int flags;

            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGEINFO
        {
            public IntPtr hbmImage;
            public IntPtr hbmMask;
            public int Unused1;
            public int Unused2;
            public StUtil.Internal.Native.NativeStructs.RECT rcImage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGELISTDRAWPARAMS
        {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap;        // x offest from the upperleft of bitmap
            public int yBitmap;        // y offset from the upperleft of bitmap
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEHOOKSTRUCTEX
        {
            public MOUSEHOOKSTRUCT mouseHookStruct;
            public int MouseData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public POINT(System.Drawing.Point pt)
                : this(pt.X, pt.Y)
            {
            }

            public static implicit operator System.Drawing.Point(POINT p)
            {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public int cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeConsts.MAX_PATH)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public Int32 cx;
            public Int32 cy;

            public SIZE(Int32 cx, Int32 cy)
            {
                this.cx = cx; this.cy = cy;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TBBUTTON
        {
            public int iBitmap;
            public int idCommand;

            [StructLayout(LayoutKind.Explicit)]
            private struct TBBUTTON_U
            {
                [FieldOffset(0)]
                public byte fsState;

                [FieldOffset(1)]
                public byte fsStyle;

                [FieldOffset(0)]
                private IntPtr bReserved;
            }

            private TBBUTTON_U union;
            public byte fsState { get { return union.fsState; } set { union.fsState = value; } }
            public byte fsStyle { get { return union.fsStyle; } set { union.fsStyle = value; } }
            public UIntPtr dwData;
            public IntPtr iString;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TV_ITEM
        {
            public int mask;
            public int hItem;
            public int state;
            public int stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public int lParam;
            public int iIntegral;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct TVITEMEX
        {
            public uint mask;
            public IntPtr hItem;
            public uint state;
            public uint stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public int lParam;
            public int iIntegral;
        }

        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int code;
        }

        public struct NMCUSTOMDRAW
        {
            public NMHDR hdr;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public IntPtr dwItemSpec;
            public int uItemState;
            public IntPtr lItemlParam;
        }

        public struct NMTVCUSTOMDRAW
        {
            public NMCUSTOMDRAW nmcd;
            public int clrText;
            public int clrTextBk;
            public int iLevel;
        }
    }
}