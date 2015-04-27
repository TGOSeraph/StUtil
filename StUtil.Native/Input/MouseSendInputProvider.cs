using StUtil.Native.Internal;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public class MouseSendInputProvider : MouseInputProvider
    {
        protected override bool RequiresHandle
        {
            get { return false; }
        }

        public override void MoveTo(int x, int y)
        {
            SendMouseInput(x, y, NativeEnums.MouseEventFlags.Move);
        }

        protected override void ButtonDown(MouseButtons button, int x, int y)
        {
            NativeEnums.MouseEventFlags flag;
            switch (button)
            {
                case MouseButtons.Left:
                    flag = NativeEnums.MouseEventFlags.LeftDown;
                    break;

                case MouseButtons.Right:
                    flag = NativeEnums.MouseEventFlags.RightDown;
                    break;

                case MouseButtons.Middle:
                    flag = NativeEnums.MouseEventFlags.MiddleDown;
                    break;

                default:
                    throw new NotImplementedException(button.ToString());
            }
            SendMouseInput(x, y, flag);
        }

        protected override void ButtonUp(MouseButtons button, int x, int y)
        {
            NativeEnums.MouseEventFlags flag;
            switch (button)
            {
                case MouseButtons.Left:
                    flag = NativeEnums.MouseEventFlags.LeftUp;
                    break;

                case MouseButtons.Right:
                    flag = NativeEnums.MouseEventFlags.RightUp;
                    break;

                case MouseButtons.Middle:
                    flag = NativeEnums.MouseEventFlags.MiddleUp;
                    break;

                default:
                    throw new NotImplementedException(button.ToString());
            }
            SendMouseInput(x, y, flag);
        }




        private static void SendMouseInput(int x, int y, NativeEnums.MouseEventFlags flag)
        {
            NativeStructs.INPUT mouseInput = new NativeStructs.INPUT();
            mouseInput.type = NativeEnums.SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = NativeUtilities.CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = NativeUtilities.CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            mouseInput.mkhi.mi.dwFlags = flag | NativeEnums.MouseEventFlags.Absolute;
            NativeMethods.SendInput(1, ref mouseInput, Marshal.SizeOf(new NativeStructs.INPUT()));
        }


    }
}