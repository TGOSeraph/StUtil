using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hooks
{
    public abstract class HookMethod
    {
        public IntPtr hHook { get; set; }

        protected abstract IntPtr SetHookInternal(int hookId, StUtil.Internal.Native.NativeUtils.HookProc callback);
        
        internal virtual void SetHook(int hookId, StUtil.Internal.Native.NativeUtils.HookProc callback)
        {
            hHook = SetHookInternal(hookId, callback);
            if (hHook.ToInt32() == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        internal virtual void RemoveHook()
        {
            StUtil.Internal.Native.NativeMethods.UnhookWindowsHookEx(hHook);
        }

        internal abstract int GetHookCode(WindowsHook.HookType type);
    }
}
