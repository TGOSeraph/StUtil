using StUtil.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Design
{
    public class ControlListDropDownEditor<TItem> : DropDownEditor
         where TItem : ComponentChildItem<HostedComponent>
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return base.EditValue(context, provider, value);
        }

        protected override object[] GetItems(object instance)
        {
            TItem item = (TItem)instance;
            if (item != null && item.Parent != null && item.Parent.ContainerControl != null)
            {
                return item
                    .Parent
                    .ContainerControl
                    .Controls
                    .OfType<Control>()
                    .Concat(new[] { item.Parent.ContainerControl })
                    .ToArray();
            }
            else
            {
                return new object[] { };
            }
        }
    }
}
