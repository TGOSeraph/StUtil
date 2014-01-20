using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class TopDockPanel : Panel
    {
        public bool AutoDockControls { get; set; }

        public TopDockPanel()
        {
            this.AutoDockControls = true;
            this.ControlAdded += new ControlEventHandler(TopDockPanel_ControlAdded);
        }

        private void TopDockPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            if (AutoDockControls)
            {
                e.Control.Dock = DockStyle.Top;
                e.Control.BringToFront();
            }
        }

        public void AddControl(Control ctrl)
        {
            this.AutoScroll = true;
            ctrl.Dock = DockStyle.Top;
            this.Controls.Add(ctrl);
            ctrl.BringToFront();
        }
    }
}
