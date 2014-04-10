using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Reflection.Dynamic
{
    public class DynamicObjPropertyDescriptor<T> : PropertyDescriptor where T : DynamicObj
    {
        private object InitialValue = null;

        public override Type ComponentType
        {
            get { return typeof(T); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(object); }
        }

        public DynamicObjPropertyDescriptor(string name) : base(name, null) { }

        public override bool CanResetValue(object component)
        {
            return true;
        }
        public override object GetValue(object component)
        {
            return (component as T)[Name];
        }
        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            if (InitialValue == null)
            {
                InitialValue = GetValue(component);
            }
            (component as T)[Name] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            throw new NotImplementedException();
        }
    }
}
