using StUtil.Extensions;
using StUtil.Native.Hook;
using StUtil.Native.Internal;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Input.Hook
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

        protected override bool ProcessEvent(IntPtr wParam, IntPtr lParam)
        {
            NativeStructs.MSLLHOOKSTRUCT mouseHookStruct = (NativeStructs.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.MSLLHOOKSTRUCT));

            MouseButtons button = MouseButtons.None;
            short mouseDelta = 0;
            NativeEnums.WM message = (NativeEnums.WM)wParam;

            switch (message)
            {
                case NativeEnums.WM.LBUTTONDOWN:
                case NativeEnums.WM.LBUTTONUP:
                case NativeEnums.WM.LBUTTONDBLCLK:
                    button = MouseButtons.Left;
                    break;

                case NativeEnums.WM.RBUTTONDOWN:
                case NativeEnums.WM.RBUTTONUP:
                case NativeEnums.WM.RBUTTONDBLCLK:
                    button = MouseButtons.Right;
                    break;

                case NativeEnums.WM.MBUTTONDOWN:
                case NativeEnums.WM.MBUTTONUP:
                case NativeEnums.WM.MBUTTONDBLCLK:
                    button = MouseButtons.Middle;
                    break;

                case NativeEnums.WM.MOUSEWHEEL:
                    mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);
                    break;
                default:
                    break;
            }

            //double clicks
            int clickCount = 0;
            if (button != MouseButtons.None)
                if (message == NativeEnums.WM.LBUTTONDBLCLK || message == NativeEnums.WM.RBUTTONDBLCLK) clickCount = 2;
                else clickCount = 1;

            //generate event
            MouseEventArgs e = new MouseEventArgs(button, clickCount, mouseHookStruct.pt.X, mouseHookStruct.pt.Y, mouseDelta);

            switch (message)
            {
                case NativeEnums.WM.LBUTTONDOWN:
                case NativeEnums.WM.RBUTTONDOWN:
                case NativeEnums.WM.MBUTTONDOWN:
                    MouseDown.RaiseEvent(this, e);
                    break;

                case NativeEnums.WM.LBUTTONUP:
                case NativeEnums.WM.RBUTTONUP:
                case NativeEnums.WM.MBUTTONUP:
                    MouseUp.RaiseEvent(this, e);
                    MouseClick.RaiseEvent(this, e);
                    break;

                case NativeEnums.WM.LBUTTONDBLCLK:
                case NativeEnums.WM.RBUTTONDBLCLK:
                case NativeEnums.WM.MBUTTONDBLCLK:
                    MouseDoubleClick.RaiseEvent(this, e);
                    break;

                case NativeEnums.WM.MOUSEWHEEL:
                    MouseWheel.RaiseEvent(this, e);
                    break;

                case NativeEnums.WM.MOUSEMOVE:
                    MouseMove.RaiseEvent(this, e);
                    break;
            }

            return false;
        }
    }
}