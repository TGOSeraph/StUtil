using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    /// NodeLineType enumeration
    /// Type of dotted line to draw when drawing a node.
    /// </summary>
    internal enum NodeLineType
    {
        None,
        Straight,
        WithChildren
    }

    /// <summary>
    ///  TriStateTreeNode class
    /// </summary>
    public class TriStateTreeNode : TreeNode
    {
      public event PropertyChangedEventHandler PropertyChanged;
        // stores node state ( either checked, unchecked or indeterminate )
        private CheckState _nodeCheckState = CheckState.Unchecked;

        // determines if the node should be regarded a container. 
        // Only a folder can have an third (indeterminate) state
        private bool _isContainer = true;
        private bool _checkboxVisible = true;
        private bool isUpdatingCheckState = false;

        // public constructor
        public TriStateTreeNode() : base() { Init(); }
        public TriStateTreeNode(string text) : base(text) { Init(); }
        public TriStateTreeNode(string text, int imageIndex, int selectedImageIndex) : base(text, imageIndex, selectedImageIndex) { Init(); }
        public TriStateTreeNode(string text, int imageIndex, int selectedImageIndex, TriStateTreeNode[] children) : base(text, imageIndex, selectedImageIndex, children) { Init(); }
        public TriStateTreeNode(string text, TriStateTreeNode[] children) : base(text, children) { Init(); }

        public BindingList<NodeData> Data { get; set; }

        private void Init()
        {
            Data = new BindingList<NodeData>();
        }

        /// <summary>
        /// Get / set if the node is checked or not.
        /// </summary>
        [Browsable(false)]
        new public bool Checked
        {
            get { return (this._nodeCheckState != CheckState.Unchecked); }
        }

        new public TriStateTreeNode Parent
        {
            get
            {
                return base.Parent as TriStateTreeNode;
            }
        }

        /// <summary>
        /// Get's the node's current state, either checked / unchecked for non-container nodes,
        /// or checked / unchecked / indeterminate for container nodes.
        /// </summary>
        public CheckState CheckState
        {
            get
            {
                return _nodeCheckState;
            }
            set
            {
                if (this._nodeCheckState != value && !isUpdatingCheckState)
                {
                    this._nodeCheckState = value;
                    UpdateTreeCheckState();
                }
            }
        }

        /// <summary>
        /// Determines if the node should act as a container
        /// </summary>
        public bool IsContainer
        {
            get { return _isContainer; }
            set { _isContainer = value; }
        }

        public bool CheckboxVisible
        {
            get { return this._checkboxVisible; }
            set { this._checkboxVisible = value; }
        }

        internal NodeLineType NodeLineType
        {
            get
            {
                // is this node bound to a treeview ?
                if (null != this.TreeView)
                {
                    // do we need to draw lines at all?
                    if (!this.TreeView.ShowLines) { return NodeLineType.None; }
                    if (this.CheckboxVisible) { return NodeLineType.None; }

                    if (this.Nodes.Count > 0)
                    {
                        return NodeLineType.WithChildren;
                    }
                    return NodeLineType.Straight;
                }

                // no treeview so this node will never been drawn at all
                return NodeLineType.None;
            }
        }

        private void UpdateTreeCheckState()
        {
            isUpdatingCheckState = true;
            PropertyChanged.RaiseEvent(this, "CheckState");
            //If the node is checked/unchecked then do the same to its children
            if (this._nodeCheckState != System.Windows.Forms.CheckState.Indeterminate)
            {
                foreach (TriStateTreeNode child in Nodes)
                {
                    child.CheckState = this._nodeCheckState;
                }
                foreach (NodeData child in Data)
                {
                    child.IsChecked = false;
                }

                //Now process the parent
                if (this.Parent != null)
                {
                    if (this.Parent.Data.All(d => d.IsChecked == (this._nodeCheckState == System.Windows.Forms.CheckState.Checked))
                        && this.Parent.Nodes.OfType<TriStateTreeNode>().All(n => n.CheckState == this._nodeCheckState))
                    {
                        this.Parent.CheckState = this._nodeCheckState;
                    }
                    else
                    {
                        this.Parent.CheckState = System.Windows.Forms.CheckState.Indeterminate;
                    }
                }
            }
            else
            {
                if (this.Parent != null)
                {
                    this.Parent.CheckState = System.Windows.Forms.CheckState.Indeterminate;
                }
            }

            isUpdatingCheckState = false;
        }
    }
    public class NodeData : INotifyPropertyChanged
    {
        public TriStateTreeNode Parent { get; set; }
        public string Data { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    PropertyChanged.RaiseEvent(this, "IsChecked");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

