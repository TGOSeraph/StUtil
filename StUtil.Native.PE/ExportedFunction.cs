using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Native.PE
{
    public class ExportedFunction
    {
        public int Ordinal { get; set; }
        public int Hint { get; set; }
        public uint RVA { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name + " (" + Ordinal.ToString() + ") [" + Hint + "]";
        }
    }
}
