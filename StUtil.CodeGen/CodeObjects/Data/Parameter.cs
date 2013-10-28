using StUtil.CodeGen.CodeObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Data
{
    public class Parameter : NamedObject
    {
        public TypeObject Type { get; set; }
        public DataObject Default { get; set; }

        public Parameter(string name, TypeObject type)
            : base(name)
        {
            this.Type = type;
        }
    }
}
