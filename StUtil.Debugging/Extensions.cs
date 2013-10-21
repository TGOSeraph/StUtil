using StUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Debugging
{
    /// <summary>
    /// Extensions to aid in testing and debugging
    /// </summary>
    /// <remarks>
    /// 2013-10-17  - Initial version
    /// </remarks>
    public static class Extensions
    {
        /// <summary>
        /// Converts an object to string listing its properties and/or fields
        /// </summary>
        /// <param name="obj">The object to stringify</param>
        /// <param name="properties">If properties should be output</param>
        /// <param name="fields">If fields should be output</param>
        /// <returns></returns>
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
