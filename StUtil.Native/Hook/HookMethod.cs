using System;

namespace StUtil.Native.Hook
{
    public abstract class HookMethod
    {
        public IntPtr hHook { get; set; }

        protected abstract IntPtr SetHookInternal(int hookId, StUtil.Native.Internal.NativeCallbacks.HookProc callback);

        internal virtual void SetHook(int hookId, StUtil.Native.Internal.NativeCallbacks.HookProc callback)
        {
            hHook = SetHookInternal(hookId, callback);
            if (hHook.ToInt32() == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        internal virtual void RemoveHook()
        {
            StUtil.Native.Internal.NativeMethods.UnhookWindowsHookEx(hHook);
        }

        internal abstract int GetHookCode(WindowsHook.HookType type);
    }
}