using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RectangleExtensions
    {

        public static Rectangle ClientToScreen(this Rectangle rect, Control ctrl)
        {
            return ClientToScreen(rect, ctrl.Handle);
        }

        public static Rectangle ClientToScreen(this Rectangle rect, Control ctrl, bool includesFrame)
        {
            return ClientToScreen(rect, ctrl.Handle, includesFrame);
        }

        public static Rectangle ClientToScreen(this Rectangle rect, IntPtr hWnd)
        {
            return ClientToScreen(rect, hWnd, true);
        }

        public static Rectangle ClientToScreen(this Rectangle rect, IntPtr hWnd, bool includesFrame)
        {
            Point pt = rect.Location.ClientToScreen(hWnd, includesFrame);
            return new Rectangle(pt.X, pt.Y, rect.Width, rect.Height);
        }
    }
}
