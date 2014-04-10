using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Controls
{
    public partial class FocusChildForm : Form
    {
        internal bool ForceActiveBar { get; set; }

        public FocusChildForm()
        {
            InitializeComponent();
            this.ForceActiveBar = true;

            WndProcHandler handler = new WndProcHandler(this,
              new WndProcHandler.MessageHandler(NativeConsts.WM_NCACTIVATE, HandleNcActivate));
        }

        private bool HandleNcActivate(WndProcHandler handler, ref Message m)
        {
            if (m.Msg == NativeConsts.WM_NCACTIVATE)
            {
                if (this.ForceActiveBar && m.WParam == IntPtr.Zero)
                {
                    NativeMethods.SendMessage(this.Handle, NativeConsts.WM_NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                }
            }
            return true;
        }
    }
}
