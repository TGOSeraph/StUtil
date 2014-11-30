using StUtil.Extensions;
using StUtil.Native.Internal;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StUtil.Native.Injection
{
    public static class Injector
    {
        [DllImport(@"StUtil.Native.Injection.dll", SetLastError = true)]
        private static extern bool Inject(int pid, IntPtr hProcess, [MarshalAs(UnmanagedType.LPWStr)]string dll, [Out]out int result);

        [DllImport(@"StUtil.Native.Injection.dll", SetLastError = true)]
        private static extern bool InjectDotNetAssembly(
            int pid,
            IntPtr hProcess,
            [MarshalAs(UnmanagedType.LPWStr)]string dll,
            [MarshalAs(UnmanagedType.LPWStr)]string typeName,
            [MarshalAs(UnmanagedType.LPWStr)]string method,
            [MarshalAs(UnmanagedType.LPWStr)]string args,
            [Out]out int result);

        static Injector()
        {
            //Load the correct injection library
            NativeMethods.LoadLibrary((Environment.Is64BitProcess ? "x64" : "x86") + "\\StUtil.Native.Injection.dll");
        }

        private static bool RunHelper(string args, out int result)
        {
            string path = (Environment.Is64BitProcess ? "x86" : "x64") + "\\StUtil.Native.Injection.Helper.exe";
            //Start the helper for the platform the current process is not running as
            Process p = Process.Start(path, args);
            p.WaitForExit();
            int mask = p.ExitCode;
            result = mask >> 16;
            return (mask & 0xFFFF) == 1;
        }

        private static bool InjectUsingHelper(IntPtr hProcess, string dll, out int result)
        {
            return RunHelper("\"" + hProcess.ToString() + "\" \"" + dll.ToString() + "\"", out result);
        }

        private static bool InjectUsingHelper(IntPtr hProcess, string dll, string typeName, string method, string args, out int result)
        {
            return RunHelper("\"" + hProcess.ToString() + "\" \"" + dll.ToString() + "\" \"" + typeName.ToString() + "\" \"" + method.ToString() + "\" \"" + args.ToString(), out result);
        }

        public static int Inject(string dll, Process process)
        {
            IntPtr hProcess = NativeUtilities.OpenProcess(process, NativeEnums.ProcessAccess.AllAccess);
            int val = 0;

            bool current64 = Environment.Is64BitProcess;
            bool target64 = process.Is64Bit();

            if (current64 != target64)
            {
                if (!InjectUsingHelper(hProcess, dll, out val))
                {
                    throw new Win32Exception(val);
                }
            }
            else
            {
                if (!Inject(process.Id, hProcess, dll, out val))
                {
                    throw new Win32Exception(val);
                }
            }

            NativeMethods.CloseHandle(hProcess);

            return val;
        }

        public static int Inject(Process process, Func<string, int> function, params string[] args)
        {
            IntPtr hProcess = IntPtr.Zero;
            try
            {
                if (!function.Method.IsStatic)
                {
                    throw new ArgumentException("The method to run must be static.");
                }
                if (function.Method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length > 0)
                {
                    throw new ArgumentException("The method to run must not be an anonymous method.");
                }

                int val = 0;
                bool success = false;

                bool current64 = Environment.Is64BitProcess;
                bool target64 = process.Is64Bit();

                if (current64 != target64)
                {
                    success = InjectUsingHelper(new IntPtr(process.Id),
                        function.Method.DeclaringType.Assembly.Location,
                        function.Method.DeclaringType.FullName,
                        function.Method.Name,
                        args == null ? "" : string.Join(";", args),
                        out val);
                }
                else
                {
                    hProcess = NativeUtilities.OpenProcess(process, NativeEnums.ProcessAccess.AllAccess);
                    success = InjectDotNetAssembly(process.Id, hProcess,
                        function.Method.DeclaringType.Assembly.Location,
                        function.Method.DeclaringType.FullName,
                        function.Method.Name,
                        args == null ? "" : string.Join(";", args),
                        out val);
                }

                if (!success)
                {
                    if (val < 0)
                    {
                        byte[] buffer = new byte[sizeof(int)];
                        IntPtr read;
                        NativeMethods.ReadProcessMemory(hProcess, new IntPtr(-val), buffer, buffer.Length, out read);
                        int length = BitConverter.ToInt32(buffer, 0);
                        byte[] message = new byte[length];
                        NativeMethods.ReadProcessMemory(hProcess, new IntPtr(-val + buffer.Length), message, message.Length, out read);

                        throw new InjectionException(System.Text.Encoding.Unicode.GetString(message));
                    }

                    if (val != 1)
                    {
                        throw new Win32Exception(val);
                    }
                }
                return val;
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