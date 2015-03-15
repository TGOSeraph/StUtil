using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components
{
    public class ComponentChildItem
    {
        [Browsable(false)]
        public object Parent { get; set; }
    }

    public class ComponentChildItem<TParent> : ComponentChildItem
    {
        [Browsable(false)]
        public new TParent Parent
        {
            get
            {
                return (TParent)base.Parent;
            }
            set
            {
                base.Parent = value;
            }
        }
    }
}
