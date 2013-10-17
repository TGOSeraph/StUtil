using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Component's
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Check for if the current component is in design mode
        /// </summary>
        /// <param name="component">The component to check if it is in design mode</param>
        /// <returns>If the component is being designed</returns>
        public static bool InDesignMode(this Component component)
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                (bool)component.GetType().GetProperty("DesignMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .GetValue(component, null);
        }
    }
}
