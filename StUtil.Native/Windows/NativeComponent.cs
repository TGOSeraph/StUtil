using StUtil.Native.Internal;
using StUtil.Native.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Windows
{
    public class NativeComponent
    {
        public IntPtr Handle { get; private set; }

        public string Text
        {
            get
            {
                return NativeUtilities.GetWindowText(Handle);
            }
            set
            {
                NativeMethods.SetWindowText(Handle, value);
            }
        }

        public string ClassName
        {
            get
            {
                return NativeUtilities.GetClassName(Handle);
            }
        }

        public IEnumerable<NativeComponent> RawChildren
        {
            get
            {
                return NativeUtilities.GetChildWindows(this.Handle).Select(w => new NativeComponent(w));
            }
        }


        public IEnumerable<NativeComponent> Children
        {
            get
            {
                return RawChildren.Select(c =>
                {
                    switch (c.ClassName)
                    {
                        case "Button":
                            return new NativeButton(c.Handle);
                        case "Static":
                            return new NativeStatic(c.Handle);
                        default:
                            return c;
                    }
                });
            }
        }

        public NativeComponent(IntPtr handle)
        {
            this.Handle = handle;
        }
    }
}
