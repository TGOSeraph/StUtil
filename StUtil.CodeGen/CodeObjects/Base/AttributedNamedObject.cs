using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    public class AttributedNamedObject : NamedObject
    {
        public CodeObjectList<AttributeSection> Attributes { get; set; }
        public AttributedNamedObject(string name, params AttributeSection[] attributes)
            :base(name)
        {
            this.Attributes = new CodeObjectList<AttributeSection>("\n", attributes);
        }
    }
}
