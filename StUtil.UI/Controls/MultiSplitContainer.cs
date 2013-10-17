using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using StUtil.UI.Extensions;

namespace StUtil.UI.Controls
{
    public class MultiSplitContainer : ContainerControl
    {
        private List<SplitterPanel> panels = new List<SplitterPanel>();
        public IEnumerable<SplitterPanel> Panels
        {
            get
            {
                return panels;
            }
        }

        private List<SplitContainer> containers = new List<SplitContainer>();

        private Orientation setOrientation;
        public Orientation Orientation
        {
            get
            {
                return containers.Count == 0 ? Orientation.Vertical : containers[0].Orientation;
            }
            set
            {
                if (containers[0].Width == 0)
                {
                    setOrientation = value;
                    this.SizeChanged += MultiSplitContainer_SizeChanged;
                }
                else
                {
                    foreach (SplitContainer container in containers)
                    {
                        container.SplitterDistance = container.Panel1MinSize;
                        container.Orientation = value;
                    }
                }
            }
        }

        public MultiSplitContainer()
        {
            SplitContainer container = CreateSplitContainer();
            this.containers.Add(container);
            this.Controls.Add(container);
            this.panels.Add(container.Panel1);

        }

        private void MultiSplitContainer_SizeChanged(object sender, EventArgs e)
        {
            Orientation = setOrientation;
            this.SizeChanged -= MultiSplitContainer_SizeChanged;
        }

        public SplitterPanel AddPanel()
        {
            return AddPanel(null);
        }
        public SplitterPanel AddPanel(Control contents)
        {
            return InsertPanel(panels.Count, contents);
        }
        public SplitterPanel InsertPanel(int index)
        {
            return InsertPanel(index, null);
        }
        public SplitterPanel InsertPanel(int index, Control contents)
        {
            SplitContainer sc = CreateSplitContainer();

            if (contents != null)
            {
                sc.Panel1.Controls.Add(contents);
            }

            SplitContainer prev;

            if (index == 0)
            {
                prev = containers[0];
                this.Controls.Remove(prev);
                sc.Panel2.Controls.Add(prev);
                sc.Panel2Collapsed = false;
                containers.Insert(index, sc);
                panels.Insert(index, sc.Panel1);
                this.Controls.Add(sc);
            }
            else
            {
                containers.Insert(index, sc);
                prev = containers[index - 1];
                SplitContainer store = null;
                if (prev.Panel2.Controls.Count > 0)
                {
                    store = (SplitContainer)prev.Panel2.Controls[0];
                    prev.Panel2.Controls.Clear();
                }

                if (store != null)
                {
                    sc.Panel2.Controls.Add(store);
                    sc.Panel2Collapsed = false;
                }
                prev.Panel2Collapsed = false;
                prev.Panel2.Controls.Add(sc);
                panels.Insert(index, sc.Panel1);
            }

            return sc.Panel1;
        }

        public void RemoveAt(int index)
        {
            Remove(panels[index]);
        }

        public void Remove(SplitterPanel panel)
        {
            SplitContainer parent = (SplitContainer)panel.Parent;
        
            if (this.panels[0] == panel)
            {
                if (this.panels.Count == 1)
                {
                    panel.Controls.Clear();
                    return;
                }
                else
                {
                    Control container = panel.Parent.Parent;
                    Control right = null;
                    if (parent.Panel2.Controls.Count > 0)
                    {
                        right = parent.Panel2.Controls[0];
                    }
                    int index = container.Controls.GetChildIndex(parent);
                    container.Controls.Remove(parent);
                    container.Controls.Add(right);
                    container.Controls.SetChildIndex(right, index);
                    this.panels.Remove(panel);
                    this.containers.Remove(parent);
                }
            }
            else
            {
                SplitContainer container = (SplitContainer)parent.Parent.Parent;
                Control right = null;
                if (parent.Panel2.Controls.Count > 0)
                {
                    right = parent.Panel2.Controls[0];
                }
                container.Panel2.Controls.Remove(parent);
                if (right != null)
                {
                    container.Panel2.Controls.Add(right);
                }
                this.panels.Remove(panel);
                this.containers.Remove(parent);
                if (panels.Count == 1)
                {
                    container.Panel2Collapsed = true;
                }
            }


            //panels.Remove(panel);
            //SplitContainer parent = (SplitContainer)panel.Parent;
            //containers.Remove(parent);

            //Control store = null;
            //if (parent.Panel2.Controls.Count > 0)
            //{
            //    store = parent.Panel2.Controls[0];
            //}
            //Control parentParent = parent.Parent;
            //parentParent.Controls.Remove(parent);

            //((SplitContainer)parentParent.Parent).Panel2Collapsed = true;

            //if (store != null)
            //{
            //    parentParent.Controls.Add(store);
            //}
        }

        private SplitContainer CreateSplitContainer()
        {
            return new SplitContainer
            {
                Dock = DockStyle.Fill,
                Panel2Collapsed = true,
                Orientation = Orientation
            };
        }

        public void ResizePanelsEvenly()
        {
            int w;
            if (Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                w = (this.Height - containers[0].SplitterWidth * Panels.Count()) / (Panels.Count());
            }
            else
            {
                w = (this.Width - containers[0].SplitterWidth * Panels.Count()) / (Panels.Count());
            }
            foreach (SplitterPanel panel in Panels)
            {
                panel.SetWidth(w);
            }
        }
    }
}
