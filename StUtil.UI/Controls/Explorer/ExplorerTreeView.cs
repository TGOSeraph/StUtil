using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using StUtil.Extensions;
using StUtil.Internal.Shell;

namespace StUtil.UI.Controls.Explorer
{
    public partial class ExplorerTreeView : ExplorerTreeViewWnd
    {
        public ExplorerTreeView()
        {
            InitializeComponent();

            // Set the TreeView image list to the system image list.
            if (!this.InDesignMode())
            {
                SystemImageListHelper.SetImageList(this, new SystemImageList(), true);
                LoadRootNodes();
            }

            this.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        /// <summary>
        /// Loads the root TreeView nodes.
        /// </summary>
        private void LoadRootNodes()
        {
            // Create the root shell item.
            ShellItem m_shDesktop = ShellItem.GetRoot();

            // Create the root node.
            TriStateTreeNode tvwRoot = new TriStateTreeNode();
            tvwRoot.Text = m_shDesktop.DisplayName;
            tvwRoot.ImageIndex = m_shDesktop.IconIndex;
            tvwRoot.SelectedImageIndex = m_shDesktop.IconIndex;
            tvwRoot.Tag = m_shDesktop;

            // Now we need to add any children to the root node.
            List<ShellItem> arrChildren = m_shDesktop.GetDirectories();
            foreach (ShellItem shChild in arrChildren)
            {
                TriStateTreeNode tvwChild = new TriStateTreeNode();
                tvwChild.Text = shChild.DisplayName;
                tvwChild.ImageIndex = shChild.IconIndex;
                tvwChild.SelectedImageIndex = shChild.IconIndex;
                tvwChild.Tag = shChild;

                // If this is a folder item and has children then add a place holder node.
                if (shChild.IsFolder && shChild.HasSubFolder)
                    tvwChild.Nodes.Add("PH");
                tvwRoot.Nodes.Add(tvwChild);
            }

            // Add the root node to the tree.
            base.Nodes.Clear();
            base.Nodes.Add(tvwRoot);
            tvwRoot.Expand();
        }
    }
}
