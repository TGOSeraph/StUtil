using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hook
{
    public abstract class WindowsHook
    {
        public enum HookType
        {
            Keyboard,
            Mouse,
            Message
        }

        protected HookMethod hooker;
        protected HookType type;
        protected NativeCallbacks.HookProc hookProc;

        public bool IsHookSet { get; private set; }

        public WindowsHook(HookMethod hooker, HookType type)
        {
            this.hooker = hooker;
            this.type = type;
        }

        protected abstract bool ProcessEvent(IntPtr wParam, IntPtr lParam);

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected int HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                if (ProcessEvent(wParam, lParam))
                {
                    return -1;
                }
            }

            return NativeMethods.CallNextHookEx(hooker.hHook, nCode, wParam, lParam).ToInt32();
        }

        public void SetHook()
        {
            hookProc = new NativeCallbacks.HookProc(HookCallback);
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
