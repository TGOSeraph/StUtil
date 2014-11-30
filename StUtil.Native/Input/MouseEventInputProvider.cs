using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Input
{
    public class MouseEventInputProvider : MouseInputProvider
    {
        protected override bool RequiresHandle
        {
            get { return false; }
        }

        public override void Move(int x, int y)
        {
            NativeMethods.mouse_event((uint)(NativeEnums.MouseEventFlags.MOVE | NativeEnums.MouseEventFlags.ABSOLUTE), (uint)NativeUtilities.CalculateAbsoluteCoordinateX(x), (uint)NativeUtilities.CalculateAbsoluteCoordinateY(y), 0, IntPtr.Zero);
        }

        protected override void ButtonDown(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            NativeEnums.MouseEventFlags flag;
            switch (button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    flag = NativeEnums.MouseEventFlags.LEFTDOWN;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    flag = NativeEnums.MouseEventFlags.RIGHTDOWN;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    flag = NativeEnums.MouseEventFlags.MIDDLEDOWN;
                    break;
                default:
                    throw new NotImplementedException(button.ToString());
            }
            NativeMethods.mouse_event((uint)(flag | NativeEnums.MouseEventFlags.ABSOLUTE), (uint)NativeUtilities.CalculateAbsoluteCoordinateX(x), (uint)NativeUtilities.CalculateAbsoluteCoordinateY(y), 0, IntPtr.Zero);
        }

        protected override void ButtonUp(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            NativeEnums.MouseEventFlags flag;
            switch (button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    flag = NativeEnums.MouseEventFlags.LEFTUP;
                    break;
                case System.Windows.Forms.MouseButtons.Right:
                    flag = NativeEnums.MouseEventFlags.RIGHTUP;
                    break;
                case System.Windows.Forms.MouseButtons.Middle:
                    flag = NativeEnums.MouseEventFlags.MIDDLEUP;
                    break;
                default:
                    throw new NotImplementedException(button.ToString());
            }
            NativeMethods.mouse_event((uint)(flag | NativeEnums.MouseEventFlags.ABSOLUTE), (uint)NativeUtilities.CalculateAbsoluteCoordinateX(x), (uint)NativeUtilities.CalculateAbsoluteCoordinateY(y), 0, IntPtr.Zero);
        }
    }
}
