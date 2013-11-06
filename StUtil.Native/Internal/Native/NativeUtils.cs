using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
namespace StUtil.Internal.Native
{
    public static class NativeUtils
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(NativeConsts.MAX_CAPTION_LENGTH);
            NativeMethods.GetClassName(hWnd, sb, NativeConsts.MAX_CAPTION_LENGTH);
            return sb.ToString();
        }

        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(NativeConsts.MAX_CAPTION_LENGTH);
            NativeMethods.GetWindowText(hWnd, sb, NativeConsts.MAX_CAPTION_LENGTH);
            return sb.ToString();
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return NativeMethods.SetWindowsHookEx(NativeConsts.WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void SetTopMost(Control control)
        {
            NativeMethods.SetWindowPos(control.Handle, NativeConsts.HWND_TOPMOST, 0, 0, 0, 0, 0x13);
        }

        public static NativeStructs.SCROLLINFO GetScrollInfo(Control ctrl, NativeEnums.ScrollBarDirection direction)
        {
            NativeStructs.SCROLLINFO info = new NativeStructs.SCROLLINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.fMask = (int)NativeEnums.ScrollInfoMask.SIF_ALL;
            NativeMethods.GetScrollInfo(ctrl.Handle, (int)direction, ref info);
            return info;
        }

        public static List<IntPtr> GetChildren(IntPtr hWnd)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowsProc childProc = new EnumWindowsProc(GetChildrenProc);
                Internal.Native.NativeMethods.EnumChildWindows(hWnd, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private static bool GetChildrenProc(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            return true;
        }

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return NativeMethods.GetWindowLongPtr64(hWnd, nIndex);
            else
                return NativeMethods.GetWindowLongPtr32(hWnd, nIndex);
        }

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(NativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong.ToInt32()));
        }
    }
}
