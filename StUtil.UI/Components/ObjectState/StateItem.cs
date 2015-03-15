using StUtil.UI.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components.ObjectState
{
    public class StateItem : ComponentChildItem<HostedComponent>
    {
        [Editor(typeof(ControlListDropDownEditor<StateItem>), typeof(UITypeEditor))]
        [Description("The control to target")]
        [Category("General")]
        public object Target { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DefaultValue(typeof(ComponentChildItemCollection<StateItem, StateEvent>))]
        [Category("General")]
        [Description("A collection of events that trigger states")]
        public ComponentChildItemCollection<StateItem, StateEvent> HandledEvents { get; set; }

        public StateItem()
        {
            this.HandledEvents = new ComponentChildItemCollection<StateItem, StateEvent>(this);
            this.HandledEvents.Parent = this;
        }

        public override string ToString()
        {
            return Target == null ? "(Target not set)" : Target.ToString();
        }
    }
}
