using StUtil.Extensions;
using StUtil.UI.Controls.Theme.Menu;
using StUtil.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StUtil.UI.Controls.Theme.Menu
{
    public class MenuItem
    {
        public event EventHandler Deselected;
        public event EventHandler Selected;
        internal event EventHandler ForceSelected;

        private bool isSelected;
        public bool AutoSelect { get; set; }
        public BindingList<MenuItem> Children { get; set; }
        public UISymbol Icon { get; set; }
        public MenuItemControl Control { get; set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (value)
                {
                    Selected.RaiseEvent(this);
                }
                else
                {
                    Deselected.RaiseEvent(this);
                }
            }
        }

        public MenuItem Parent { get; private set; }

        private MenuTask task;
        public MenuTask Task
        {
            get
            {
                return task;
            }
            set
            {
                task = value;
                if (task != null)
                {
                    task.MenuItem = this;
                }
            }
        }

        public string Title { get; set; }

        public MenuItem(string title)
            : this(title, null)
        {
        }

        public MenuItem(string title, MenuTask task)
            : this(title, UISymbol.None, task)
        {
        }

        public MenuItem(string title, UISymbol icon)
            : this(title, icon, null)
        {
        }

        public MenuItem(string title, UISymbol icon, MenuTask task)
        {
            this.Title = title;
            this.Children = new BindingList<MenuItem>();
            this.Children.ListChanged += Children_ListChanged;
            this.Task = task;
            this.Icon = icon;
        }

        public static MenuItem Create(string title, UISymbol icon, MenuTask task, List<MenuItem> children, EventHandler selected = null, EventHandler deselected = null)
        {
            MenuItem item = new MenuItem(title, icon, task);
            if (selected != null)
            {
                item.Selected += selected;
            }
            if (deselected != null)
            {
                item.Deselected += deselected;
            }
            if (children != null)
            {
                foreach (MenuItem c in children)
                {
                    item.Children.Add(c);
                }
            }
            return item;
        }

        private void Children_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Children[e.NewIndex].Parent = this;
            }
        }

        public void SetSelected()
        {
            ForceSelected.RaiseEvent(this);
        }
    }
}