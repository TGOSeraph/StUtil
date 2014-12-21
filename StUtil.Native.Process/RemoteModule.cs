using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StUtil.Extensions;

namespace StUtil.Native.Process
{
    public class RemoteModule
    {
        public IntPtr BaseAddress
        {
            get
            {
                return Module.BaseAddress;
            }
        }
        public ProcessModule Module { get; private set; }
        public RemoteProcess Process { get; private set; }

        public RemoteModule(RemoteProcess process, ProcessModule module)
        {
            this.Process = process;
            this.Module = module;
        }

        public bool Unload()
        {
            return Process
                .GetModules()
                .First(m => m.Module.FileName.IndexOf("kernel32", StringComparison.InvariantCultureIgnoreCase) > -1)
                .Invoke("FreeLibrary", RemoteMemoryAllocation.Allocate(Process.Handle, BitConverter.GetBytes(BaseAddress.ToInt32()))) != IntPtr.Zero;
        }

        public IntPtr Invoke(IntPtr method, IntPtr args)
        {
            IntPtr threadId = IntPtr.Zero;
            IntPtr hThread = NativeMethods.CreateRemoteThread(Process.Handle, IntPtr.Zero, IntPtr.Zero, method, args, 0, threadId);
            if (hThread == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            NativeMethods.WaitForSingleObject(hThread, unchecked((uint)-1));
            IntPtr exitCode;
            NativeMethods.GetExitCodeThread(hThread, out exitCode);
            NativeMethods.CloseHandle(hThread);
            return exitCode;
        }

        public IntPtr Invoke(string method, IntPtr args)
        {
            return Invoke(GetProcAddress(method), args);
        }

        private IntPtr GetProcAddress(string method)
        {
            IntPtr hMod = NativeMethods.LoadLibrary(Module.FileName);
            IntPtr lpFn = NativeMethods.GetProcAddress(hMod, method);
            IntPtr offset = lpFn.Decrement(hMod);
            NativeMethods.FreeLibrary(hMod);
            return offset.Increment(hMod);
        }

        public IntPtr Invoke(string method, RemoteMemoryAllocation args)
        {
            return Invoke(method, args.MemoryAddress);
        }

        public IntPtr Invoke(string method, byte[] args)
        {
            using (RemoteMemoryAllocation mem = RemoteMemoryAllocation.Allocate(Process.Handle, args))
            {
                return Invoke(method, mem);
            }
        }

        public IntPtr Invoke(string method, string args)
        {
            return Invoke(method, args, Encoding.Unicode);
        }

        public IntPtr Invoke(string method, string args, Encoding encoding)
        {
            return Invoke(method, encoding.GetBytes(args));
        }

        public IntPtr Invoke<T>(string method, T args) where T : struct
        {
            return Invoke(method, Utilities.StructToBytes(args));
        }

        public IntPtr Invoke(IntPtr method, RemoteMemoryAllocation args)
        {
            return Invoke(method, args.MemoryAddress);
        }

        public IntPtr Invoke(IntPtr method, byte[] args)
        {
            using (RemoteMemoryAllocation mem = RemoteMemoryAllocation.Allocate(Process.Handle, args))
            {
                return Invoke(method, mem);
            }
        }

        public IntPtr Invoke(IntPtr method, string args)
        {
            return Invoke(method, args, Encoding.Unicode);
        }

        public IntPtr Invoke(IntPtr method, string args, Encoding encoding)
        {
            return Invoke(method, encoding.GetBytes(args));
        }

        public IntPtr Invoke<T>(IntPtr method, T args) where T : struct
        {
            return Invoke(method, Utilities.StructToBytes(args));
        }

        public override string ToString()
        {
            return Module.ToString();
        }
    }
}
