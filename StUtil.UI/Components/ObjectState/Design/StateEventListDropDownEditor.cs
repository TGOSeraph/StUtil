using StUtil.UI.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components.ObjectState.Design
{
    public class StateEventListDropDownEditor : DropDownEditor
    {
        protected override object[] GetItems(object instance)
        {
            StateEvent item = (StateEvent)instance;
            if (item != null && item.Parent != null && item.Parent.Target != null)
            {
                return item
                    .Parent
                    .Target
                    .GetType()
                    .GetEvents()
                    .Select(e => e.Name)
                    .OrderBy(e => e)
                    .ToArray();
            }
            else
            {
                return new object[] { };
            }
        }
    }
}
