using StUtil.Native.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace StUtil.Native
{
    public class ExternalProcess : IDisposable
    {
        public Process TargetProcess { get; private set; }
        public IntPtr hProcess { get; set; }
        public bool IsOpen
        {
            get
            {
                return hProcess != IntPtr.Zero;
            }
        }

        public ExternalProcess(Process target)
        {
            this.TargetProcess = target;
        }

        public ExternalProcess(int processID)
        {
            this.TargetProcess = Process.GetProcessById(processID);
        }

        public ExternalProcess(IntPtr hWnd)
        {
            IntPtr pid;
            Internal.Native.NativeMethods.GetWindowThreadProcessId(hWnd, out pid);
            this.TargetProcess = Process.GetProcessById(pid.ToInt32());
        }

        public bool Open()
        {
            this.hProcess = Internal.Native.NativeMethods.OpenProcess(
                Internal.Native.NativeEnums.ProcessAccess.VMRead |
                Internal.Native.NativeEnums.ProcessAccess.VMWrite |
                Internal.Native.NativeEnums.ProcessAccess.VMOperation,
                false, (uint)this.TargetProcess.Id);

            if(this.hProcess == IntPtr.Zero)
                throw new Win32Exception();

            return this.IsOpen;
        }

        public void Close()
        {
            if (this.IsOpen)
            {
                Internal.Native.NativeMethods.CloseHandle(this.hProcess);
            }
        }

        public ExternalMemory Allocate(uint size)
        {
            return new ExternalMemory(this, size);
        }

        public ExternalMemory Allocate<T>(T obj) where T: struct
        {
            ExternalMemory mem = new ExternalMemory(this, (uint)Marshal.SizeOf(obj));
            mem.Write(obj);
            return mem;
        }

        public ExternalMemory Get(IntPtr address, uint size)
        {
            return new ExternalMemory(this, address, size);
        }

        public void Dispose()
        {
            Close();
        }
    }
}
