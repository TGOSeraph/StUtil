using StUtil.Native.Internal;
using System;

namespace StUtil.Native.Windows.Controls
{
    public class NativeButton : NativeComponent
    {
        public NativeButton(IntPtr handle)
            : base(handle)
        {
        }

        public void Click()
        {
            NativeUtilities.DispatchMessage(Handle, (int)NativeEnums.BM.CLICK, IntPtr.Zero, IntPtr.Zero);
        }
    }
}