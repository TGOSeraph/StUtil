using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Namespace : MemberContainerObject
    {
        public CodeObjectList<Using> Usings { get; set; }
      
        public Namespace(string name)
            :base(name)
        {
            Usings = new CodeObjectList<Using>("\n");
        }
    }
}
