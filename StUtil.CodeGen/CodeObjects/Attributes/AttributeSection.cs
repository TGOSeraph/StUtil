using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Attributes
{
    /// <summary>
    /// Object representing a group of attributes in a single section
    /// </summary>
    public class AttributeSection : BaseCodeObject
    {
        /// <summary>
        /// The attributes in the section
        /// </summary>
        public CodeObjectList<Attribute> Attributes {get; set;}
        /// <summary>
        /// Create a new attribute section
        /// </summary>
        /// <param name="attributes">The attributes in the section</param>
        public AttributeSection(params Attribute[] attributes)
        {
            Attributes = new CodeObjectList<Attribute>(", ");
            Attributes.Items.AddRange(attributes);
        }
    }
}
