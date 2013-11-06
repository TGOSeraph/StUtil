using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Property : AccessAttributedNamedObject
    {
        public TypeObject ReturnType { get; set; }
        public PropertyGetter Getter { get; set; }
        public PropertySetter Setter { get; set; }
        public MethodModifiers Modifier { get; set; }

        public Property(string name, TypeObject type, AccessModifiers access)
            : base(name, access)
        {
            this.ReturnType = type;
            this.Getter = new PropertyGetter();
            this.Setter = new PropertySetter();
        }
    }
}
