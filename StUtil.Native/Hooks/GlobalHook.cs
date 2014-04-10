using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Hooks
{
    public class GlobalHook : HookMethod
    {
        protected override IntPtr SetHookInternal(int hookId, StUtil.Internal.Native.NativeUtils.HookProc callback)
        {
            return StUtil.Internal.Native.NativeMethods.SetWindowsHookEx(
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
                    return StUtil.Internal.Native.NativeConsts.WH_KEYBOARD_LL;
                case WindowsHook.HookType.Mouse:
                    return StUtil.Internal.Native.NativeConsts.WH_MOUSE_LL;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
