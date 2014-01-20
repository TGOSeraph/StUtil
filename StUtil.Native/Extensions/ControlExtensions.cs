using StUtil.Internal.Native;
using StUtil.Native.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace StUtil.Native.Extensions
{
    /// <summary>
    /// Extensions for Controls
    /// </summary>
    /// <remarks>
    /// 2013-06-27  - Initial version
    /// </remarks>
    public static class ControlExtensions
    {
        /// <summary>
        /// Suspend drawing on the control
        /// </summary>
        /// <param name="parent">The control to prevent drawing children on</param>
        public static void SuspendDrawing(this Control parent)
        {
            NativeMethods.SendMessage(parent.Handle, NativeConsts.WM_SETREDRAW, false, 0);
        }

        /// <summary>
        /// Resume drawing on the control
        /// </summary>
        /// <param name="parent">The control to allow drawing children on</param>
        public static void ResumeDrawing(this Control parent)
        {
            NativeMethods.SendMessage(parent.Handle, NativeConsts.WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        /// <summary>
        /// Get the vertical scrollbar info object for a control that does not expose scrollbar info
        /// </summary>
        /// <param name="ctrl">The control to get the scroll info from</param>
        /// <returns>An Scrollbar info object containing scrollbar values</returns>
        public static ScrollbarInfo GetVScrollInfo(this Control ctrl)
        {
            return new ScrollbarInfo(ctrl, ScrollOrientation.VerticalScroll);
        }

        /// <summary>
        /// Get the horizontal scrollbar info object for a control that does not expose scrollbar info
        /// </summary>
        /// <param name="ctrl">The control to get the scroll info from</param>
        /// <returns>An Scrollbar info object containing scrollbar values</returns>
        public static ScrollbarInfo GetHScrollInfo(this Control ctrl)
        {
            return new ScrollbarInfo(ctrl, ScrollOrientation.HorizontalScroll);
        }

        public static bool IsHScrollVisible(this Control ctrl)
        {
            long style = NativeUtils.GetWindowLongPtr(ctrl.Handle, -16).ToInt64();
            return (style & NativeConsts.HSCROLL) != 0;
        }

        public static bool IsVScrollVisible(this Control ctrl)
        {
            long style = NativeUtils.GetWindowLongPtr(ctrl.Handle, -16).ToInt64();
            return (style & NativeConsts.VSCROLL) != 0;
        }
    }
}
