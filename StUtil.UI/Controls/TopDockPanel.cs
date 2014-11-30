using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    /// <summary>
    /// A panel that docks controls to the top
    /// </summary>
    public class TopDockPanel : FlickerFree.Panel
    {
        /// <summary>
        /// The cache of controls that have been made top docking
        /// </summary>
        private static HashSet<Control> cache = new HashSet<Control>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TopDockPanel" /> class.
        /// </summary>
        public TopDockPanel()
        {
            this.AutoScroll = true;
            TopDockPanel.SetTopDockingControl(this);
        }

        /// <summary>
        /// Sets the control to top docking.
        /// </summary>
        /// <typeparam name="T">The type of the control.</typeparam>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Control already set as top docking;control</exception>
        public static T SetTopDockingControl<T>(T control) where T : Control
        {
            if (cache.Contains(control))
            {
                throw new ArgumentException("Control already set as top docking", "control");
            }
            control.Disposed += control_Disposed;
            control.ControlAdded += control_ControlAdded;

            cache.Add(control);
            return control;
        }

        /// <summary>
        /// Removes the top docking from the control.
        /// </summary>
        /// <typeparam name="T">The type of control.</typeparam>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Control not set as top docking;control</exception>
        public static T RemoveTopDockingControl<T>(T control) where T : Control
        {
            if (!cache.Contains(control))
            {
                throw new ArgumentException("Control not set as top docking", "control");
            }
            control.Disposed -= control_Disposed;
            control.ControlAdded -= control_ControlAdded;
            cache.Remove(control);
            return control;
        }

        /// <summary>
        /// Handles the ControlAdded event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ControlEventArgs"/> instance containing the event data.</param>
        private static void control_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Dock = DockStyle.Top;
            e.Control.BringToFront();
        }

        /// <summary>
        /// Handles the Disposed event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private static void control_Disposed(object sender, EventArgs e)
        {
            RemoveTopDockingControl((Control)sender);
        }
    }
}