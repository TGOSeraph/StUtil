using StUtil.CodeGen.CodeObjects.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class PropertySetter : BaseCodeObject
    {
        public BaseSyntaxObject Code { get; set; }
        public PropertySetter()
        {
            Code = new TextSyntax("set;");
        }
    }
}
