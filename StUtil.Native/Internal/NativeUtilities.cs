using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace StUtil.Native.Internal
{
    public static class NativeUtilities
    {
        /// <summary>
        /// Dispatches the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="post">if set to <c>true</c> then the message will be posted to the targets message queue.</param>
        public static void DispatchMessage(IntPtr handle, int message, IntPtr wParam, IntPtr lParam, bool post = false)
        {
            if (post)
            {
                StUtil.Native.Internal.NativeMethods.PostMessage(handle, message, wParam, lParam);
            }
            else
            {
                StUtil.Native.Internal.NativeMethods.SendMessage(handle, message, wParam, lParam);
            }
        }

        /// <summary>
        /// Makes an l parameter.
        /// </summary>
        /// <param name="LoWord">The lo word.</param>
        /// <param name="HiWord">The hi word.</param>
        /// <returns>The lParam of the two words</returns>
        public static int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }

        /// <summary>
        /// Gets the window text.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <returns>The caption of the window</returns>
        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(NativeConsts.MAX_CAPTION_LENGTH);
            NativeMethods.GetWindowText(hWnd, sb, NativeConsts.MAX_CAPTION_LENGTH);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the name of the windows class.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <returns>The class of the window</returns>
        public static string GetClassName(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(NativeConsts.MAX_CAPTION_LENGTH);
            NativeMethods.GetClassName(hWnd, sb, NativeConsts.MAX_CAPTION_LENGTH);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            NativeMethods.EnumChildWindows(parent, delegate(IntPtr hWnd, IntPtr lParam)
            {
                result.Add(hWnd);
                return true;
            }, IntPtr.Zero);

            return result;
        }

        public static IntPtr OpenProcess(Process process, NativeEnums.ProcessAccess flags)
        {
            return OpenProcess(process.Id, flags);
        }

        public static IntPtr OpenProcess(int processid, NativeEnums.ProcessAccess flags)
        {
            IntPtr hProcess = NativeMethods.OpenProcess((uint)flags, false, (uint)processid);

            if (hProcess == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return hProcess;
        }
    }
}