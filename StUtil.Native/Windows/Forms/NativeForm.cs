using System;
using System.Collections.Generic;
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

        public static IEnumerable<NativeForm> GetTopLevelWindows()
        {
            List<NativeForm> f = new List<NativeForm>();

            StUtil.Native.Internal.NativeMethods.EnumWindows((hWnd, lParam) =>
            {
                f.Add(new NativeForm(hWnd));
                return true;
            }, IntPtr.Zero);

            return f;
        }
    }
}