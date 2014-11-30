using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Extensions
{
    public static class ProcessExtensions
    {
        public static bool Is64Bit(this Process process)
        {
            IntPtr proc = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("Kernel32.dll"), "IsWow64Process");
            IntPtr hProcess = IntPtr.Zero;
            try
            {
                if (proc == IntPtr.Zero)
                {
                    return false;
                }
                hProcess = NativeUtilities.OpenProcess(process, NativeEnums.ProcessAccess.AllAccess);
                bool retVal = false;
                if (NativeMethods.IsWow64Process(hProcess, out retVal))
                {
                    return !retVal;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                if (hProcess != IntPtr.Zero)
                {
                    NativeMethods.CloseHandle(hProcess);
                }
            }
          
        }
    }
}
