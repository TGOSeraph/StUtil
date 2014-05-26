using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls.Theme.Menu
{
    public partial class MenuContainer : UserControl
    {
        public event EventHandler SelectedItemChanged;

        private MenuItemControl highlightedItem;
        private MenuItem root;

        private MenuItemControl selectedItem;

        public MenuItemControl HighlightedItem
        {
            get { return highlightedItem; }
            set
            {
                if (highlightedItem != null)
                {
                    highlightedItem.IsHiglighted = false;
                }
                highlightedItem = value;
            }
        }

        public MenuItem Root
        {
            get { return root; }
            set
            {
                if (value == null) return;

                if (root != null)
                {
                    root.Children.ListChanged -= Children_ListChanged;
                }

                root = value;
                root.Children.ListChanged += Children_ListChanged;

                Populate(root);
            }
        }

        public MenuItemControl SelectedItem
        {
            get
            {
                return selectedItem;
            }
        }

        public MenuContainer()
        {
            InitializeComponent();
            Header.ParentSelected += Header_ParentSelected;
        }

        private MenuItemControl AddControl(MenuItemControl mItem)
        {
            tdpTasks.AddControl(mItem);
            mItem.ItemClicked += mItem_ItemClicked;
            return mItem;
        }

        private void Children_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                bool isFirst = false;
                AddItem(root.Children[e.NewIndex], false, ref isFirst);
            }
            else if (e.ListChangedType == ListChangedType.ItemChanged)
            {

            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {

            }
        }

        private void Header_ParentSelected(object sender, EventArgs e)
        {
            Root = ((MenuHeaderControl)sender).Item.Parent;
        }

        private void mItem_ItemClicked(object sender, EventArgs e)
        {
            MenuItemControl item = ((MenuItemControl)sender);

            if (selectedItem == item)
            {
                return;
            }
            selectedItem = item;
            if (item.Item.Children.Count > 0)
            {
                Root = item.Item;
            }
            else
            {
                HighlightedItem.IsHiglighted = false;
                HighlightedItem = item;
                item.IsHiglighted = true;
            }
            SelectedItemChanged.RaiseEvent(item);
        }

        private void Populate(MenuItem item)
        {
            bool isFirst = true;
            tdpTasks.Controls.Clear();
            foreach (MenuItem child in item.Children)
            {
                AddItem(child, item.AutoSelect, ref isFirst);
            }

            Header.Item = item;
        }

        private void AddItem(MenuItem child, bool autoSelect, ref bool isFirst)
        {
            var ctrl = AddControl(new MenuItemControl(child));
            child.Control = ctrl;
            child.ForceSelected += (s, o) =>
            {
                HighlightedItem = ctrl;
                ctrl.IsHiglighted = true;
                ctrl.Item.IsSelected = true;
                selectedItem = ctrl;
                SelectedItemChanged.RaiseEvent(ctrl);
            };
            if (child.Task != null)
            {
                child.Task.TitleChanged += (s, o) =>
                {
                    ctrl.TitleLabel.Text = child.Task.Title;
                };
            }
            if (isFirst)
            {
                if (autoSelect)
                {
                    HighlightedItem = ctrl;
                    ctrl.IsHiglighted = true;
                    ctrl.Item.IsSelected = true;
                    selectedItem = ctrl;
                    SelectedItemChanged.RaiseEvent(ctrl);
                }
                isFirst = false;
            }
        }
    }
}
