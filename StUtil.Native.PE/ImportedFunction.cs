using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Native.PE
{
    public class ImportedFunction
    {
        public int Hint { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name + " [" + Hint + "]";
        }
    }
}
