using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace StUtil.Native.Extensions
{
    /// <summary>
    /// Extensions for structs
    /// </summary>
    /// <remarks>
    /// 2013-06-27  - Initial version
    /// </remarks>
    public static class StructExtensions
    {
        /// <summary>
        /// Convert a struct to a byte array
        /// </summary>
        /// <typeparam name="T">The type of the struct</typeparam>
        /// <param name="obj">The struct to convert to bytes</param>
        /// <returns>The byte representation of the object</returns>
        public static byte[] ToBytes<T>(this T obj) where T : struct
        {
            int size = Marshal.SizeOf(obj   );
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }
    }
}
