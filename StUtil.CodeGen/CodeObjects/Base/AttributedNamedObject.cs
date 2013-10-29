using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    /// <summary>
    /// An object that can have attributes
    /// </summary>
    public class AttributedNamedObject : NamedObject
    {
        /// <summary>
        /// The attributes to place on the object
        /// </summary>
        public CodeObjectList<AttributeSection> Attributes { get; set; }

        /// <summary>
        /// Create a new attributed named object
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <param name="attributes">Any attributes to place on the object</param>
        public AttributedNamedObject(string name, params AttributeSection[] attributes)
            :base(name)
        {
            this.Attributes = new CodeObjectList<AttributeSection>("\n", attributes);
        }
    }
}
