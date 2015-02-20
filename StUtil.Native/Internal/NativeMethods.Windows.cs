using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace StUtil.Native.Internal
{
    public static partial class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, UInt32 wMsg, UInt32 wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, Int32 lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, NativeEnums.WM wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, NativeEnums.WM wMsg, bool wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, NativeEnums.WM wMsg, bool wParam, int lParam);

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool PostMessage(IntPtr hWnd, NativeEnums.WM wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, [MarshalAs(UnmanagedType.AsAny)]object wParam, IntPtr lParam);

        /// <summary>
        /// Sends the specified message to a window or windows.
        /// The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">The window handle to send the message to.</param>
        /// <param name="wMsg">The id of the message to send.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, [MarshalAs(UnmanagedType.AsAny)]object wParam, [MarshalAs(UnmanagedType.AsAny)]object lParam);

        [DllImport("user32")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newProc);

        [DllImport("user32")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, NativeCallbacks.MessageProc newProc);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// <para>Passes message information to the specified window procedure.</para>
        /// </summary>
        /// <param name="lpPrevWndFunc">The previous window procedure. If this value is obtained by calling the GetWindowLong function with the nIndex parameter set to GWL_WNDPROC or DWL_DLGPROC, it is actually either the address of a window or dialog box procedure, or a special internal value meaningful only to CallWindowProc. </param>
        /// <param name="hWnd">A handle to the window procedure to receive the message. </param>
        /// <param name="Msg">The message.</param>
        /// <param name="wParam">Additional message-specific information. The contents of this parameter depend on the value of the Msg parameter. </param>
        /// <param name="lParam">Additional message-specific information. The contents of this parameter depend on the value of the Msg parameter. </param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: LRESULT</para>
        /// <para>The return value specifies the result of the message processing and depends on the message sent.</para>
        /// </returns>
        /// <remarks>
        /// <para>Use the CallWindowProc function for window subclassing. Usually, all windows with the same class share one window procedure. A subclass is a window or set of windows with the same class whose messages are intercepted and processed by another window procedure (or procedures) before being passed to the window procedure of the class.</para>
        /// <para>The SetWindowLong function creates the subclass by changing the window procedure associated with a particular window, causing the system to call the new window procedure instead of the previous one. An application must pass any messages not processed by the new window procedure to the previous window procedure by calling CallWindowProc. This allows the application to create a chain of window procedures.</para>
        /// <para>For further information about functions declared with empty argument lists, refer to
        /// The C++ Programming Language, Second Edition, by Bjarne Stroustrup.</para>
        /// <para>The CallWindowProc function handles Unicode-to-ANSI conversion. You cannot take advantage of this conversion if you call the window procedure directly.</para>
        /// </remarks>
       
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
      
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
       
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref NativeStructs.RECT rect);
       
        [DllImport("user32.dll")]
        public static extern IntPtr GetClientRect(IntPtr hWnd, ref NativeStructs.RECT rect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, NativeEnums.SWP wFlags);
    }
}