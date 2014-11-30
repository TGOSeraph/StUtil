using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Input
{
    public class KeyboardMessageInputProvider : KeyboardInputProvider
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


        private void DispatchMessage(StUtil.Native.Internal.NativeEnums.WM message, IntPtr wParam, IntPtr lParam)
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

        protected override void Down(System.Windows.Forms.Keys key)
        {
            IntPtr lParam = new IntPtr(0x00000001 | (StUtil.Native.Internal.NativeMethods.MapVirtualKey((uint)key, 0) << 16));
            DispatchMessage(StUtil.Native.Internal.NativeEnums.WM.KEYDOWN, new IntPtr((int)key), lParam);
        }

        protected override void Up(System.Windows.Forms.Keys key)
        {
            IntPtr lParam = new IntPtr(0x00000001 | (StUtil.Native.Internal.NativeMethods.MapVirtualKey((uint)key, 0) << 16));
            DispatchMessage(StUtil.Native.Internal.NativeEnums.WM.KEYUP, new IntPtr((int)key), lParam);
        }

    }
}
