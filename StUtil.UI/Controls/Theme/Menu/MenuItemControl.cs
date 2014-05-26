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
using StUtil.UI.Utilities;

namespace StUtil.UI.Controls.Theme.Menu
{
    public partial class MenuItemControl : Theme.ThemePanel
    {
        public event EventHandler ItemClicked;
        public MenuItem Item { get; private set; }

        private bool highlighted;

        public bool IsHiglighted
        {
            get { return highlighted; }
            set
            {
                highlighted = value;
                if (value)
                {
                    this.Style = Theme.ThemeManager.Style.Highlight;
                    ArrowLabel.Style = IconLabel.Style = TitleLabel.Style = Theme.ThemeManager.Style.White;
                }
                else
                {
                    this.Style = Theme.ThemeManager.Style.Dark;
                    ArrowLabel.Style = IconLabel.Style = TitleLabel.Style = Theme.ThemeManager.Style.MediumLight;
                }
            }
        }

        public MenuItemControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;

            this.MouseEnter += TaskItem_MouseEnter;
            this.MouseLeave += TaskItem_MouseLeave;
            IconLabel.MouseEnter += TaskItem_MouseEnter;
            IconLabel.MouseLeave += TaskItem_MouseLeave;
            ArrowLabel.MouseEnter += TaskItem_MouseEnter;
            ArrowLabel.MouseLeave += TaskItem_MouseLeave;
            TitleLabel.MouseEnter += TaskItem_MouseEnter;
            TitleLabel.MouseLeave += TaskItem_MouseLeave;

            this.Click += MenuItemControl_Click;
            IconLabel.Click += MenuItemControl_Click;
            ArrowLabel.Click += MenuItemControl_Click;
            TitleLabel.Click += MenuItemControl_Click;
        }

        public MenuItemControl(MenuItem item)
            : this()
        {
            this.Item = item;
            SetIcon(item.Icon);
            this.TitleLabel.Text = item.Title;
            ArrowLabel.Visible = item.Children.Count > 0;
        }

        public void SetIcon(UISymbol symbol)
        {
            if (symbol != UISymbol.None)
            {
                this.IconLabel.Symbol = symbol;
                this.IconLabel.Visible = true;
                this.TitleLabel.Left = this.IconLabel.Right + 10;
            }
            else
            {
                this.IconLabel.Visible = false;
                this.TitleLabel.Left = this.IconLabel.Left + 10;
            }
        }

        void MenuItemControl_Click(object sender, EventArgs e)
        {
            ItemClicked.RaiseEvent(this);
            Item.IsSelected = true;
        }

        void TaskItem_MouseLeave(object sender, EventArgs e)
        {
            ArrowLabel.Style = IconLabel.Style = TitleLabel.Style = IsHiglighted
                ? Theme.ThemeManager.Style.Text 
                : Theme.ThemeManager.Style.MediumLight;
        }

        void TaskItem_MouseEnter(object sender, EventArgs e)
        {
            ArrowLabel.Style = IconLabel.Style = TitleLabel.Style = Theme.ThemeManager.Style.White;
        }
    }
}
