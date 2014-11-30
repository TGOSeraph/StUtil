using System;

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