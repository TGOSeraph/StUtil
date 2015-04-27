using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Internal
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// Determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState.
        /// </summary>
        /// <param name="virtualKeyCode">The virtual-key code.</param>
        /// <returns>
        /// If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState
        /// </returns>
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        /// <summary>
        /// Copies the status of the 256 virtual keys to the specified buffer.
        /// </summary>
        /// <param name="lpKeyState">The 256-byte array that receives the status data for each virtual key.</param>
        /// <returns>
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] lpKeyState);

        /// <summary>
        /// <para>Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, offâ€”alternating each time the key is pressed).</para>
        /// </summary>
        /// <param name="nVirtKey">A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9),
        /// nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code. </param>
        /// <returns>
        /// <para>Type: SHORT</para>
        /// <para>The return value specifies the status of the specified virtual key, as follows:</para>
        /// f the high-order bit is 1, the key is down; otherwise, it is up.
        /// f the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// </returns>
        /// <remarks>
        /// <para>The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information.</para>
        /// <para>An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated.</para>
        /// <para>To retrieve state information for all the virtual keys, use the GetKeyboardState function.</para>
        /// <para>An application can use the virtual key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the
        /// nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for
        /// nVirtKey to distinguish between the left and right instances of those keys:</para>
        /// <list>
        /// <listheader>VK_LSHIFT</listheader>
        /// <item>VK_RSHIFT</item>
        /// <listheader>VK_LCONTROL</listheader>
        /// <item>VK_RCONTROL</item>
        /// <listheader>VK_LMENU</listheader>
        /// <item>VK_RMENU</item>
        /// </list>
        /// <para>These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions.</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int nVirtKey);

        /// <summary>
        /// <para>Retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, offâ€”alternating each time the key is pressed).</para>
        /// </summary>
        /// <param name="nVirtKey">A virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9),
        /// nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code. </param>
        /// <returns>
        /// <para>Type: SHORT</para>
        /// <para>The return value specifies the status of the specified virtual key, as follows:</para>
        /// f the high-order bit is 1, the key is down; otherwise, it is up.
        /// f the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// </returns>
        /// <remarks>
        /// <para>The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information.</para>
        /// <para>An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated.</para>
        /// <para>To retrieve state information for all the virtual keys, use the GetKeyboardState function.</para>
        /// <para>An application can use the virtual key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the
        /// nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for
        /// nVirtKey to distinguish between the left and right instances of those keys:</para>
        /// <list>
        /// <listheader>VK_LSHIFT</listheader>
        /// <item>VK_RSHIFT</item>
        /// <listheader>VK_LCONTROL</listheader>
        /// <item>VK_RCONTROL</item>
        /// <listheader>VK_LMENU</listheader>
        /// <item>VK_RMENU</item>
        /// </list>
        /// <para>These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions.</para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(Keys nVirtKey);

        /// <summary>
        /// <para>Translates the specified virtual-key code and keyboard state to the corresponding character or characters. The function translates the code using the input language and physical keyboard layout identified by the keyboard layout handle.</para>
        /// </summary>
        /// <param name="uVirtKey">The virtual-key code to be translated. See Virtual-Key Codes.</param>
        /// <param name="uScanCode">The hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up (not pressed). </param>
        /// <param name="lpKeyState">A pointer to a 256-byte array that contains the current keyboard state. Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, the key is down (pressed). </param>
        /// <param name="lpChar">The buffer that receives the translated character or characters. </param>
        /// <param name="uFlags">This parameter must be 1 if a menu is active, or 0 otherwise. </param>
        /// <returns>
        /// <para>Type: int</para>
        /// <para>If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values.</para>
        /// eturn valueDescription
        ///
        /// he specified virtual key has no translation for the current state of the keyboard.1
        /// ne character was copied to the buffer.2
        /// wo characters were copied to the buffer. This usually happens when a dead-key character (accent or diacritic) stored in the keyboard layout cannot be composed with the specified virtual key to form a single character./// </returns>
        /// <remarks>
        /// <para>The parameters supplied to the ToAscii function might not be sufficient to translate the virtual-key code, because a previous dead key is stored in the keyboard layout.</para>
        /// <para>Typically, ToAscii performs the translation based on the virtual-key code. In some cases, however, bit 15 of the
        /// uScanCode parameter may be used to distinguish between a key press and a key release. The scan code is used for translating ALT+
        /// number key combinations.</para>
        /// <para>Although NUM LOCK is a toggle key that affects keyboard behavior, ToAscii ignores the toggle setting (the low bit) of
        /// lpKeyState (VK_NUMLOCK) because the
        /// uVirtKey parameter alone is sufficient to distinguish the cursor movement keys (VK_HOME, VK_INSERT, and so on) from the numeric keys (VK_DECIMAL, VK_NUMPAD0 - VK_NUMPAD9).</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int ToAscii(uint uVirtKey, uint uScanCode, byte[] lpKeyState, byte[] lpChar, uint uFlags);

        /// <summary>
        /// <para>The mouse_event function synthesizes mouse motion and button clicks.</para>
        /// </summary>
        /// <param name="dwFlags">Controls various aspects of mouse motion and button clicking. This parameter can be certain combinations of the following values.</param>
        /// <param name="dx">The mouse's absolute position along the x-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual x-coordinate; relative data is specified as the number of mickeys moved. A
        /// mickey is the amount that a mouse has to move for it to report that it has moved. </param>
        /// <param name="dy">The mouse's absolute position along the y-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE. Absolute data is specified as the mouse's actual y-coordinate; relative data is specified as the number of mickeys moved. </param>
        /// <param name="dwData">If
        /// dwFlags contains MOUSEEVENTF_WHEEL, then
        /// dwData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120. </param>
        /// <param name="dwExtraInfo">An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information. </param>
        /// <returns>
        /// <para>This function has no return value.</para>
        /// </returns>
        /// <remarks>
        /// <para>If the mouse has moved, indicated by MOUSEEVENTF_MOVE being set,
        /// dx and
        /// dy hold information about that motion. The information is specified as absolute or relative integer values.</para>
        /// <para>If MOUSEEVENTF_ABSOLUTE value is specified,
        /// dx and
        /// dy contain normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface, (65535,65535) maps onto the lower-right corner.</para>
        /// <para>If the MOUSEEVENTF_ABSOLUTE value is not specified,
        /// dx and
        /// dy specify relative motions from when the last mouse event was generated (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up).</para>
        /// <para>Relative mouse motion is subject to the settings for mouse speed and acceleration level. An end user sets these values using the Mouse application in Control Panel. An application obtains and sets these values with the SystemParametersInfo function.</para>
        /// <para>The system applies two tests to the specified relative mouse motion when applying acceleration. If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse acceleration level is not zero, the operating system doubles the distance. If the specified distance along either the x- or y-axis is greater than the second mouse threshold value, and the mouse acceleration level is equal to two, the operating system doubles the distance that resulted from applying the first threshold test. It is thus possible for the operating system to multiply relatively-specified mouse motion along the x- or y-axis by up to four times.</para>
        /// <para>Once acceleration has been applied, the system scales the resultant value by the desired mouse speed. Mouse speed can range from 1 (slowest) to 20 (fastest) and represents how much the pointer moves based on the distance the mouse moves. The default value is 10, which results in no additional modification to the mouse motion.</para>
        /// <para>The mouse_event function is used to synthesize mouse events by applications that need to do so. It is also used by applications that need to obtain more information from the mouse than its position and button state. For example, if a tablet manufacturer wants to pass pen-based information to its own applications, it can write a DLL that communicates directly to the tablet hardware, obtains the extra information, and saves it in a queue. The DLL then calls mouse_event with the standard button and x/y position data, along with, in the dwExtraInfo parameter, some pointer or index to the queued extra information. When the application needs the extra information, it calls the DLL with the pointer or index stored in
        /// dwExtraInfo, and the DLL returns the extra information.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        /// <summary>
        /// <para>Synthesizes a keystroke. The system can use such a synthesized keystroke to generate a WM_KEYUP or WM_KEYDOWN message. The keyboard driver's interrupt handler calls the keybd_event function.</para>
        /// </summary>
        /// <param name="bVk">A virtual-key code. The code must be a value in the range 1 to 254. For a complete list, see Virtual Key Codes. </param>
        /// <param name="bScan">A hardware scan code for the key.</param>
        /// <param name="dwFlags">Controls various aspects of function operation. This parameter can be one or more of the following values. </param>
        /// <param name="dwExtraInfo">An additional value associated with the key stroke. </param>
        /// <returns>
        /// <para>This function does not return a value.</para>
        /// </returns>
        /// <remarks>
        /// <para>An application can simulate a press of the PRINTSCRN key in order to obtain a screen snapshot and save it to the clipboard. To do this, call keybd_event with the
        /// bVk parameter set to VK_SNAPSHOT.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        /// <summary>
        /// <para>Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code.</para>
        /// </summary>
        /// <param name="uCode">The virtual key code or scan code for a key. How this value is interpreted depends on the value of the uMapType parameter. </param>
        /// <param name="uMapType">The translation to be performed. The value of this parameter depends on the value of the uCode parameter. </param>
        /// <returns>
        /// <para>Type: UINT</para>
        /// <para>The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType. If there is no translation, the return value is zero.</para>
        /// </returns>
        /// <remarks>
        /// <para>An application can use MapVirtualKey to translate scan codes to the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU, and vice versa. These translations do not distinguish between the left and right instances of the SHIFT, CTRL, or ALT keys.</para>
        /// <para>An application can get the scan code corresponding to the left or right instance of one of these keys by calling MapVirtualKey with uCode set to one of the following virtual-key code constants.</para>
        /// K_LSHIFT
        /// K_RSHIFT
        /// K_LCONTROL
        /// K_RCONTROL
        /// K_LMENU
        /// K_RMENU
        /// <para>These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions.</para>
        /// </remarks>
        [DllImport("User32.dll", SetLastError = true)]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, ref NativeStructs.INPUT pInputs, int cbSize);
    }
}