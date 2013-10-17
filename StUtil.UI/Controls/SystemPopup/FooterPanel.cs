using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel.Design;

namespace StUtil.UI.Controls.SystemPopup
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))] 
    public partial class SystemPopupFooterPanel : UserControl
    {
        public Color BorderColor
        {
            get
            {
                return pnlFooterBorder.BackColor;
            }
            set
            {
                pnlFooterBorder.BackColor = value;
            }
        }
        public SystemPopupFooterPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Bottom;
        }
    }
}
