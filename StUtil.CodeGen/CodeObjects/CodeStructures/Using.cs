using StUtil.CodeGen.CodeObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Using : TypeObject
    {
        public Using(Type type)
            : base(type)
        {
        }
        public Using(string usingNamespace)
            : base(usingNamespace)
        {
        }
    }
}
