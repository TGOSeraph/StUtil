using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Generic
{
    public class GenericConstraint : BaseCodeObject
    {
        public GenericArgument GenericArgument { get; set; }
        public CodeObjectList<TypeObject> TypeConstraints { get; set; }

        public GenericConstraint(GenericArgument arg, params TypeObject[] constraints)
        {
            this.GenericArgument = arg;
            this.TypeConstraints = new CodeObjectList<TypeObject>(", ", constraints);
        }
    }
}
