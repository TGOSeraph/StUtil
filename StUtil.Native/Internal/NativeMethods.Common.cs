using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native.Internal
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Retrieves the thread identifier of the calling thread.
        /// </summary>
        /// <returns>The return value is the thread identifier of the calling thread.</returns>
        [DllImport("kernel32")]
        public static extern int GetCurrentThreadId();

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        /// </summary>
        /// <returns>
        /// The return value is a handle to the foreground window. The foreground window can be NULL in certain circumstances, such as when a window is losing activation.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// <para>Retrieves the length, in characters, of the specified window's title bar text (if the window has a title bar). If the specified window is a control, the function retrieves the length of the text within the control. However, GetWindowTextLength cannot retrieve the length of the text of an edit control in another application.</para>
        /// </summary>
        /// <param name="hWnd">A handle to the window or control.</param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: int</para>
        /// <para>If the function succeeds, the return value is the length, in characters, of the text. Under certain conditions, this value may actually be greater than the length of the text. For more information, see the following Remarks section.</para>
        /// <para>If the window has no text, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the target window is owned by the current process, GetWindowTextLength causes a WM_GETTEXTLENGTH message to be sent to the specified window or control.</para>
        /// <para>Under certain conditions, the GetWindowTextLength function may return a value that is larger than the actual length of the text. This occurs with certain mixtures of ANSI and Unicode, and is due to the system allowing for the possible existence of double-byte character set (DBCS) characters within the text. The return value, however, will always be at least as large as the actual length of the text; you can thus always use it to guide buffer allocation. This behavior can occur when an application uses both ANSI functions and common dialogs, which use Unicode. It can also occur when an application uses the ANSI version of GetWindowTextLength with a window whose window procedure is Unicode, or the Unicode version of GetWindowTextLength with a window whose window procedure is ANSI. For more information on ANSI and ANSI functions, see Conventions for Function Prototypes.</para>
        /// <para>To obtain the exact length of the text, use the WM_GETTEXT, LB_GETTEXT, or CB_GETLBTEXT messages, or the GetWindowText function.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// <para>Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another application.</para>
        /// </summary>
        /// <param name="hWnd">A handle to the window or control containing the text. </param>
        /// <param name="lpString">The buffer that will receive the text. If the string is as long or longer than the buffer, the string is truncated and terminated with a null character. </param>
        /// <param name="nMaxCount">The maximum number of characters to copy to the buffer, including the null character. If the text exceeds this limit, it is truncated. </param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: int</para>
        /// <para>If the function succeeds, the return value is the length, in characters, of the copied string, not including the terminating null character. If the window has no title bar or text, if the title bar is empty, or if the window or control handle is invalid, the return value is zero. To get extended error information, call GetLastError.</para>
        /// <para>This function cannot retrieve the text of an edit control in another application.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the target window is owned by the current process, GetWindowText causes a WM_GETTEXT message to be sent to the specified window or control. If the target window is owned by another process and has a caption, GetWindowText retrieves the window caption text. If the window does not have a caption, the return value is a null string. This behavior is by design. It allows applications to call GetWindowText without becoming unresponsive if the process that owns the target window is not responding. However, if the target window is not responding and it belongs to the calling application, GetWindowText will cause the calling application to become unresponsive.</para>
        /// <para>To retrieve the text of a control in another process, send a WM_GETTEXT message directly instead of calling GetWindowText.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// Changes the text of the specified window's title bar (if it has one). If the specified window is a control, the text of the control is changed. However, SetWindowText cannot change the text of a control in another application.
        /// </summary>
        /// <param name="hWnd">A handle to the window or control whose text is to be changed.</param>
        /// <param name="lpString">The new title or control text.</param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: BOOL</para>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the target window is owned by the current process, SetWindowText causes a WM_SETTEXT message to be sent to the specified window or control. If the control is a list box control created with the WS_CAPTION style, however, SetWindowText sets the text for the control, not for the list box entries.</para>
        /// <para>To set the text of a control in another process, send the WM_SETTEXT message directly instead of calling SetWindowText.</para>
        /// <para>The SetWindowText function does not expand tab characters (ASCII code 0x09). Tab characters are displayed as vertical bar (|) characters.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        /// <summary>
        /// Retrieves the name of the class to which the specified window belongs.
        /// </summary>
        /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="lpClassName">The class name string.</param>
        /// <param name="nMaxCount">The length of the lpClassName buffer, in characters. The buffer must be large enough to include the terminating null character; otherwise, the class name string is truncated to nMaxCount-1 characters.</param>
        /// <returns>
        /// If the function succeeds, the return value is the number of characters copied to the buffer, not including the terminating null character.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// <para>Enumerates the child windows that belong to the specified parent window by passing the handle to each child window, in turn, to an application-defined callback function. EnumChildWindows continues until the last child window is enumerated or the callback function returns FALSE.</para>
        /// </summary>
        /// <param name="hWndParent">A handle to the parent window whose child windows are to be enumerated. If this parameter is NULL, this function is equivalent to EnumWindows.
        /// </param>
        /// <param name="lpEnumFunc">A pointer to an application-defined callback function. For more information, see EnumChildProc. </param>
        /// <param name="lParam">An application-defined value to be passed to the callback function. </param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: BOOL</para>
        /// <para>The return value is not used.</para>
        /// </returns>
        /// <remarks>
        /// <para>If a child window has created child windows of its own, EnumChildWindows enumerates those windows as well.</para>
        /// <para>A child window that is moved or repositioned in the Z order during the enumeration process will be properly enumerated. The function does not enumerate a child window that is destroyed before being enumerated or that is created during the enumeration process.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, NativeCallbacks.EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(NativeCallbacks.EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// <para>Closes an open object handle.</para>
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call
        /// etLastError.</para>
        /// <para>If the application is running under a debugger, the function will throw an exception if it receives either a handle value that is not valid or a pseudo-handle value. This can happen if you close a handle twice, or if you call
        /// loseHandle on a handle returned by the
        /// indFirstFile function instead of calling the FindClose function.</para>
        /// </returns>
        /// <remarks>
        /// <para>The documentation for the functions that create these objects indicates that CloseHandle should be used when you are finished with the object, and what happens to pending operations on the object after the handle is closed. In general, CloseHandle invalidates the specified object handle, decrements the object's handle count, and performs object retention checks. After the last handle to an object is closed, the object is removed from the system. For a summary of the creator functions for these objects, see Kernel Objects.</para>
        /// <para>Generally, an application should call CloseHandle once for each handle it opens. It is usually not necessary to call CloseHandle if a function that uses a handle fails with ERROR_INVALID_HANDLE, because this error usually indicates that the handle is already invalidated. However, some functions use ERROR_INVALID_HANDLE to indicate that the object itself is no longer valid. For example, a function that attempts to use a handle to a file on a network might fail with ERROR_INVALID_HANDLE if the network connection is severed, because the file object is no longer available. In this case, the application should close the handle.</para>
        /// <para>If a handle is transacted, all handles bound to a transaction should be closed before the transaction is committed. If a transacted handle was opened by calling CreateFileTransacted with the FILE_FLAG_DELETE_ON_CLOSE flag, the file is not deleted until the application closes the handle and calls CommitTransaction. For more information about transacted objects, see Working With Transactions.</para>
        /// <para>Closing a thread handle does not terminate the associated thread or remove the thread object. Closing a process handle does not terminate the associated process or remove the process object. To remove a thread object, you must terminate the thread, then close all handles to the thread. For more information, see Terminating a Thread. To remove a process object, you must terminate the process, then close all handles to the process. For more information, see Terminating a Process.</para>
        /// <para>Closing a handle to a file mapping can succeed even when there are file views that are still open. For more information, see Closing a File Mapping Object.</para>
        /// <para>Do not use the CloseHandle function to close a socket. Instead, use the closesocket function, which releases all resources associated with the socket including the handle to the socket object. For more information, see Socket Closure.</para>
        /// <para>Windows Phone 8: This API is supported.</para>
        /// <para>Windows Phone 8.1: This API is supported.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// <para>Waits until the specified object is in the signaled state or the time-out interval elapses.</para>
        /// </summary>
        /// <param name="hHandle">If this handle is closed while the wait is still pending, the function's behavior is undefined.</param>
        /// <param name="dwMilliseconds">Note  The dwMilliseconds value does not include time spent in low-power states. For example, the timeout will not keep counting down while the computer is asleep.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value indicates the event that caused the function to return. It can be one of the following values.</para>
        /// eturn code/valueDescription
        /// AIT_ABANDONED
        /// x00000080L
        /// he specified object is a mutex object that was not released by the thread that owned the mutex object before the owning thread terminated. Ownership of the mutex object is granted to the calling thread and the mutex state is set to nonsignaled.
        /// f the mutex was protecting persistent state information, you should check it for consistency.WAIT_OBJECT_0
        /// x00000000L
        /// he state of the specified object is signaled.WAIT_TIMEOUT
        /// x00000102L
        /// he time-out interval elapsed, and the object's state is nonsignaled.WAIT_FAILED
        /// DWORD)0xFFFFFFFF
        /// he function has failed. To get extended error information, call
        /// etLastError./// </returns>
        /// <remarks>
        /// <para>The
        /// aitForSingleObject function checks the current state of the specified object. If the object's state is nonsignaled, the calling thread enters the wait state until the object is signaled or the time-out interval elapses.</para>
        /// <para>The function modifies the state of some types of synchronization objects. Modification occurs only for the object whose signaled state caused the function to return. For example, the count of a semaphore object is decreased by one.</para>
        /// <para>The
        /// aitForSingleObject function can wait for the following objects:</para>
        /// hange notification
        /// onsole input
        /// vent
        /// emory resource notification
        /// utex
        /// rocess
        /// emaphore
        /// hread
        /// aitable timer
        /// <para>Use caution when calling the wait functions and code that directly or indirectly creates windows. If a thread creates any windows, it must process messages. Message broadcasts are sent to all windows in the system. A thread that uses a wait function with no time-out interval may cause the system to become deadlocked. Two examples of code that indirectly creates windows are DDE and the CoInitialize function. Therefore, if you have a thread that creates windows, use
        /// sgWaitForMultipleObjects or
        /// sgWaitForMultipleObjectsEx, rather than
        /// aitForSingleObject.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        /// <summary>
        /// <para>Retrieves the termination status of the specified thread.</para>
        /// </summary>
        /// <param name="hThread">The handle must have the THREAD_QUERY_INFORMATION or THREAD_QUERY_LIMITED_INFORMATION access right. For more information, see
        /// hread Security and Access Rights.</param>
        /// <param name="lpExitCode">A pointer to a variable to receive the thread termination status. For more information, see Remarks.</param>
        /// <returns>
        /// <para>If the function succeeds, the return value is nonzero.</para>
        /// <para>If the function fails, the return value is zero. To get extended error information, call
        /// etLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>This function returns immediately. If the specified thread has not terminated and the function succeeds, the status returned is STILL_ACTIVE. If the thread has terminated and the function succeeds, the status returned is one of the following values:</para>
        /// he exit value specified in the
        /// xitThread or
        /// erminateThread function.
        /// he return value from the thread function.
        /// he exit value of the thread's process.
        /// <para>Important  The GetExitCodeThread function returns a valid error code defined by the application only after the thread terminates. Therefore, an application should not use STILL_ACTIVE (259) as an error code. If a thread returns STILL_ACTIVE (259) as an error code, applications that test for this value could interpret it to mean that the thread is still running and continue to test for the completion of the thread after the thread has terminated, which could put the application into an infinite loop.</para>
        /// </remarks>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeThread(IntPtr hThread, out IntPtr lpExitCode);
        /// <summary>
        /// <para>Retrieves a handle to the top-level window whose class name and window name match the specified strings. This function does not search child windows. This function does not perform a case-sensitive search.</para>
        /// </summary>
        /// <param name="lpClassName">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpClassName; the high-order word must be zero. </param>
        /// <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match. </param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: HWND</para>
        /// <para>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the lpWindowName parameter is not NULL, FindWindow calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks for GetWindowText.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// <para>Retrieves a handle to a window whose class name and window name match the specified strings. The function searches child windows, beginning with the one following the specified child window. This function does not perform a case-sensitive search.</para>
        /// </summary>
        /// <param name="hwndParent">A handle to the parent window whose child windows are to be searched.</param>
        /// <param name="hwndChildAfter">A handle to a child window. The search begins with the next child window in the Z order. The child window must be a direct child window of hwndParent, not just a descendant window. </param>
        /// <param name="lpszClass">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be placed in the low-order word of lpszClass; the high-order word must be zero.</param>
        /// <param name="lpszWindow">The window name (the window's title). If this parameter is NULL, all window names match. </param>
        /// <returns>
        /// <para>Type:</para>
        /// <para>Type: HWND</para>
        /// <para>If the function succeeds, the return value is a handle to the window that has the specified class and window names.</para>
        /// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the lpszWindow parameter is not NULL, FindWindowEx calls the GetWindowText function to retrieve the window name for comparison. For a description of a potential problem that can arise, see the Remarks section of GetWindowText.</para>
        /// <para>An application can call this function in the following way.</para>
        /// <para>FindWindowEx( NULL, NULL, MAKEINTATOM(0x8000), NULL );</para>
        /// <para>Note that 0x8000 is the atom for a menu class. When an application calls this function, the function checks whether a context menu is being displayed that the application created.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

   
    }
}