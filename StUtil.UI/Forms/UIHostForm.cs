using System;
using System.Windows.Forms;

namespace StUtil.UI.Forms
{
    public class UIHostForm : StUtil.Native.Controls.FocusChildForm
    {
        public Panel PlaceholderControl { get; private set; }

        public Control TargetControl { get; private set; }
        private Control parent;

        public UIHostForm(Control targetControl)
        {
            this.TargetControl = targetControl;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;

            this.FormClosed += UIHostForm_FormClosed;
        }

        void UIHostForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Move -= TargetControl_Mirror;
            this.PlaceholderControl.Resize -= TargetControl_Mirror;
            this.TargetControl.VisibleChanged -= TargetControl_Mirror;
        }

        public UIHostForm Replace()
        {
            this.Show(TargetControl.FindForm());

            PlaceholderControl = new Panel()
            {
                Width = TargetControl.Width,
                Height = TargetControl.Height,
                Anchor = TargetControl.Anchor,
                Dock = TargetControl.Dock,
                Location = TargetControl.Location,
                BackColor = System.Drawing.Color.Transparent
            };

            this.Owner.Move += TargetControl_Mirror;
            this.PlaceholderControl.Resize += TargetControl_Mirror;
            this.TargetControl.VisibleChanged += TargetControl_Mirror;
            
            this.parent = TargetControl.Parent;
            this.parent.Move += TargetControl_Mirror;
            this.parent.Resize += TargetControl_Mirror;


            this.Controls.Add(TargetControl);
            TargetControl.Location = new System.Drawing.Point(0, 0);
            parent.Controls.Add(PlaceholderControl);
            MirrorTarget();

            return this;
        }

        public UIHostForm Restore()
        {
            return this;
        }

        private void MirrorTarget()
        {
            this.Size = PlaceholderControl.Size;
            this.Location = parent.PointToScreen(PlaceholderControl.Location);
        }

        private void TargetControl_Mirror(object sender, EventArgs e)
        {
            MirrorTarget();
            //if (PlaceholderControl.Width <= 0 || PlaceholderControl.Height <= 0)
            //{
            //    this.Opacity = 0;
            //}

            //if (this.Opacity == 0)
            //{
            //    if (TargetControl.Parent == this)
            //    {
            //        Owner.BringToFront();
            //    }
            //}
        }
    }
}