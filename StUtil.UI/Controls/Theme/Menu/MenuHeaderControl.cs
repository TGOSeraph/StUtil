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

namespace StUtil.UI.Controls.Theme.Menu
{
    public partial class MenuHeaderControl : UserControl
    {
        public event EventHandler ParentSelected;

        private MenuItem menuItem;

        public MenuItem Item
        {
            get { return menuItem; }
            set
            {
                menuItem = value;
                TitleLabel.Text = value.Title;
                if (value.Parent == null)
                {
                    ArrowLabel.Visible = PathLabel.Visible = false;
                    TitleLabel.Left = 5;
                    Cursor = Cursors.Default;
                }
                else
                {
                    ArrowLabel.Visible = PathLabel.Visible = true;
                    TitleLabel.Left = 39;
                    Cursor = Cursors.Hand;

                    MenuItem item = value.Parent;
                    Stack<string> nodes = new Stack<string>();
                    while (item != null)
                    {
                        nodes.Push(item.Title);
                        item = item.Parent;
                    }
                    PathLabel.Left = TitleLabel.Right;
                    PathLabel.Text = string.Join(">", nodes);
                }
            }
        }

        public MenuHeaderControl()
        {
            InitializeComponent();

            this.Click += MenuHeaderControl_Click;
            TitleLabel.Click += MenuHeaderControl_Click;
            ArrowLabel.Click += MenuHeaderControl_Click;
            PathLabel.Click += MenuHeaderControl_Click;
        }

        private void MenuHeaderControl_Click(object sender, EventArgs e)
        {
            if (Item != null && Item.Parent != null)
            {
                ParentSelected.RaiseEvent(this);
            }
        }
    }
}
