using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Extensions;
using StUtil.UI.Forms;

namespace StUtil.UI.Controls
{
    public partial class LoadingOverlay : UserControl
    {
        private Action onCancel;

        public Image LoadingImage
        {
            get
            {
                return pbImage.Image;
            }
            set
            {
                pbImage.Image = value;
                pbImage.Center();
                lblText.Top = pbImage.Bottom;
                lblText.Center(ControlExtensions.CenterMode.Horizontal);
                lblText.Width = this.Width;
                llblCancel.Center(ControlExtensions.CenterMode.Horizontal);
                llblCancel.Top = lblText.Bottom + 5;
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

        public LoadingOverlay(Control ctrl, Action onCancel)
            : this()
        {
            this.Parent = ctrl.Parent;
            this.Size = ctrl.Size;
            this.Location = ctrl.Location;
            this.Dock = ctrl.Dock;
            this.Anchor = ctrl.Anchor;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BringToFront();
            this.llblCancel.Visible = onCancel != null;
            this.onCancel = onCancel;
        }

        public LoadingOverlay()
        {
            InitializeComponent();
        }

        private void llblCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (onCancel != null)
            {
                onCancel();
            }
        }
    }
}
