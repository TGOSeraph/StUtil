using System;
using System.Diagnostics;

namespace StUtil.Native.Hook
{
    public class GlobalHook : HookMethod
    {
        protected override IntPtr SetHookInternal(int hookId, StUtil.Native.Internal.NativeCallbacks.HookProc callback)
        {
            return StUtil.Native.Internal.NativeMethods.SetWindowsHookEx(
                hookId,
                callback,
                Process.GetCurrentProcess().MainModule.BaseAddress,
                0);
        }

        internal override int GetHookCode(WindowsHook.HookType type)
        {
            switch (type)
            {
                case WindowsHook.HookType.Keyboard:
                    return (int)StUtil.Native.Internal.NativeEnums.HookType.WH_KEYBOARD_LL;

                case WindowsHook.HookType.Mouse:
                    return (int)StUtil.Native.Internal.NativeEnums.HookType.WH_MOUSE_LL;

                case WindowsHook.HookType.Message:
                    return (int)StUtil.Native.Internal.NativeEnums.HookType.WH_CALLWNDPROC;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}