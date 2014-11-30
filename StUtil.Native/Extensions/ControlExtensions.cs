using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Controls
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Suspend drawing on the control
        /// </summary>
        /// <param name="parent">The control to prevent drawing children on</param>
        public static void SuspendDrawing(this Control parent)
        {
            NativeMethods.SendMessage(parent.Handle, NativeEnums.WM.SETREDRAW, false, 0);
        }

        /// <summary>
        /// Resume drawing on the control
        /// </summary>
        /// <param name="parent">The control to allow drawing children on</param>
        public static void ResumeDrawing(this Control parent)
        {
            NativeMethods.SendMessage(parent.Handle, NativeEnums.WM.SETREDRAW, true, 0);
            parent.Refresh();
        }
    }
}
