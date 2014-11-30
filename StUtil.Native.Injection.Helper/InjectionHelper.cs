using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Injection.Helper
{
    internal static class InjectionHelper
    {
        private delegate bool InjectPtr(int pid, IntPtr hProcess, [MarshalAs(UnmanagedType.LPWStr)]string dll, [Out]out int result);

        private delegate bool InjectDotNetAssemblyPtr(int pid, IntPtr hProcess,
            [MarshalAs(UnmanagedType.LPWStr)]string dll,
            [MarshalAs(UnmanagedType.LPWStr)]string typeName,
            [MarshalAs(UnmanagedType.LPWStr)]string method,
            [MarshalAs(UnmanagedType.LPWStr)]string args,
            [Out]out int result);

        private static InjectDotNetAssemblyPtr InjectDotNetAssembly;
        private static InjectPtr Inject;

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [Flags]
        public enum ProcessAccess
        {
            /// <summary>
            /// Required to create a thread.
            /// </summary>
            CreateThread = 0x0002,

            /// <summary>
            ///
            /// </summary>
            SetSessionId = 0x0004,

            /// <summary>
            /// Required to perform an operation on the address space of a process
            /// </summary>
            VmOperation = 0x0008,

            /// <summary>
            /// Required to read memory in a process using ReadProcessMemory.
            /// </summary>
            VmRead = 0x0010,

            /// <summary>
            /// Required to write to memory in a process using WriteProcessMemory.
            /// </summary>
            VmWrite = 0x0020,

            /// <summary>
            /// Required to duplicate a handle using DuplicateHandle.
            /// </summary>
            DupHandle = 0x0040,

            /// <summary>
            /// Required to create a process.
            /// </summary>
            CreateProcess = 0x0080,

            /// <summary>
            /// Required to set memory limits using SetProcessWorkingSetSize.
            /// </summary>
            SetQuota = 0x0100,

            /// <summary>
            /// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
            /// </summary>
            SetInformation = 0x0200,

            /// <summary>
            /// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
            /// </summary>
            QueryInformation = 0x0400,

            /// <summary>
            /// Required to suspend or resume a process.
            /// </summary>
            SuspendResume = 0x0800,

            /// <summary>
            /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
            /// A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION.
            /// </summary>
            QueryLimitedInformation = 0x1000,

            /// <summary>
            /// Required to wait for the process to terminate using the wait functions.
            /// </summary>
            Synchronize = 0x100000,

            /// <summary>
            /// Required to delete the object.
            /// </summary>
            Delete = 0x00010000,

            /// <summary>
            /// Required to read information in the security descriptor for the object, not including the information in the SACL.
            /// To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            /// </summary>
            ReadControl = 0x00020000,

            /// <summary>
            /// Required to modify the DACL in the security descriptor for the object.
            /// </summary>
            WriteDac = 0x00040000,

            /// <summary>
            /// Required to change the owner in the security descriptor for the object.
            /// </summary>
            WriteOwner = 0x00080000,

            StandardRightsRequired = 0x000F0000,

            /// <summary>
            /// All possible access rights for a process object.
            /// </summary>
            AllAccess = StandardRightsRequired | Synchronize | 0xFFFF
        }

        static InjectionHelper()
        {
            try
            {
                string path = (Environment.Is64BitProcess ? "x64" : "x86") + "\\StUtil.Native.Injection.dll";
                path = System.IO.Path.GetFullPath(path);
                IntPtr hModule = LoadLibrary(path);
                if (hModule == IntPtr.Zero)
                {
                    throw new System.ComponentModel.Win32Exception();
                }
                IntPtr hInjectDotNet = GetProcAddress(hModule, "InjectDotNetAssembly");
                InjectDotNetAssembly = Marshal.GetDelegateForFunctionPointer<InjectDotNetAssemblyPtr>(hInjectDotNet);

                IntPtr hInject = GetProcAddress(hModule, "Inject");
                Inject = Marshal.GetDelegateForFunctionPointer<InjectPtr>(hInject);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Environment.ExitCode = -1;
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                IntPtr hProcess = OpenProcess((uint)ProcessAccess.AllAccess, false, uint.Parse(args[1]));
                if (hProcess == IntPtr.Zero)
                {
                    throw new ApplicationException("Invalid handle");
                }
                try
                {
                    string dll = args[2];
                    if (args.Length != 3)
                    {
                        string typeName = args[3];
                        string method = args[4];
                        string arg = args[5];
                        int res = 0;
                        bool success = InjectDotNetAssembly(int.Parse(args[1]), hProcess, dll, typeName, method, arg, out res);
                        int mask = success ? 1 : 0;

                        mask |= res << 16;
                        MessageBox.Show("Mask = " + mask.ToString() + " Success = " + success.ToString() + " Result = " + res.ToString());
                        Environment.Exit(mask);
                    }
                    else
                    {
                        int res = 0;
                        int mask = Inject(int.Parse(args[1]), hProcess, dll, out res) ? 1 : 0;
                        mask |= res << 16;
                        Environment.Exit(mask);
                    }
                }
                finally
                {
                    CloseHandle(hProcess);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
    }
}