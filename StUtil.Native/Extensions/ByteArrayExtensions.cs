using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Byte Array's
    /// </summary>
    /// <remarks>
    /// 2013-06-27  - Initial version
    /// </remarks>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Convert a byte array to a structure of the specified type
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="data">The byte representation of the object</param>
        /// <returns>The byte array marshaled as a structure</returns>
        public static T ToStruct<T>(this byte[] data) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(data, 0, ptr, size);

            T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        /// <summary>
        /// Convert a byte array to a structure of the specified type
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="data">The byte representation of the object</param>
        /// <param name="encoding">The encoding to use to convert</param>
        /// <returns>The byte array marshaled as a structure</returns>
        public static T ToStruct<T>(this byte[] data, Encoding encoding) where T : struct
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)encoding.GetString(data);
            }
            else
            {
                return data.ToStruct<T>();
            }
        }
    }
}
