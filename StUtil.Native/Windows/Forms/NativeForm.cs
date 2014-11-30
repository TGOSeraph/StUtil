using System;
using System.Drawing;

namespace StUtil.Native.Windows.Forms
{
    public class NativeForm : NativeComponent
    {
        public Bitmap Icon
        {
            set
            {
                Native.Internal.NativeMethods.SendMessage(Handle, Native.Internal.NativeEnums.WM.SETICON, new IntPtr(1), value.GetHicon());
            }
        }

        public NativeForm(IntPtr handle)
            : base(handle)
        {
        }
    }
}