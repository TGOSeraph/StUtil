using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Input
{
    public class MouseMessageInputProvider : MouseInputProvider
    {
        public MessageDispatchMethod DispatchMethod
        {
            get;
            set;
        }

        protected override bool RequiresHandle
        {
            get { return true; }
        }

        private enum InputMessage
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208
        }

        private void DispatchMessage(InputMessage message, IntPtr wParam, IntPtr lParam)
        {
            if (DispatchMethod == MessageDispatchMethod.Post)
            {
                StUtil.Native.Internal.NativeMethods.PostMessage(Handle, (int)message, wParam, lParam);
            }
            else
            {
                StUtil.Native.Internal.NativeMethods.SendMessage(Handle, (int)message, wParam, lParam);
            }
        }

        public override void MoveTo(int x, int y)
        {
            DispatchMessage(InputMessage.WM_MOUSEMOVE, IntPtr.Zero, new IntPtr(StUtil.Native.Internal.NativeUtilities.MakeLParam(x, y)));
        }

        protected override void ButtonDown(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            InputMessage message;
            switch (button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    message = InputMessage.WM_LBUTTONDOWN;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    message = InputMessage.WM_RBUTTONDOWN;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    message = InputMessage.WM_MBUTTONDOWN;
                    break;
                default:
                    throw new NotImplementedException(button.ToString());
            }

            DispatchMessage(message, IntPtr.Zero, new IntPtr(StUtil.Native.Internal.NativeUtilities.MakeLParam(x, y)));
        }

        protected override void ButtonUp(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            InputMessage message;
            switch (button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    message = InputMessage.WM_LBUTTONUP;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    message = InputMessage.WM_RBUTTONUP;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    message = InputMessage.WM_MBUTTONUP;
                    break;
                default:
                    throw new NotImplementedException(button.ToString());
            }

            DispatchMessage(message, IntPtr.Zero, new IntPtr(StUtil.Native.Internal.NativeUtilities.MakeLParam(x, y)));
        }
    }
}
