using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Forms
{
    public class ControlHostForm : Form
    {
        private Panel placeholder;

        private Control hostedControlParent;
        public Control HostedControl { get; private set; }

        private void Replace()
        {
            this.Owner = HostedControl.FindForm();

            hostedControlParent = HostedControl.Parent;

            placeholder = new Panel()
            {
                Width = HostedControl.Width,
                Height = HostedControl.Height,
                Anchor = HostedControl.Anchor,
                Dock = HostedControl.Dock,
                Location = HostedControl.Location,
                BackColor = System.Drawing.Color.Transparent
            };
            MirrorTarget();

            HostedControl.Location = new Point(0, 0);
            this.Controls.Add(HostedControl);
            hostedControlParent.Controls.Add(placeholder);

            this.Owner.Move += HostedControl_Mirror;
            this.placeholder.Resize += HostedControl_Mirror;
            this.HostedControl.VisibleChanged += HostedControl_Mirror;

            this.Owner.Focus();

        }

        private void MirrorTarget()
        {
            this.Size = placeholder.Size;
            this.Location = Owner.PointToScreen(placeholder.Location);

            if (placeholder.Width == 0 || placeholder.Height == 0)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }
        }

        private void HostedControl_Mirror(object sender, EventArgs e)
        {
            MirrorTarget();
        }

        private void Restore()
        {
            hostedControlParent.Controls.Add(HostedControl);
            HostedControl.Location = placeholder.Location;
            hostedControlParent.Controls.Remove(placeholder);
            this.Owner.Move -= HostedControl_Mirror;
            this.placeholder.Resize -= HostedControl_Mirror;
            this.HostedControl.VisibleChanged -= HostedControl_Mirror;
        }

        public ControlHostForm(Control hostedControl)
        {
            HostedControl = hostedControl;

            this.FormClosing += ControlHostForm_FormClosing;
        }

        void ControlHostForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Restore();
        }

        protected override void OnShown(EventArgs e)
        {
            Replace();
            base.OnShown(e);
        }

    }
}
