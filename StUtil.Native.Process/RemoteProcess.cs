using StUtil.Extensions;
using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Process
{
    public class RemoteProcess : IDisposable
    {
        private const string BOOTSTRAP_DLL = "StUtil.Native.Bootstrap.{0}.dll";
        private const string BOOTSTRAP_EXE = "StUtil.Native.Bootstrapper.{0}.exe";

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

        private string CreateFile(string file, byte[] data)
        {
            if (System.IO.File.Exists(file))
            {
                return file;
            }
            System.IO.File.WriteAllBytes(file, data);
            return file;
        }

        public IntPtr LoadDotNetModule(string path, string typeName, string method, string args)
        {
            bool x64 = Process.Is64Bit();
            if (x64 != Environment.Is64BitProcess)
            {
                string helper = null;
                //Use helper
                if (x64)
                {
                    helper = CreateFile(string.Format(BOOTSTRAP_EXE, "x64"), Properties.Resources.StUtil_Native_Bootstrapper_x64);
                }
                else
                {
                    helper = CreateFile(string.Format(BOOTSTRAP_EXE, "x86"), Properties.Resources.StUtil_Native_Bootstrapper_x86);
                }
                helper = System.IO.Path.GetFullPath(helper);
                IPC.NamedPipes.NamedPipeServer server = new IPC.NamedPipes.NamedPipeServer();
                Guid guid = Guid.NewGuid();
                IntPtr result = IntPtr.Zero;
                Exception ex = null; ;
                server.ConnectionRecieved += (s, e) =>
                {
                    IPC.IConnectionMessage msg = e.Value.SendAndReceive(new InjectionMessage
                    {
                        File = path,
                        Type = typeName,
                        Method = method,
                        Args = args,
                        ProcessId = Process.Id
                    });
                    if (msg is IPC.ValueMessage<IntPtr>)
                    {
                        result = ((IPC.ValueMessage<IntPtr>)msg).Value;
                    }
                    else
                    {
                        ex = ((IPC.ValueMessage<Exception>)msg).Value;
                    }
                };
                var p = System.Diagnostics.Process.Start(helper, guid.ToString());
                server.Start(new IPC.NamedPipes.NamedPipeInitialisation(guid.ToString()));
                if (ex != null)
                {
                    throw ex;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return LoadDotNetModuleImpl(path, typeName, method, args);
            }
        }

        private IntPtr LoadDotNetModuleImpl(string path, string typeName, string method, string args)
        {
            bool x64 = Process.Is64Bit();
            string file = null;
            //Export the bootstrapper
            if (x64)
            {
                file = CreateFile(string.Format(BOOTSTRAP_DLL, "x64"), Properties.Resources.StUtil_Native_Bootstrap_x64);
            }
            else
            {
                file = CreateFile(string.Format(BOOTSTRAP_DLL, "x86"), Properties.Resources.StUtil_Native_Bootstrap_x86);
            }

            if (BootstrapModule == null)
            {
                BootstrapModule = LoadNativeModule(file);
            }
            RemoteMemoryAllocation mPath = RemoteMemoryAllocation.Allocate(Handle, path, Encoding.Unicode);
            RemoteMemoryAllocation mTypeName = RemoteMemoryAllocation.Allocate(Handle,   typeName, Encoding.Unicode);
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

                IntPtr v = BootstrapModule.Invoke("CLRBootstrap", bootstrap);
                if (v.ToInt32() < 0)
                {
                    Marshal.ThrowExceptionForHR(v.ToInt32());
                }
                return v;
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
            Open();
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
