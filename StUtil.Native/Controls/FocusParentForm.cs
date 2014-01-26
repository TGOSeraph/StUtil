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
    public partial class FocusParentForm : Form
    {
        private bool ignoreNcActivate = false;

        public FocusParentForm()
        {
            InitializeComponent();
            StUtil.Native.WndProcHandler handler = new WndProcHandler(this,
                    new WndProcHandler.MessageHandler(NativeConsts.WM_NCACTIVATE, HandleNcActivate),
                    new WndProcHandler.MessageHandler(NativeConsts.WM_ACTIVATEAPP, HandleAppActivate));
        }

        private bool HandleAppActivate(WndProcHandler handler, ref Message m)
        {
            if (m.WParam == IntPtr.Zero)
            {
                NativeMethods.PostMessage(this.Handle, NativeConsts.WM_NCACTIVATE, IntPtr.Zero, IntPtr.Zero);
                foreach (FocusChildForm f in this.OwnedForms.OfType<FocusChildForm>())
                {
                    f.ForceActiveBar = false;
                    NativeMethods.PostMessage(f.Handle, NativeConsts.WM_NCACTIVATE, IntPtr.Zero, IntPtr.Zero);
                }
                ignoreNcActivate = true;
            }
            else if (m.WParam == new IntPtr(1))
            {
                NativeMethods.SendMessage(this.Handle, NativeConsts.WM_NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                foreach (FocusChildForm f in this.OwnedForms.OfType<FocusChildForm>())
                {
                    f.ForceActiveBar = true;
                    NativeMethods.SendMessage(f.Handle, NativeConsts.WM_NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                }
            }
            return true;
        }

        private bool HandleNcActivate(WndProcHandler handler, ref Message m)
        {
            if (m.WParam == IntPtr.Zero)
            {
                if (ignoreNcActivate)
                {
                    ignoreNcActivate = false;
                }
                else
                {
                    NativeMethods.SendMessage(this.Handle, NativeConsts.WM_NCACTIVATE, new IntPtr(1), IntPtr.Zero);
                }
            }
            return true;
        }
    }
}
