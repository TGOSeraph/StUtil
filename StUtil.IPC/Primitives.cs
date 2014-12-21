using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IPC
{
    public static class Primitives
    {
        public static Type NULL
        {
            get
            {
                return typeof(NullObject);
            }
        }

        public static bool IsNull(Type t)
        {
            return t == NULL;
        }

        public struct NullObject { }
    }
}
