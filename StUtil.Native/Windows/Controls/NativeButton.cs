using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Windows.Controls
{
    public class NativeButton: NativeComponent
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
