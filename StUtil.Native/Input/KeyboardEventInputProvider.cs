using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Input
{
    public class KeyboardEventInputProvider : KeyboardInputProvider
    {
        protected override void Down(System.Windows.Forms.Keys key)
        {
            StUtil.Native.Internal.NativeMethods.keybd_event((byte)key, 0, (uint)StUtil.Native.Internal.NativeEnums.KeyEventFlag.KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
        }

        protected override void Up(System.Windows.Forms.Keys key)
        {
            StUtil.Native.Internal.NativeMethods.keybd_event((byte)key, 0, (uint)StUtil.Native.Internal.NativeEnums.KeyEventFlag.KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        protected override bool RequiresHandle
        {
            get { return false; }
        }
    }
}
