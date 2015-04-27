using StUtil.Native.Internal;
using StUtil.Native.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StUtil.Native.Windows
{
    public class NativeComponent : IEnumerable<NativeComponent>
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
                return RawChildren.Select(c => c.ToKnownComponent());
            }
        }


        public Rectangle Bounds
        {
            get
            {
                NativeStructs.RECT r = new NativeStructs.RECT();
                NativeMethods.GetWindowRect(Handle, ref r);
                return r;
            }
            set
            {
                NativeMethods.SetWindowPos(Handle, IntPtr.Zero, value.X, value.Y, value.Width, value.Height, NativeEnums.SWP.DRAWFRAME | NativeEnums.SWP.FRAMECHANGED | NativeEnums.SWP.NOOWNERZORDER);
            }
        }


        public NativeComponent(IntPtr handle)
        {
            this.Handle = handle;
        }

        public override string ToString()
        {
            return "{hWnd: " + Handle.ToString() + ", Class: " + ClassName + ", Text: " + Text + "}";
        }

        public NativeComponent ToKnownComponent()
        {
            switch (ClassName)
            {
                case "Button":
                    return new NativeButton(Handle);
                case "Static":
                    return new NativeStatic(Handle);
                default:
                    return this;
            }
        }

        public IEnumerator<NativeComponent> GetEnumerator()
        {
            return RawChildren.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return RawChildren.GetEnumerator();
        }

        public static NativeComponent Desktop()
        {
            return new NativeComponent(Internal.NativeMethods.GetDesktopWindow());
        }
    }
}