using StUtil.CodeGen.CodeObjects.CodeStructures;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{   
    public class MemberContainerObject : NamedObject
    {
        public CodeObjectList<Region> Regions { get; set; }
        public CodeObjectList<Class> Classes { get; set; }
        public CodeObjectList<Event> Events { get; set; }
        public CodeObjectList<Field> Fields { get; set; }
        public CodeObjectList<Method> Methods { get; set; }

        public MemberContainerObject(string name)
            : base(name)
        {
            Regions = new CodeObjectList<Region>("\n");
            Classes = new CodeObjectList<Class>("\n");
            Fields = new CodeObjectList<Field>("\n");
            Events = new CodeObjectList<Event>("\n");
            Methods = new CodeObjectList<Method>("\n");
        }
    }
}
