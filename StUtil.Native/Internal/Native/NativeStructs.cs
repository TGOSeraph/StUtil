using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace StUtil.Internal.Native
{
    public static class NativeStructs
    {
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
        public struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
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
    }
}
