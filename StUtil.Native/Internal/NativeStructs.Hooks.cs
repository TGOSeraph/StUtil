using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Internal
{
    public static partial class NativeStructs
    {
        /// <summary>
        /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public int vkCode;

            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// </summary>
            public int scanCode;

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int flags;

            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public int dwExtraInfo;
        }

        /// <summary>
        /// The MOUSEHOOKSTRUCT structure contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public class MOUSEHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;

            /// <summary>
            /// Handle to the window that will receive the mouse message corresponding to the mouse event.
            /// </summary>
            public int hwnd;

            /// <summary>
            /// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message.
            /// </summary>
            public int wHitTestCode;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class MSLLHOOKSTRUCT
        {
            /// <summary>
            /// Specifies a POINT structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public POINT pt;

            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta.
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward,
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
            /// One wheel click is defined as WHEEL_DELTA, which is 120.
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, mouseData is not used.
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public int mouseData;

            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse flags. Value Purpose
            ///LLMHF_INJECTED Test the event-injected flag.
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
            /// </summary>
            public int flags;

            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;

            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            /// <summary>
            /// Additional information about the message. The exact meaning depends on the message value.
            /// </summary>
            public IntPtr lParam;
            /// <summary>
            /// Additional information about the message. The exact meaning depends on the message value.
            /// </summary>
            public IntPtr wParam;
            /// <summary>
            /// The message.
            /// </summary>
            public int message;
            /// <summary>
            /// A handle to the window to receive the message.
            /// </summary>
            public IntPtr hwnd;
        }
    }
}
