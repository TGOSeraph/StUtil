using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Native
{
    public sealed class WindowFinder
    {
        public IntPtr Handle { get; set; }

        public WindowFinder(IntPtr handle)
        {
            this.Handle = handle;
        }

        public WindowFinder(string caption, string className)
        {
            this.Handle = NativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, className, caption);
        }

        public IEnumerable<WindowFinder> Children
        {
            get
            {
                return Internal.Native.NativeUtils.GetChildren(this.Handle).Select(h => new WindowFinder(h));
            }
        }

        public WindowFinder Parent
        {
            get
            {
                return new WindowFinder(NativeMethods.GetParent(this.Handle));
            }
        }

        public IEnumerable<WindowFinder> Siblings
        {
            get
            {
                return Parent.Children.Where(w => w.Handle != Handle);
            }
        }

        public WindowFinder After(string caption, string className)
        {
            return new WindowFinder(NativeMethods.FindWindowEx(IntPtr.Zero, Handle, className, caption));
        }

        public WindowFinder AfterByCaption(string caption)
        {
            return After(caption, null);
        }

        public WindowFinder AfterByClass(string className)
        {
            return After(null, className);
        }

        public WindowFinder Sibling(string caption, string className)
        {
            return Siblings.FirstOrDefault(w => className.Equals(NativeUtils.GetClassName(w.Handle), StringComparison.InvariantCultureIgnoreCase));
        }

        public WindowFinder SiblingByCaption(string caption)
        {
            return Sibling(caption, null);
        }

        public WindowFinder SiblingByClass(string className)
        {
            return Sibling(null, className);
        }

        public WindowFinder Child(string caption, string className)
        {
            return new WindowFinder(NativeMethods.FindWindowEx(Handle, IntPtr.Zero, className, caption));
        }

        public WindowFinder ChildByCaption(string caption)
        {
            return Child(caption, null);
        }

        public WindowFinder ChildByClass(string className)
        {
            return Child(null, className);
        }

        public override string ToString()
        {
            return Handle.ToString();
        }

        public static WindowFinder Find(string caption, string className)
        {
            return new WindowFinder(caption, className);
        }
    }
}
