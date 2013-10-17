using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel.Design;
using StUtil.Extensions;

namespace StUtil.UI.Controls.SystemPopup
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class SystemPopupHeaderPanel : UserControl
    {
        public event EventHandler<MouseEventArgs> HeaderClicked;
        private List<Control> HeaderListenersAdded = new List<Control>();

        private Color backColor;
        private Color borderColor;

        public Color HeaderHoverColor { get; set; }
        public Color BorderHoverColor { get; set; }
        public Color BorderColor
        {
            get
            {
                return this.pnlHeaderBorder.BackColor;
            }
            set
            {
                this.pnlHeaderBorder.BackColor = value;
            }
        }

        private bool headerClickEnabled = false;
        public bool HeaderClickEnabled
        {
            get
            {
                return headerClickEnabled;
            }
            set
            {
                if (value)
                {
                    SetupHeaderMouseListeners();
                }
                else
                {
                    RemoveHeaderMouseListeners();
                }
            }
        }

        public string TitleText
        {
            get
            {
                return this.lblTitle.Text;
            }
            set
            {
                this.lblTitle.Text = value;
            }
        }

        public string DescriptionText
        {
            get
            {
                return this.lblDescription.Text;
            }
            set
            {
                this.lblDescription.Text = value;
            }
        }

        public Image Icon
        {
            get
            {
                return pbIcon.Image;
            }
            set
            {
                if (value == null)
                {
                    lblDescription.Left = pbIcon.Left;
                    lblTitle.Left = pbIcon.Left;
                    pbIcon.Visible = false;
                }
                else
                {
                    lblDescription.Left = pbIcon.Right + 6;
                    lblTitle.Left = pbIcon.Right + 6;
                    pbIcon.Visible = true;
                }
                pbIcon.Image = value;
            }
        }

        public void SetupHeaderMouseListeners()
        {
            lock (HeaderListenersAdded)
            {
                this.headerClickEnabled = true;
                List<Control> newAdded = new List<Control>();
                foreach (Control ctrl in this.Controls)
                {
                    if (!HeaderListenersAdded.Contains(ctrl))
                    {
                        ctrl.MouseEnter += Header_MouseEnter;
                        ctrl.MouseLeave += Header_MouseLeave;
                        ctrl.MouseClick += Header_MouseClick;
                    }
                    newAdded.Add(ctrl);
                }
                this.MouseEnter += Header_MouseEnter;
                this.MouseLeave += Header_MouseLeave;
                this.MouseClick += Header_MouseClick;
                HeaderListenersAdded = newAdded;
            }
        }

        public void RemoveHeaderMouseListeners()
        {
            lock (HeaderListenersAdded)
            {
                this.headerClickEnabled = false;
                foreach (Control ctrl in HeaderListenersAdded)
                {
                    if (!ctrl.IsDisposed)
                    {
                        ctrl.MouseEnter -= Header_MouseEnter;
                        ctrl.MouseLeave -= Header_MouseLeave;
                        ctrl.MouseClick -= Header_MouseClick;
                    }
                }
                this.MouseEnter -= Header_MouseEnter;
                this.MouseLeave -= Header_MouseLeave;
                this.MouseClick -= Header_MouseClick;
                HeaderListenersAdded.Clear();
            }
        }

        private void Header_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderClicked.RaiseEvent(this, e);
        }

        private void Header_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = this.backColor;
            this.pnlHeaderBorder.Height = 1;
            this.BorderColor = borderColor;
        }

        private void Header_MouseEnter(object sender, EventArgs e)
        {
            this.borderColor = this.BorderColor;
            this.backColor = this.BackColor;
            this.BackColor = this.HeaderHoverColor;
            this.pnlHeaderBorder.Height = 2;
            this.BorderColor = BorderHoverColor;
        }

        public SystemPopupHeaderPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;
            this.Icon = null;
            this.HeaderHoverColor = Color.FromArgb(229, 243, 251);
            this.BorderHoverColor = Color.FromArgb(112, 192, 231);
        }
    }
}
