using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Native.Internal;

namespace StUtil.UI.Controls.Theme
{
    public partial class CaptionBarControl : UserControl
    {
        private Form form;
        protected Form Form
        {
            get
            {
                if (form == null)
                {
                    form = base.FindForm();
                }
                return form;
            }
        }

        public override string Text
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }


        public CaptionBarControl()
        {
            InitializeComponent();

            this.Dock = DockStyle.Top;
            lblText.DisableMouseEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pbIcon.Image = Form.Icon.ToBitmap();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Capture = false;
                Message msg = Message.Create(this.Form.Handle, (int)NativeEnums.WM.NCLBUTTONDOWN, (IntPtr)NativeEnums.HT.CAPTION, IntPtr.Zero);
                WndProc(ref msg);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ShowSysMenu();
            }
        }

        private void ShowSysMenu()
        {
            int menuItem = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(Form.Handle, false), NativeEnums.TPM.LEFTBUTTON | NativeEnums.TPM.RETURNCMD, Cursor.Position.X, Cursor.Position.Y + 1, Form.Handle, IntPtr.Zero);
            Message msg = Message.Create(Form.Handle, (int)NativeEnums.WM.SYSCOMMAND, (IntPtr)menuItem, IntPtr.Zero);
            WndProc(ref msg);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (Form.WindowState == FormWindowState.Maximized)
                {
                    Form.WindowState = FormWindowState.Normal;
                }
                else
                {
                    Form.WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Form.Close();
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            if (this.Form.WindowState == FormWindowState.Maximized)
            {
                this.Form.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Form.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.form.WindowState = FormWindowState.Minimized;
        }

        private void pbIcon_Click(object sender, EventArgs e)
        {
            ShowSysMenu();
        }

        private void pbIcon_DoubleClick(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }
    }
}
