using StUtil.Extensions;
using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Process
{
    public class RemoteProcess : IDisposable
    {
        private const string BOOTSTRAP_DLL = "StUtil.Native.Bootstrap.{0}.dll";

        public int Id
        {
            get
            {
                return Process.Id;
            }
        }

        public bool IsHandleCreated
        {
            get
            {
                return Handle != IntPtr.Zero;
            }
        }

        public IntPtr Handle { get; private set; }
        public System.Diagnostics.Process Process { get; private set; }

        public RemoteModule BootstrapModule { get; private set; }

        public RemoteProcess(System.Diagnostics.Process process)
        {
            this.Process = process;
        }

        public void Open()
        {
            if (IsHandleCreated)
            {
                return;
            }
            Handle = NativeMethods.OpenProcess((uint)NativeEnums.ProcessAccess.AllAccess, false, (uint)Id);
        }

        public void Dispose()
        {
            if (IsHandleCreated)
            {
                NativeMethods.CloseHandle(Handle);
                Handle = IntPtr.Zero;
                Process.Dispose();
            }
        }

        public void LoadDotNetModule(string path, string typeName, string method, string args)
        {
            bool x64 = Process.Is64Bit();

            string file = string.Format(BOOTSTRAP_DLL, x64 ? "x64" : "x86");

            if (!System.IO.File.Exists(file))
            {
                System.IO.File.WriteAllBytes(file, x64 ? Properties.Resources.StUtil_Native_Bootstrap_x64 : Properties.Resources.StUtil_Native_Bootstrap_x86);
            }

            if (BootstrapModule == null)
            {
                BootstrapModule = LoadNativeModule(file);
            }

            RemoteMemoryAllocation mPath = RemoteMemoryAllocation.Allocate(Handle, path, Encoding.Unicode);
            RemoteMemoryAllocation mTypeName = RemoteMemoryAllocation.Allocate(Handle, typeName, Encoding.Unicode);
            RemoteMemoryAllocation mMethod = RemoteMemoryAllocation.Allocate(Handle, method, Encoding.Unicode);
            RemoteMemoryAllocation mArgs = RemoteMemoryAllocation.Allocate(Handle, args, Encoding.Unicode);
            try
            {
                CLRINITARGS bootstrap = new CLRINITARGS()
                {
                    args = mArgs.MemoryAddress,
                    dll = mPath.MemoryAddress,
                    method = mMethod.MemoryAddress,
                    typeName = mTypeName.MemoryAddress
                };

                BootstrapModule.Invoke("CLRBootstrap", bootstrap);
            }
            finally
            {
                mPath.Dispose();
                mTypeName.Dispose();
                mMethod.Dispose();
                mArgs.Dispose();
            }
        }

        public RemoteModule LoadNativeModule(string path)
        {
            path = StUtil.IO.Utilities.NormalizePath(path);
            var k32 = GetModules().First(m => m.Module.ModuleName.Equals("kernel32.dll", StringComparison.InvariantCultureIgnoreCase));
            IntPtr hMod = k32.Invoke("LoadLibraryA", path, Encoding.ASCII);
            return GetModules().First(m => StUtil.IO.Utilities.NormalizePath(m.Module.FileName) == path);
        }

        public IEnumerable<RemoteModule> GetModules()
        {
            int pid = Process.Id;
            var mods = Process.Modules.AsEnumerable<ProcessModule>().Select(m => new RemoteModule(this, m));
            Process.Dispose();
            Process = System.Diagnostics.Process.GetProcessById(pid);
            return mods;
        }
    }
}
