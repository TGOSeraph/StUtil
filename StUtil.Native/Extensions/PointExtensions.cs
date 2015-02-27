using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class PointExtensions
    {
        public static Point ClientToScreen(this Point pt, Control ctrl)
        {
            return ClientToScreen(pt, ctrl.Handle);
        }

        public static Point ClientToScreen(this Point pt, Control ctrl, bool includesFrame)
        {
            return ClientToScreen(pt, ctrl.Handle, includesFrame);
        }

        public static Point ClientToScreen(this Point pt, IntPtr hWnd)
        {
            return ClientToScreen(pt, hWnd, true);
        }

        public static Point ClientToScreen(this Point pt, IntPtr hWnd, bool includesFrame)
        {
            Point p = pt;
            NativeMethods.ClientToScreen(hWnd, ref p);

            if (!includesFrame)
            {
                return p;
            }

            Native.Internal.NativeStructs.RECT r = new Native.Internal.NativeStructs.RECT();
            NativeMethods.GetWindowRect(hWnd, ref r);
            Rectangle rect = r;
            NativeMethods.GetClientRect(hWnd, ref r);
            pt = Point.Empty.ClientToScreen(hWnd, false);
            Rectangle client = new Rectangle(pt, ((Rectangle)r).Size);
            int titleHeight = client.Top - rect.Top;

            return new Point(p.X - (client.Width - rect.Width) / 2, p.Y - titleHeight);
        }
    }
}
