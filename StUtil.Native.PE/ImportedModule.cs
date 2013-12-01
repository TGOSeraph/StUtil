using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Native.PE
{
    public class ImportedModule
    {
        public string Name { get; set; }
        public List<ImportedFunction> Functions { get; set; }

        public override string ToString()
        {
            return Name + " (" + Functions.Count + ")";
        }
    }
}
