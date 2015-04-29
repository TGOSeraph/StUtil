using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Components.ObjectState.Design
{
    public partial class StateEventPropertiesForm : Form
    {
        public object Target { get; set; }
        private StateEventProperties values;
        private CustomObjectType customObj;

        public StateEventPropertiesForm(StateEventProperties values)
            : this()
        {
            this.values = values;
        }

        public StateEventPropertiesForm()
        {
            InitializeComponent();
        }

        internal StateEventProperties GetResults()
        {
            var props = new StateEventProperties(customObj.Values);
            return props;
        }

        protected override void OnShown(EventArgs e)
        {
            customObj = new CustomObjectType(this.values.Properties.ToDictionary(k => k.PropertyName, k => k.Value));
            customObj.Properties.AddRange(Target.GetType().GetProperties().Select(p => new CustomProperty
            {
                Description = p
                    .GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .Select(a => ((DescriptionAttribute)a).Description)
                    .FirstOrDefault(),
                Category = p
                    .GetCustomAttributes(typeof(CategoryAttribute), true)
                    .Select(a => ((CategoryAttribute)a).Category)
                    .FirstOrDefault(),
                Name = p.Name,
                Type = p.PropertyType,
                Value = p.GetValue(Target)
            }));
            customObj.Properties.Add(new CustomProperty() {
                Description = "ObjectState",
                Name = "$ObjectState",
                Type = typeof(string),
                Value = null,
                Category = " Object State"
            });

            propertyGrid1.SelectedObject = customObj;

            base.OnShown(e);
        }

        [TypeConverter(typeof(CustomObjectType.CustomObjectConverter))]
        public class CustomObjectType
        {
            private readonly List<CustomProperty> props = new List<CustomProperty>();
            [Browsable(false)]
            public List<CustomProperty> Properties { get { return props; } }

            private Dictionary<string, object> values = new Dictionary<string, object>();

            [Browsable(false)]
            public Dictionary<string, object> Values
            {
                get
                {
                    return values;
                }
            }

            public CustomObjectType(Dictionary<string, object> values)
            {
                if (values == null)
                {
                    values = new Dictionary<string, object>();
                }
                this.values = values;
            }

            private class CustomObjectConverter : ExpandableObjectConverter
            {
                public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
                {
                    var stdProps = base.GetProperties(context, value, attributes);
                    CustomObjectType obj = (CustomObjectType)value;
                    List<CustomProperty> customProps = obj == null ? null : obj.Properties;
                    PropertyDescriptor[] props = new PropertyDescriptor[stdProps.Count + (customProps == null ? 0 : customProps.Count)];
                    stdProps.CopyTo(props, 0);
                    if (customProps != null)
                    {
                        int index = stdProps.Count;
                        foreach (CustomProperty prop in customProps)
                        {
                            props[index++] = new CustomPropertyDescriptor(prop);
                        }
                    }
                    return new PropertyDescriptorCollection(props);
                }
            }
            private class CustomPropertyDescriptor : PropertyDescriptor
            {
                private readonly CustomProperty prop;
                public CustomPropertyDescriptor(CustomProperty prop)
                    : base(prop.Name, null)
                {
                    this.prop = prop;
                }
                public override string Category { get { return prop.Category ?? "Dynamic"; } }
                public override string Description { get { return prop.Description; } }
                public override string Name { get { return prop.Name; } }
                public override bool ShouldSerializeValue(object component)
                {
                    CustomObjectType obj = (CustomObjectType)component;
                    return obj.Values.GetOrDefault(prop.Name, prop.Value) != prop.Value;
                }
                public override void ResetValue(object component)
                {
                    ((CustomObjectType)component).Values.Remove(prop.Name);
                }
                public override bool IsReadOnly { get { return false; } }
                public override Type PropertyType { get { return prop.Type; } }
                public override bool CanResetValue(object component) { return true; }
                public override Type ComponentType { get { return typeof(CustomObjectType); } }
                public override void SetValue(object component, object value)
                {
                    ((CustomObjectType)component).Values.AddOrUpdate(prop.Name, value);
                }
                public override object GetValue(object component)
                {
                    return ((CustomObjectType)component).Values.GetOrDefault(prop.Name, prop.Value);
                }
            }
        }

        public class CustomProperty
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public object Value { get; set; }
            public Type Type { get; set; }
        }

    }
}
