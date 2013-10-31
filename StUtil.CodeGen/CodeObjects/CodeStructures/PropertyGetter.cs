using StUtil.CodeGen.CodeObjects.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class PropertyGetter : BaseCodeObject
    {
        public BaseSyntaxObject Code { get; set; }

        public PropertyGetter()
        {
            Code = new TextSyntax("get;");
        }
    }
}
