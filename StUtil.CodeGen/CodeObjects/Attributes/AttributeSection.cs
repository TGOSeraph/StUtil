using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Attributes
{
    public class AttributeSection : BaseCodeObject
    {
        public CodeObjectList<Attribute> Attributes {get; set;}
        public AttributeSection(params Attribute[] attributes)
        {
            Attributes = new CodeObjectList<Attribute>(", ");
            Attributes.Items.AddRange(attributes);
        }
    }
}
