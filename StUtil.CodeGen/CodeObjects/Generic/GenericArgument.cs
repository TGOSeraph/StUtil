using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Generic
{
    [Keyword]
    public class GenericArgument : BaseCodeObject
    {
        public string Identifier { get; set; }

        public GenericArgument(string arg)
        {
            this.Identifier = arg;
        }

        public static implicit operator string(GenericArgument d)
        {
            return d.Identifier;
        }
        public static implicit operator GenericArgument(string d)
        {
            return new GenericArgument(d);
        }
    }
}
