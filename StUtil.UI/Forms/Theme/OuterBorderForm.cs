using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Forms.Theme
{
    public abstract partial class OuterBorderForm : Form
    {
        public TabAlignment Side { get; private set; }
        public int BorderSize
        {
            get;
            private set;
        }

        public virtual bool Active { get; set; }

        public void SetSize(int size)
        {
            this.BorderSize = size;
            int diff;
            switch (this.Side)
            {
                case TabAlignment.Top:
                    diff = this.Height - size;
                    this.Height = size;
                    this.Top += diff;
                    break;
                case TabAlignment.Bottom:
                    this.Height = size;
                    break;
                case TabAlignment.Left:
                    diff = this.Width - size;
                    this.Width = size;
                    this.Left += diff;
                    break;
                case TabAlignment.Right:
                    this.Width = size;
                    break;
            }
        }

        public OuterBorderForm(TabAlignment side, int size)
        {
            this.BorderSize = size;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Side = side;
            this.Width = 0;
            this.Height = 0;
            this.BackColor = Color.FromArgb(202, 81, 0);
            this.StartPosition = FormStartPosition.Manual;
            SetSize(size);
        }

        private bool AtTop(int y)
        {
            return y < this.Width;
        }

        private bool AtBottom(int y)
        {
            return y > this.Height - this.Width;
        }

        private bool AtRight(int x)
        {
            return x > this.Width - (this.Height * 2);
        }

        private bool AtLeft(int x)
        {
            return x < (this.Height * 2);
        }

        public abstract void Update(object args);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            switch (Side)
            {
                case TabAlignment.Left:
                    if (AtTop(e.Y))
                    {
                        Cursor = Cursors.SizeNWSE;
                    }
                    else if (AtBottom(e.Y))
                    {
                        Cursor = Cursors.SizeNESW;
                    }
                    else
                    {
                        Cursor = Cursors.SizeWE;
                    }
                    break;
                case TabAlignment.Right:
                    if (AtTop(e.Y))
                    {
                        Cursor = Cursors.SizeNESW;
                    }
                    else if (AtBottom(e.Y))
                    {
                        Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        Cursor = Cursors.SizeWE;
                    }
                    break;
                case TabAlignment.Top:
                    if (AtLeft(e.X))
                    {
                        Cursor = Cursors.SizeNWSE;
                    }
                    else if (AtRight(e.X))
                    {
                        Cursor = Cursors.SizeNESW;
                    }
                    else
                    {
                        Cursor = Cursors.SizeNS;
                    }
                    break;
                case TabAlignment.Bottom:
                    if (AtLeft(e.X))
                    {
                        Cursor = Cursors.SizeNESW;
                    }
                    else if (AtRight(e.X))
                    {
                        Cursor = Cursors.SizeNWSE;
                    }
                    else
                    {
                        Cursor = Cursors.SizeNS;
                    }
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                NativeEnums.HT val = NativeEnums.HT.NOWHERE;

                if (Cursor == Cursors.SizeNS)
                {
                    if (Side == TabAlignment.Top)
                    {
                        val = NativeEnums.HT.TOP;
                    }
                    else
                    {
                        val = NativeEnums.HT.BOTTOM;
                    }
                }
                else if (Cursor == Cursors.SizeNESW)
                {
                    switch (Side)
                    {
                        case TabAlignment.Top:
                            val = NativeEnums.HT.TOPRIGHT;
                            break;
                        case TabAlignment.Bottom:
                            val = NativeEnums.HT.BOTTOMLEFT;
                            break;
                        case TabAlignment.Right:
                            val = NativeEnums.HT.TOPRIGHT;
                            break;
                        case TabAlignment.Left:
                            val = NativeEnums.HT.BOTTOMLEFT;
                            break;
                    }
                }
                else if (Cursor == Cursors.SizeNWSE)
                {
                    switch (Side)
                    {
                        case TabAlignment.Top:
                            val = NativeEnums.HT.TOPLEFT;
                            break;
                        case TabAlignment.Bottom:
                            val = NativeEnums.HT.BOTTOMRIGHT;
                            break;
                        case TabAlignment.Right:
                            val = NativeEnums.HT.BOTTOMRIGHT;
                            break;
                        case TabAlignment.Left:
                            val = NativeEnums.HT.TOPLEFT;
                            break;
                    }
                }
                else if (Cursor == Cursors.SizeWE)
                {
                    if (Side == TabAlignment.Left)
                    {
                        val = NativeEnums.HT.LEFT;
                    }
                    else
                    {
                        val = NativeEnums.HT.RIGHT;
                    }
                }

                if (val != NativeEnums.HT.NOWHERE)
                {
                    this.Capture = false;
                    this.Owner.Focus();
                    NativeMethods.SendMessage(this.Owner.Handle, NativeEnums.WM.NCLBUTTONDOWN, new IntPtr((int)val), IntPtr.Zero);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch ((NativeEnums.WM)m.Msg)
            {
                case NativeEnums.WM.ACTIVATEAPP:
                case NativeEnums.WM.ACTIVATE:
                    if (!StUtil.Native.Input.Mouse.IsButtonDown(System.Windows.Forms.MouseButtons.Left))
                    {
                        this.Owner.Focus();
                        m.Result = new IntPtr(-1);
                    }
                    return;
            }
            base.WndProc(ref m);
        }
    }
}
