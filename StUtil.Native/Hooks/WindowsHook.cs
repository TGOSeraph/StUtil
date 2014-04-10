using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hooks
{
    public abstract class WindowsHook
    {
        public enum HookType
        {
            Keyboard,
            Mouse
        }

        protected HookMethod hooker;
        protected HookType type;
        protected StUtil.Internal.Native.NativeUtils.HookProc hookProc;

        public bool IsHookSet { get; private set; }

        public WindowsHook(HookMethod hooker, HookType type)
        {
            this.hooker = hooker;
            this.type = type;
        }

        protected abstract bool ProcessEvent(int wParam, IntPtr lParam);

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected int HookCallback(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                if (ProcessEvent(wParam, lParam))
                {
                    return -1;
                }
            }

            return StUtil.Internal.Native.NativeMethods.CallNextHookEx(hooker.hHook, nCode, wParam, lParam);
        }

        public void SetHook()
        {
            hookProc = new Internal.Native.NativeUtils.HookProc(HookCallback);
            hooker.SetHook(hooker.GetHookCode(type), hookProc);
            IsHookSet = true;
        }

        public void RemoveHook()
        {
            hooker.RemoveHook();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                RemoveHook();
            }
            else
            {
                hooker.RemoveHook();
            }
        }

        ~WindowsHook()
        {
            Dispose(false);
        }
    }
}
