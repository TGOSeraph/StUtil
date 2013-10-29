using StUtil.CodeGen.CodeObjects.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    /// <summary>
    /// An object with a name and access modifiers
    /// </summary>
    public class AccessNamedObject : NamedObject
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
        public AccessNamedObject(string name, Misc.AccessModifiers modifier)
            : base(name)
        {
            this.AccessModifier = modifier;
        }
    }
}
