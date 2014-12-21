using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;

namespace StUtil.Native
{
    public class RemoteMemoryAllocation : IDisposable
    {
        public IntPtr ProcessHandle { get; private set; }
        public IntPtr MemoryAddress { get; private set; }
        public int Size { get; private set; }

        public RemoteMemoryAllocation(IntPtr hProcess, IntPtr lpAddress, int size)
        {
            this.ProcessHandle = hProcess;
            this.MemoryAddress = lpAddress;
            this.Size = size;
        }

        public T Read<T>(Encoding encoding)
        {
            byte[] buffer = new byte[Size];
            IntPtr read;
            NativeMethods.ReadProcessMemory(ProcessHandle, MemoryAddress, buffer, Size, out read);
            return buffer.ToStruct<T>(encoding);
        }

        public T Read<T>()
        {
            byte[] buffer = new byte[Size];
            IntPtr read;
            NativeMethods.ReadProcessMemory(ProcessHandle, MemoryAddress, buffer, Size, out read);
            return buffer.ToStruct<T>();
        }

        public static RemoteMemoryAllocation Allocate(IntPtr hProcess, byte[] data)
        {
            IntPtr lpAddress = NativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, new IntPtr(data.Length), (uint)(NativeEnums.AllocationType.Commit | NativeEnums.AllocationType.Reserve), (uint)NativeEnums.MemoryProtection.ReadWrite);
            IntPtr written;
            NativeMethods.WriteProcessMemory(hProcess, lpAddress, data, new IntPtr(data.Length), out written);
            return new RemoteMemoryAllocation(hProcess, lpAddress, data.Length);
        }
        public static RemoteMemoryAllocation Allocate<T>(IntPtr hProcess, T data) where T : struct
        {
            IntPtr hMem = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr<T>(data, hMem, false);
            byte[] buffer = new byte[Marshal.SizeOf<T>()];
            Marshal.Copy(hMem, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(hMem);
            return Allocate(hProcess, buffer);
        }
        public static RemoteMemoryAllocation Allocate(IntPtr hProcess, string data)
        {
            return Allocate(hProcess, data, Encoding.Unicode);
        }
        public static RemoteMemoryAllocation Allocate(IntPtr hProcess, string data, System.Text.Encoding encoding)
        {
            return Allocate(hProcess, encoding.GetBytes(data));
        }

        public void Dispose()
        {
            if (MemoryAddress != IntPtr.Zero)
            {
                NativeMethods.VirtualFreeEx(ProcessHandle, MemoryAddress, new IntPtr(Size), (uint)NativeEnums.FreeType.Release);
                MemoryAddress = IntPtr.Zero;
            }
        }
    }
}
