using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Misc
{
    public enum FieldModifiers
    {
        None,
        Static,
        Constant,
        Volatile,
        StaticReadonly,
        Readonly,
        StaticVolatile
    }
}
