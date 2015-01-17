using StUtil.Native;
using StUtil.Native.Internal;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Controls
    /// </summary>
    public static class ControlExtensions
    {
        private static Dictionary<Control, WndProcOverride> clickThroughs = new Dictionary<Control, WndProcOverride>();

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

        /// <summary>
        /// Prevents the control from intercepting mouse clicks
        /// </summary>
        /// <param name="control"></param>
        public static void DisableMouseEvents(this Control control)
        {
            if (clickThroughs.ContainsKey(control))
            {
                control.Disposed += control_Disposed;
            }
            StUtil.Native.WndProcOverride o = new Native.WndProcOverride(control, Native.WndProcOverride.CreateClickThroughHandler());
            clickThroughs.Add(control, o);
        }

        /// <summary>
        /// Allows the control to handle mouse clicks
        /// </summary>
        /// <param name="control"></param>
        public static void EnableMouseEvents(this Control control)
        {
            if (clickThroughs.ContainsKey(control))
            {
                control.Disposed -= control_Disposed;
                clickThroughs[control].Dispose();
                clickThroughs.Remove(control);
            }
        }

        private static void control_Disposed(object sender, System.EventArgs e)
        {
            EnableMouseEvents((Control)sender);
        }
    }
}