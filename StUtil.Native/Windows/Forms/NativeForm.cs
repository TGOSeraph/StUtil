using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public NativeForm(IntPtr handle) : base(handle)
        {
        }
    }
}
