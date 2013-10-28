using StUtil.CodeGen.CodeObjects.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    public class AccessNamedObject : NamedObject
    {
        public Misc.AccessModifiers AccessModifier { get; set; }

        public AccessNamedObject(string name, Misc.AccessModifiers modifier)
            : base(name)
        {
            this.AccessModifier = modifier;
        }
    }
}
