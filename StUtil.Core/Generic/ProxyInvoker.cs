using System;
using System.Reflection;

namespace StUtil.Generic
{
    public abstract class ProxyInvoker
    {
        public abstract bool ShouldProxy(MethodInfo method);

        public abstract object Invoke(MethodInfo method, object target, object[] args, Type returnType);

        protected static object GetDefaultValue(Type type)
        {
            if (type == typeof(void)) return null;

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}