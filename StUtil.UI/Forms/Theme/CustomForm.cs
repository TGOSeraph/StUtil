using StUtil.Native.Internal;
using StUtil.UI.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Forms.Theme
{
    public partial class CustomForm : Form
    {
        private FormWindowState lastState;
        public Size? OverrideSize { get; set; }

        private FormBorder _border;
        [DefaultValue(typeof(FormBorder))]
        public FormBorder Border
        {
            get { return _border; }
            set
            {
                if (_border != null)
                {
                    _border.Dispose();
                }
                _border = value;
                _border.Apply();
            }
        }

        public CustomForm()
        {
            InitializeComponent();
            lastState = this.WindowState;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Border = new FormBorder(this);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (OverrideSize != null)
            {
                width = OverrideSize.Value.Width;
                height = OverrideSize.Value.Height;
                OverrideSize = null;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            base.OnHandleCreated(e);
        }

        protected override void WndProc(ref Message m)
        {
            switch ((NativeEnums.WM)m.Msg)
            {
                case NativeEnums.WM.NCCALCSIZE:
                    //Crop the form to remove the windows borders
                    if (m.WParam.Equals(IntPtr.Zero))
                    {
                        NativeStructs.RECT rc = (NativeStructs.RECT)m.GetLParam(typeof(NativeStructs.RECT));
                        Rectangle r = CropForm(rc);
                        Marshal.StructureToPtr(new NativeStructs.RECT(r), m.LParam, true);
                    }
                    else
                    {
                        NativeStructs.NCCALCSIZE_PARAMS csp = (NativeStructs.NCCALCSIZE_PARAMS)m.GetLParam(typeof(NativeStructs.NCCALCSIZE_PARAMS));
                        Rectangle r = CropForm(csp.rgrc0);
                        csp.rgrc0 = new NativeStructs.RECT(r);
                        Marshal.StructureToPtr(csp, m.LParam, true);
                    }
                    m.Result = IntPtr.Zero;
                    return;
                case NativeEnums.WM.SIZE:
                    if (this.WindowState != lastState)
                    {
                        lastState = this.WindowState;
                        if (this.WindowState == FormWindowState.Maximized)
                        {
                            OverrideSize = this.RestoreBounds.Size;
                        }
                        NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, NativeEnums.SWP.FRAMECHANGED | NativeEnums.SWP.NOACTIVATE | NativeEnums.SWP.NOMOVE | NativeEnums.SWP.NOSIZE | NativeEnums.SWP.NOZORDER);
                        this.Refresh();
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private Rectangle CropForm(Rectangle bounds)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                return Screen.FromControl(this).WorkingArea;
            }
            else
            {
                return bounds;
            }
        }
    }
}
