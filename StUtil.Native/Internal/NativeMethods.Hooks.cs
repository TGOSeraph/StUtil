using System;
using System.Runtime.InteropServices;

namespace StUtil.Native.Internal
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events.
        /// These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">A pointer to the hook procedure.
        /// If the dwThreadId parameter is zero or specifies the identifier of a thread created by a different process, the lpfn parameter must point to a hook procedure in a DLL.
        /// Otherwise, lpfn can point to a hook procedure in the code associated with the current process.</param>
        /// <param name="hMod">A handle to the DLL containing the hook procedure pointed to by the lpfn parameter.
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated with the current process.</param>
        /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.
        /// For desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread.</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL.
        /// To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, NativeCallbacks.HookProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">A handle to the hook to be removed.
        /// This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hhk">This parameter is ignored.</param>
        /// <param name="nCode">The hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to process the hook information.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of hook associated with the current hook chain.</param>
        /// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value. The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}