using StUtil.CodeGen.CodeObjects.Base;
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
    public class Method : AccessAttributedNamedObject
    {
        public TypeObject ReturnType { get; set; }
        public MethodModifiers Modifier { get; set; }
        public CodeObjectList<Parameter> Parameters { get; set; }
        public CodeObjectList<GenericArgument> GenericArguments { get; set; }
        public CodeObjectList<GenericConstraint> GenericConstraints { get; set; }
        public CodeObjectList<TypeObject> Implements { get; set; }

        public Method(string name, TypeObject returnType, AccessModifiers accessModifiers, params Parameter[] parameters)
            : base(name, accessModifiers)
        {
            this.ReturnType = returnType;
            Parameters = new CodeObjectList<Parameter>(", ", parameters);
        }
    }
}
