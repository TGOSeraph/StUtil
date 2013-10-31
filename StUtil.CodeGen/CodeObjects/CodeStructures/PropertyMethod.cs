using StUtil.CodeGen.CodeObjects.Misc;
using StUtil.CodeGen.CodeObjects.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class PropertyMethod : BaseCodeObject
    {
        public BaseSyntaxObject Code { get; set; }
        public AccessModifiers AccessModifier { get; set; }
        public PropertyMethod()
        {
        }
    }
}
