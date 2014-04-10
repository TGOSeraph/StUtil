using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Reflection.Dynamic
{
    public abstract class DynamicObj : DynamicObject, ICustomTypeDescriptor
    {
        protected abstract object GetValue(string name);
        protected abstract void SetValue(string name, object value);
        protected abstract string[] Properties { get; }

        public object this[string name]
        {
            get
            {
                return GetValue(name);
            }
            set
            {
                SetValue(name, value);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = GetValue(binder.Name);
                return true;
            }
            catch (Exception)
            {
                return base.TryGetMember(binder, out result);
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Properties;
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return null;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        public EventDescriptorCollection GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(Properties.Select(key => new DynamicObjPropertyDescriptor<DynamicObj>(key)).ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion
    }
}
