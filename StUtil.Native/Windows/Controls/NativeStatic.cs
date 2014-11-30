using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Windows.Controls
{
    public class NativeStatic : NativeComponent
    {
        public NativeStatic(IntPtr handle)
            : base(handle)
        {
        }

        public void SetIcon(Icon icon)
        {
            NativeMethods.SendMessage(Handle, (uint)NativeEnums.STM.STM_SETIMAGE, (uint)NativeEnums.STMImageTypes.IMAGE_CURSOR, icon.Handle);
        }
    }
}
