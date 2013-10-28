using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Attributes
{
    [Keyword]
    public class Attribute : NamedObject
    {
        public CodeObjectList<DataObject> Parameters { get; set; }
        public CodeObjectDictionary<string, DataObject> NamedParameters { get; set; }

        public Attribute(string name, params DataObject[] parameters)
            :base(name)
        {
            Parameters = new CodeObjectList<DataObject>(", ");
            Parameters.AddRange(parameters);
            NamedParameters = new CodeObjectDictionary<string, DataObject>(", ", "=");
        }
    }
}
