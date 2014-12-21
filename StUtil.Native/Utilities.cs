using System;
using System.Runtime.InteropServices;

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

        public static byte[] StructToBytes<T>(T obj) where T : struct
        {
            IntPtr hMem = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr<T>(obj, hMem, false);
            byte[] buffer = new byte[Marshal.SizeOf<T>()];
            Marshal.Copy(hMem, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(hMem);
            return buffer;
        }
    }
}