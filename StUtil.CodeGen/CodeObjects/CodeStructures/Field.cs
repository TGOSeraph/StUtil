using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Field : AccessAttributedNamedObject
    {
        public TypeObject ReturnType { get; set; }
        public FieldModifiers Modifier { get; set; }
        public DataObject Value { get; set; }
        public Field(string name, TypeObject returnType, AccessModifiers accessModifiers, params AttributeSection[] attributes)
            : base(name, accessModifiers, attributes)
        {
            this.ReturnType = returnType;
        }
    }
}
