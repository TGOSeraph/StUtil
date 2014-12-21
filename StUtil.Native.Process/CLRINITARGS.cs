using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Process
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CLRINITARGS
    {
        public IntPtr dll;
        public IntPtr typeName;
        public IntPtr method;
        public IntPtr args;
    };
}
