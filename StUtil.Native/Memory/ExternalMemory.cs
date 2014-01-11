using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using StUtil.Extensions;
using StUtil.Native.Extensions;

namespace StUtil.Native.Memory
{
    public class ExternalMemory : IDisposable
    {
        public ExternalProcess Process { get; private set; }
        public IntPtr Address { get; private set; }
        public uint Size { get; private set; }

        public bool WasAllocated { get; private set; }

        public ExternalMemory(ExternalProcess process, uint allocateSize)
            : this(process, IntPtr.Zero, allocateSize)
        {
            this.Address = Allocate(allocateSize);
        }

        public ExternalMemory(ExternalProcess process, IntPtr address, uint size)
        {
            this.Process = process;
            if (address != IntPtr.Zero)
            {
                this.Address = address;
            }
            this.Size = size;
        }

        private IntPtr Allocate(uint allocateSize)
        {
            IntPtr lpBaseAddress = NativeMethods.VirtualAllocEx(Process.hProcess, IntPtr.Zero, allocateSize, NativeEnums.AllocationType.Commit | NativeEnums.AllocationType.Reserve, NativeEnums.MemoryProtection.ReadWrite);
            if (lpBaseAddress == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            this.WasAllocated = true;
            return lpBaseAddress;
        }

        public int Write(byte[] data, int bufferOffset, uint bufferLength, int pointerOffset)
        {
            if (bufferOffset > 0)
            {
                data = data.Skip(bufferOffset).ToArray();
            }
            IntPtr address = Address;
            if (pointerOffset > 0)
            {
                address = address.Increment(pointerOffset);
            }

            if (bufferLength > Size)
            {
                throw new ArgumentException("Data to write is larger than the specified section of memory");
            }

            UIntPtr dwWritten = UIntPtr.Zero;
            if (!NativeMethods.WriteProcessMemory(Process.hProcess, address, data, (uint)bufferLength, out dwWritten))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            if ((int)dwWritten != data.Length)
            {
                throw new Exception("Not all data copied from buffer");
            }

            return (int)dwWritten;
        }

        public int Write(byte[] data)
        {
            return Write(data, 0, (uint)data.Length, 0);
        }

        public int Write(byte[] data, int pointerOffset)
        {
            return Write(data, 0, (uint)data.Length, pointerOffset);
        }

        public int Write(byte[] data, int bufferOffset, uint bufferLength)
        {
            return Write(data, bufferOffset, bufferLength, 0);
        }

        public int Write(string text, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }
            byte[] data = encoding.GetBytes(text);
            return Write(data);
        }

        public int Write<T>(T obj) where T : struct
        {
            return Write(obj.ToBytes());
        }

        public byte[] Read(uint readLength, int pointerOffset)
        {
            int dwRead = 0;
            byte[] lpBuffer = new byte[readLength];
            if (!NativeMethods.ReadProcessMemory(Process.hProcess, Address.Increment(pointerOffset), lpBuffer, (int)readLength, out dwRead))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            if (dwRead != readLength)
            {
                throw new Exception("Not all data read from memory");
            }
            return lpBuffer;
        }

        public byte[] Read(int pointerOffset)
        {
            return Read(Size, pointerOffset);
        }

        public byte[] Read()
        {
            return Read(Size, 0);
        }

        public string Read(uint readLength, int pointerOffset, Encoding encoding)
        {
            return encoding.GetString(Read(readLength, pointerOffset));
        }

        public string Read(int pointerOffset, Encoding encoding)
        {
            return encoding.GetString(Read(Size, pointerOffset));
        }

        public string Read(Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Unicode;
            return encoding.GetString(Read(Size, 0));
        }

        public T Read<T>(uint readLength, int pointerOffset) where T : struct
        {
            return Read(readLength, pointerOffset).ToStruct<T>();
        }

        public T Read<T>(int pointerOffset) where T : struct
        {
            return Read(Size, pointerOffset).ToStruct<T>();
        }

        public T Read<T>() where T : struct
        {
            return Read(Size, 0).ToStruct<T>();
        }

        public void Dispose()
        {
            if (this.WasAllocated)
            {
                NativeMethods.VirtualFreeEx(Process.hProcess, Address, Size, NativeEnums.FreeType.Release);
            }
        }
    }
}
