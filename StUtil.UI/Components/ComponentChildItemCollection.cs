using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components
{
    public class ComponentChildItemCollection<TParent, TItem> : BindingList<TItem>
        where TItem : ComponentChildItem
    {
        [Browsable(false)]
        public TParent Parent { get; set; }

        public ComponentChildItemCollection(TParent helper)
        {
            this.Parent = helper;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                TItem item = this[e.NewIndex];
                item.Parent = Parent;
            }
            base.OnListChanged(e);
        }
    }
}
