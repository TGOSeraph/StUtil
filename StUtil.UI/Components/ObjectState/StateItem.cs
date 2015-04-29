using StUtil.Extensions;
using StUtil.Generic;
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
        public event EventHandler<ValueChangedEventArgs<string>> StateChanged;

        [Editor(typeof(ControlListDropDownEditor<StateItem>), typeof(UITypeEditor))]
        [Description("The control to target")]
        [Category("General")]
        public object Target { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DefaultValue(typeof(ComponentChildItemCollection<StateItem, StateEvent>))]
        [Category("General")]
        [Description("A collection of events that trigger states")]
        public ComponentChildItemCollection<StateItem, StateEvent> HandledEvents { get; set; }

        private string state;
        [Category("General")]
        [Description("The identifier of the state the control is currently in")]
        [DefaultValue("")]
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                string old = state;
                state = value;
                StateChanged.RaiseEvent(this, new ValueChangedEventArgs<string>(value, old));
            }
        }

        public StateItem()
        {
            this.HandledEvents = new ComponentChildItemCollection<StateItem, StateEvent>(this);
            this.HandledEvents.Parent = this;
            this.State = "";
        }

        public override string ToString()
        {
            return Target == null ? "(Target not set)" : Target.ToString();
        }
    }
}
