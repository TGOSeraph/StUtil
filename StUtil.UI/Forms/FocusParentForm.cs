using StUtil.Native;
using StUtil.Native.Internal;
using System;
using System.Linq;
using System.Windows.Forms;

namespace StUtil.UI.Forms
{
    public partial class FocusParentForm : Form
    {
        /// <summary>
        /// The WND proc overrider
        /// </summary>
        private WndProcOverride wndProc;

        /// <summary>
        /// If the nc activate message should be ignored
        /// </summary>
        private bool ignoreNcActivate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusParentForm"/> class.
        /// </summary>
        public FocusParentForm()
        {
            InitializeComponent();
            wndProc = new WndProcOverride(this,
                    new WndProcHandler(HandleNcActivate) { MessageId = (int)NativeEnums.WM.NCACTIVATE },
                    new WndProcHandler(HandleAppActivate) { MessageId = (int)NativeEnums.WM.ACTIVATEAPP });
        }

        /// <summary>
        /// Handles the application activate message.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns></returns>
        private void HandleAppActivate(ref Message m, out bool stopPropagation)
        {
            if (m.WParam == IntPtr.Zero)
            {
                NativeMethods.PostMessage(this.Handle, NativeEnums.WM.NCACTIVATE, IntPtr.Zero, IntPtr.Zero);
                foreach (FocusChildForm f in this.OwnedForms.OfType<FocusChildForm>())
                {
                    f.ForceActiveBar = false;
                    NativeMethods.PostMessage(f.Handle, NativeEnums.WM.NCACTIVATE, IntPtr.Zero, IntPtr.Zero);
                }
                ignoreNcActivate = true;
            }
            else if (m.WParam == new IntPtr(1))
            {
                NativeMethods.SendMessage(this.Handle, NativeEnums.WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                foreach (FocusChildForm f in this.OwnedForms.OfType<FocusChildForm>())
                {
                    f.ForceActiveBar = true;
                    NativeMethods.SendMessage(f.Handle, NativeEnums.WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                }
            }
            stopPropagation = true;
        }

        /// <summary>
        /// Handles the nc activate messae.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns></returns>
        private void HandleNcActivate(ref Message m, out bool stopPropagation)
        {
            if (m.WParam == IntPtr.Zero)
            {
                if (ignoreNcActivate)
                {
                    ignoreNcActivate = false;
                }
                else
                {
                    NativeMethods.SendMessage(this.Handle, NativeEnums.WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                }
            }
            stopPropagation = true;
        }
    }
}