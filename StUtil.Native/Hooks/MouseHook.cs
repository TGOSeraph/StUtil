using StUtil.Extensions;
using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Hooks
{
    public class MouseHook : WindowsHook
    {
        public event EventHandler<MouseEventArgs> MouseMove;
        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseClick;
        public event EventHandler<MouseEventArgs> MouseWheel;
        public event EventHandler<MouseEventArgs> MouseDoubleClick;

        public enum Wheel_Direction
        {
            WheelUp,
            WheelDown
        }

        public MouseHook(HookMethod hooker)
            : base(hooker, HookType.Mouse)
        {
        }

        protected override bool ProcessEvent(int wParam, IntPtr lParam)
        {
            NativeStructs.MSLLHOOKSTRUCT mouseHookStruct = (NativeStructs.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.MSLLHOOKSTRUCT));

            //detect button clicked
            MouseButtons button = MouseButtons.None;
            short mouseDelta = 0;
            switch (wParam)
            {
                case NativeConsts.WM_LBUTTONDOWN:
                case NativeConsts.WM_LBUTTONUP:
                case NativeConsts.WM_LBUTTONDBLCLK:
                    button = MouseButtons.Left;
                    break;
                case NativeConsts.WM_RBUTTONDOWN:
                case NativeConsts.WM_RBUTTONUP:
                case NativeConsts.WM_RBUTTONDBLCLK:
                    button = MouseButtons.Right;
                    break;
                case NativeConsts.WM_MBUTTONDOWN:
                case NativeConsts.WM_MBUTTONUP:
                case NativeConsts.WM_MBUTTONDBLCLK:
                    button = MouseButtons.Middle;
                    break;
                case NativeConsts.WM_MOUSEWHEEL:
                    mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                    break;
            }

            //double clicks
            int clickCount = 0;
            if (button != MouseButtons.None)
                if (wParam == NativeConsts.WM_LBUTTONDBLCLK || wParam == NativeConsts.WM_RBUTTONDBLCLK) clickCount = 2;
                else clickCount = 1;

            //generate event 
            MouseEventArgs e = new MouseEventArgs(button, clickCount, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseDelta);

            switch (wParam)
            {
                case NativeConsts.WM_LBUTTONDOWN:
                case NativeConsts.WM_RBUTTONDOWN:
                case NativeConsts.WM_MBUTTONDOWN:
                    MouseDown.RaiseEvent(this, e);
                    break;
                case NativeConsts.WM_LBUTTONUP:
                case NativeConsts.WM_RBUTTONUP:
                case NativeConsts.WM_MBUTTONUP:
                    MouseUp.RaiseEvent(this, e);
                    MouseClick.RaiseEvent(this, e);
                    break;
                case NativeConsts.WM_LBUTTONDBLCLK:
                case NativeConsts.WM_RBUTTONDBLCLK:
                case NativeConsts.WM_MBUTTONDBLCLK:
                    MouseDoubleClick.RaiseEvent(this, e);
                    break;
                case NativeConsts.WM_MOUSEWHEEL:
                    MouseWheel.RaiseEvent(this, e);
                    break;
                case NativeConsts.WM_MOUSEMOVE:
                    MouseMove.RaiseEvent(this, e);
                    break;
            }


            return false;
        }
    }
}
