using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native
{
    /// <summary>
    /// Native utilities
    /// </summary>
    /// <remarks>
    /// 2013-10-18  - Initial version
    /// </remarks>
   public static  class Utilities
    {
       /// <summary>
       /// Get the text of a window
       /// </summary>
       /// <param name="hWnd">The window to get the caption of</param>
       /// <returns>The caption of the specified window</returns>
       public static string GetWindowText(IntPtr hWnd)
       {
           return Internal.Native.NativeUtils.GetWindowText(hWnd);
       }

       /// <summary>
       /// Returns the process that owns the specified window handle
       /// </summary>
       /// <param name="hWnd">The window handle to retrieve the process of</param>
       /// <returns>The process that owns the specified window handle</returns>
       public static Process GetProcessFromHWND(IntPtr hWnd)
       {
           uint pid;
           Internal.Native.NativeMethods.GetWindowThreadProcessId(hWnd, out pid);
           return Process.GetProcessById((int)pid);
       }

       /// <summary>
       /// The function determines whether the current operating system is a 
       /// 64-bit operating system.
       /// </summary>
       /// <returns>
       /// The function returns true if the operating system is 64-bit; 
       /// otherwise, it returns false.
       /// </returns>
       public static bool Is64BitOperatingSystem()
       {
           if (IntPtr.Size == 8)  // 64-bit programs run only on Win64
           {
               return true;
           }
           else  // 32-bit programs run on both 32-bit and 64-bit Windows
           {
               // Detect whether the current process is a 32-bit process 
               // running on a 64-bit system.
               bool flag;
               return ((DoesWin32MethodExist("kernel32.dll", "IsWow64Process") &&
                   NativeMethods.IsWow64Process(NativeMethods.GetCurrentProcess(), out flag)) && flag);
           }
       }

       /// <summary>
       /// The function determins whether a method exists in the export 
       /// table of a certain module.
       /// </summary>
       /// <param name="moduleName">The name of the module</param>
       /// <param name="methodName">The name of the method</param>
       /// <returns>
       /// The function returns true if the method specified by methodName 
       /// exists in the export table of the module specified by moduleName.
       /// </returns>
       static bool DoesWin32MethodExist(string moduleName, string methodName)
       {
           IntPtr moduleHandle = NativeMethods.GetModuleHandle(moduleName);
           if (moduleHandle == IntPtr.Zero)
           {
               return false;
           }
           return (NativeMethods.GetProcAddress(moduleHandle, methodName) != IntPtr.Zero);
       }

       /// <summary>
       /// Loads the specified library into the calling processes memory
       /// </summary>
       /// <param name="s_File">The library to load</param>
       /// <returns>The handle of the module that was loaded</returns>
       public static IntPtr LoadLibrary(string s_File)
       {
           IntPtr h_Module = NativeMethods.LoadLibrary(s_File);
           if (h_Module != IntPtr.Zero)
               return h_Module;

           int s32_Error = Marshal.GetLastWin32Error();
           throw new Win32Exception(s32_Error);
       }

       public static bool EnableSeDebugPrivilege()
       {
           IntPtr hToken;
           NativeStructs.LUID luidSEDebugNameValue;
           NativeStructs.TOKEN_PRIVILEGES tkpPrivileges;

           if (!NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), NativeConsts.TOKEN_ADJUST_PRIVILEGES | NativeConsts.TOKEN_QUERY, out hToken))
           {
               return false;
           }

           if (!NativeMethods.LookupPrivilegeValue(null, NativeConsts.SE_DEBUG_NAME, out luidSEDebugNameValue))
           {
               NativeMethods.CloseHandle(hToken);
               return false;
           }

           tkpPrivileges.PrivilegeCount = 1;
           tkpPrivileges.Luid = luidSEDebugNameValue;
           tkpPrivileges.Attributes = NativeConsts.SE_PRIVILEGE_ENABLED;

           if (!NativeMethods.AdjustTokenPrivileges(hToken, false, ref tkpPrivileges, 0, IntPtr.Zero, IntPtr.Zero))
           {
               return false;
           }
           NativeMethods.CloseHandle(hToken);
           return true;
       }
    }
}
