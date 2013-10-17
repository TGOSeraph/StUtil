using StUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Debugging
{
    public static class Extensions
    {
        public static string ToString(this object obj, bool properties, bool fields)
        {
            ReflectionHelper helper = new ReflectionHelper(obj);

            string outp = helper.TargetType.Name;
            if (properties)
            {
                outp += "\nProperties:";
                foreach (ReflectedProperty prop in helper.GetProperties())
                {
                    outp += "\n\t" + prop.Member.Name + " = (" + prop.ReturnType.Name + ") " + prop.Get(obj).ToString();
                }
            }
            if (fields)
            {
                outp += "\nFields:";
                foreach (ReflectedField field in helper.GetFields())
                {
                    outp += "\n\t" + field.Member.Name + " = (" + field.ReturnType.Name + ") " + field.Get(obj).ToString();
                }
            }

            return outp;
        }
    }
}
