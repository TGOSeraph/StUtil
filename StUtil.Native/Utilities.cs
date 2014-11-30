using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native
{
    public static class Utilities
    {
        /// <summary>
        /// Gets a value indicating wether or not the currently running application is x64 or x86
        /// </summary>
        /// <returns>A value indicating wether or not the currently running application is x64 or x86</returns>
        public static bool Is64BitApplication()
        {
            return IntPtr.Size == 8;
        }
    }
}
