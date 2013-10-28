using StUtil.CodeGen.CodeObjects.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    public class AccessAttributedNamedObject : AttributedNamedObject
    {
        public Misc.AccessModifiers AccessModifier { get; set; }

        public AccessAttributedNamedObject(string name, Misc.AccessModifiers modifier, params AttributeSection[] attributes)
            : base(name, attributes)
        {
            this.AccessModifier = modifier;
        }
    }
}
