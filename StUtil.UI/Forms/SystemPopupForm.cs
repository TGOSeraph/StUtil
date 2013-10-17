using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.UI.Forms
{
    public class SystemPopupForm : Form
    {
        public event EventHandler<EventArgs<bool>> FocusChanged;

        public bool UseMousePosition { get; set; }
        public bool CloseOnFocusLost { get; set; }
        public bool Resizable { get; set; }

        public SystemPopupForm()
        {
            this.VisibleChanged += SystemPopupForm_VisibleChanged;
            this.BackColor = Color.White;
            this.Resizable = false;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
            this.Name = "SystemPopupForm";
        }

        private void SystemPopupForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && !this.InDesignMode())
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = true;
                this.ControlBox = false;
                this.Text = String.Empty;
                RelocateForm();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (this.InDesignMode())
            {
                base.WndProc(ref m);
                return;
            }

            const int WM_NCHITTEST = 0x84;
            const int WM_ACTIVATE = 6;
            const int WA_INACTIVE = 0;
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    if (!Resizable)
                    {
                        return;
                    }
                    break;
                case WM_ACTIVATE:
                    bool focus = ((int)m.WParam & 0xFFFF) != WA_INACTIVE;
                    this.FocusChanged.RaiseEvent(this, focus);
                    if (CloseOnFocusLost && !focus)
                    {
                        this.Hide();
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void RelocateForm()
        {
            Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            if (!UseMousePosition)
            {
                this.Left = workingRectangle.Width - this.Width - 9;
                this.Top = workingRectangle.Height - this.Height - 8;
            }
            else
            {
                int l = Cursor.Position.X - (this.Width / 2);
                if (l + this.Width > workingRectangle.Right)
                {
                    l = workingRectangle.Right - 5 - this.Width;
                }
                this.Location = new System.Drawing.Point(l, Cursor.Position.Y - this.Height - 35);
            }
        }
    }
}
