using StUtil.Native.Internal;
using System;

namespace StUtil.Native.Hook
{
    public class LocalHook : HookMethod
    {
        protected override IntPtr SetHookInternal(int hookId, NativeCallbacks.HookProc callback)
        {
            return NativeMethods.SetWindowsHookEx(
                hookId,
                callback,
                IntPtr.Zero,
                (uint)NativeMethods.GetCurrentThreadId());
        }

        internal override int GetHookCode(WindowsHook.HookType type)
        {
            switch (type)
            {
                case WindowsHook.HookType.Keyboard:
                    return (int)NativeEnums.HookType.WH_KEYBOARD;

                case WindowsHook.HookType.Mouse:
                    return (int)NativeEnums.HookType.WH_MOUSE;

                case WindowsHook.HookType.Message:
                    return (int)NativeEnums.HookType.WH_CALLWNDPROC;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}