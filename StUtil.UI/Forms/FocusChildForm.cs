using StUtil.Native.Internal;
using System;
using System.Windows.Forms;

namespace StUtil.Native.Controls
{
    public partial class FocusChildForm : Form
    {
        /// <summary>
        /// The WND proc overrider
        /// </summary>
        private WndProcOverride wndProc;

        /// <summary>
        /// Gets or sets a value indicating whether to force the titlebar active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the title bar should be forced to be active; otherwise, <c>false</c>.
        /// </value>
        internal bool ForceActiveBar { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusChildForm"/> class.
        /// </summary>
        public FocusChildForm()
        {
            InitializeComponent();
            this.ForceActiveBar = true;

            wndProc = new WndProcOverride(this, new WndProcHandler(HandleNcActivate) { MessageId = (int)NativeEnums.WM.NCACTIVATE });
        }

        /// <summary>
        /// Handles the nc activate message.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns></returns>
        private void HandleNcActivate(ref Message msg, out bool stopPropagation)
        {
            if (this.ForceActiveBar && msg.WParam == IntPtr.Zero)
            {
                NativeMethods.SendMessage(this.Handle, NativeEnums.WM.NCACTIVATE, new IntPtr(1), IntPtr.Zero);
            }
            stopPropagation = true;
        }
    }
}