using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components.ObjectState
{
    public class StateEventProperties : ComponentChildItem<StateEvent>
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [DefaultValue(typeof(ComponentChildItemCollection<StateEventProperties, StateEventProperty>))]
        [Category("General")]
        public ComponentChildItemCollection<StateEventProperties, StateEventProperty> Properties { get; set; }

        public StateEventProperties()
        {
            this.Properties = new ComponentChildItemCollection<StateEventProperties, StateEventProperty>(this);
            this.Properties.Parent = this;
        }

        public StateEventProperties(Dictionary<string, object> values)
            : this()
        {
            this.Properties = new ComponentChildItemCollection<StateEventProperties, StateEventProperty>(this);
            foreach (var kvp in values)
            {
                this.Properties.Add(new StateEventProperty
                {
                    Parent = this,
                    PropertyName = kvp.Key,
                    Value = kvp.Value
                });
            }
        }

        public override string ToString()
        {
            return this.Properties.Count == 0 ? "{Not Set}" : this.Properties.Count == 1 ? "{1 Property}" : ("{" + this.Properties.Count.ToString() + " Properties}");
        }
    }
}
