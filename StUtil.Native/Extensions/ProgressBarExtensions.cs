using System;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extension methods for ProgressBars
    /// </summary>
    public static class ProgressBarExtensions
    {
        /// <summary>
        /// Sets the state color of the progress bar.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="state">The state.</param>
        public static void SetState(this ProgressBar control, StUtil.Native.Internal.NativeEnums.ProgressBarState state)
        {
            StUtil.Native.Internal.NativeMethods.SendMessage(control.Handle, (int)StUtil.Native.Internal.NativeEnums.PBM.PBM_SETSTATE, new IntPtr((int)state), IntPtr.Zero);
        }
    }
}