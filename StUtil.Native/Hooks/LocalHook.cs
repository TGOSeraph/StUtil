using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hooks
{
    public class LocalHook : HookMethod
    {
        protected override IntPtr SetHookInternal(int hookId, StUtil.Internal.Native.NativeUtils.HookProc callback)
        {
            return StUtil.Internal.Native.NativeMethods.SetWindowsHookEx(
                hookId,
                callback,
                IntPtr.Zero,
                (uint)StUtil.Internal.Native.NativeMethods.GetCurrentThreadId());
        }

        internal override int GetHookCode(WindowsHook.HookType type)
        {
            switch (type)
            {
                case WindowsHook.HookType.Keyboard:
                    return StUtil.Internal.Native.NativeConsts.WH_KEYBOARD;
                case WindowsHook.HookType.Mouse:
                    return StUtil.Internal.Native.NativeConsts.WH_MOUSE;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
