using StUtil.Native.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Extensions
{
    public static class NativeComponentExtensions
    {
        public static Bitmap Capture(this NativeComponent component, bool clientArea = false)
        {
            return ScreenCapture.Capture(component.Handle, clientArea);
        }
        public static Bitmap Print(this NativeComponent component, bool clientArea = false)
        {
            return ScreenCapture.Print(component.Handle, clientArea);
        }
    }
}
