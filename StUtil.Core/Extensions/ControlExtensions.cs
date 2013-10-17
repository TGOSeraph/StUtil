using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Reflection;
using StUtil.Misc;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Controls
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// 2013-06-27  - Moved Suspend/Resume drawing and GetV/HScroll info methods to StUtil.Native.Extensions
    /// </remarks>
    public static class ControlExtensions
    {
        /// <summary>
        /// The mode to center on, be it X, Y or both.
        /// </summary>
        public enum CenterMode
        {
            Verical,
            Horizontal,
            Both
        }

        /// <summary>
        /// Center a control on its parent
        /// </summary>
        /// <param name="control">The control to center</param>
        /// <param name="mode">The mode to center, vertically, horizontally or both</param>
        public static void Center(this Control control, CenterMode mode = CenterMode.Both)
        {
            int x;
            int y;

            switch (mode)
            {
                case CenterMode.Horizontal:
                    y = control.Location.Y;
                    x = (control.Parent.Width - control.Width) / 2;
                    break;
                case CenterMode.Verical:
                    x = control.Location.X;
                    y = (control.Parent.Height - control.Height) / 2;
                    break;
                default:
                    x = (control.Parent.Width - control.Width) / 2;
                    y = (control.Parent.Height - control.Height) / 2;
                    break;
            }

            control.Location = new System.Drawing.Point(x, y);
        }

        /// <summary>
        /// Uses reflection to enable the setting of a style, e.g. double buffering, on a control from outside its class
        /// </summary>
        /// <param name="ctrl">The control to set the stype on</param>
        /// <param name="style">The styles to set</param>
        /// <param name="enable">Wether to enable or disable the specified styles</param>
        public static void SetStyleEx(this Control ctrl, ControlStyles style, bool enable = true)
        {
            Type type = ctrl.GetType();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo method = type.GetMethod("SetStyle", flags);

            if (method != null)
            {
                object[] param = { style, enable };
                method.Invoke(ctrl, param);
            }
        }


    }
}
