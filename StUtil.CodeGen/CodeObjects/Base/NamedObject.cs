using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{
    [Keyword]
    public class NamedObject : BaseCodeObject
    {
        public string Name { get; set; }

        public NamedObject(string name)
        {
            this.Name = name;
        }
    }
}
