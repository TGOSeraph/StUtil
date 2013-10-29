using StUtil.CodeGen.CodeObjects.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    /// <summary>
    /// An object that has access modifiers, attributes and a name
    /// </summary>
    public class AccessAttributedNamedObject : AttributedNamedObject
    {
        /// <summary>
        /// The access modifiers to place on the member
        /// </summary>
        public Misc.AccessModifiers AccessModifier { get; set; }

        /// <summary>
        /// Construct a new object and set the parameters
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <param name="modifier">The access modifer of the object</param>
        /// <param name="attributes">Any attributes to place on the object</param>
        public AccessAttributedNamedObject(string name, Misc.AccessModifiers modifier, params AttributeSection[] attributes)
            : base(name, attributes)
        {
            this.AccessModifier = modifier;
        }
    }
}
