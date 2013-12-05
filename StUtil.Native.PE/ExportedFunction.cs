using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native.PE
{
    public class ExportedFunction
    {
        public int Ordinal { get; set; }
        public int Hint { get; set; }
        public uint RVA { get; set; }
        public string Name { get; set; }

        private string undecoratedName;
        public string UndecoratedName
        {
            get
            {
                if (undecoratedName == null)
                {
                    StringBuilder builder = new StringBuilder(255);
                    NativeMethods.UnDecorateSymbolName(Name, builder, builder.Capacity, NativeEnums.UnDecorateFlags.UNDNAME_COMPLETE);
                    undecoratedName = builder.ToString();
                }
                return undecoratedName;
            }
        }

        public override string ToString()
        {
            return UndecoratedName + " (" + Ordinal.ToString() + ") [" + Hint + "]";
        }
    }
}
