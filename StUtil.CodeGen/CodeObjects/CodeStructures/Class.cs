using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Generic;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Class : MemberContainerObject
    {
        public AccessModifiers AccessModifier { get; set; }
        public CodeObjectList<AttributeSection> Attributes { get; set; }
        public ClassModifiers Modifier { get; set; }
        public CodeObjectList<GenericArgument> GenericArguments { get; set; }
        public CodeObjectList<TypeObject> Inherits { get; set; }

        public CodeObjectList<GenericConstraint> GenericConstraints { get; set; }

        public Class(string name)
            : base(name)
        {
            this.Attributes = new CodeObjectList<AttributeSection>("\n");
            this.GenericArguments = new CodeObjectList<GenericArgument>(", ");
            this.GenericConstraints = new CodeObjectList<GenericConstraint>(", ");
            this.Inherits = new CodeObjectList<TypeObject>(", ");
            this.Fields = new CodeObjectList<Field>("\n");
        }
    }
}
