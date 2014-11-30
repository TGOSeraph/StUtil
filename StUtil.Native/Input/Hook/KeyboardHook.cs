using StUtil.Native.Hook;
using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input.Hook
{
    public class KeyboardHook : WindowsHook
    {
        public HashSet<Keys> KeysDown { get; private set; }

        /// <summary>
        /// Occurs when the user presses a key down
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when the user presses and releases
        /// </summary>
        public event KeyPressEventHandler KeyPress;
        /// <summary>
        /// Occurs when the user releases a key
        /// </summary>
        public event KeyEventHandler KeyUp;

        public KeyboardHook(HookMethod hooker)
            : base(hooker, HookType.Keyboard)
        {
            KeysDown = new HashSet<Keys>();
        }

        private void OnKeyDown(int vkCode, ref bool handled)
        {
            Keys keyData = (Keys)vkCode;
            if (!KeysDown.Contains(keyData))
            {
                KeysDown.Add(keyData);
            }
            if (KeyDown != null)
            {
                keyData |= ((NativeMethods.GetKeyState(Keys.Shift) & 0x80) == 0x80 ? Keys.Shift : Keys.None);
                keyData |= ((NativeMethods.GetKeyState(Keys.Control) & 0x80) == 0x80 ? Keys.Control : Keys.None);
                keyData |= ((NativeMethods.GetKeyState(Keys.Menu) & 0x80) == 0x80 ? Keys.Menu : Keys.None);
                KeyEventArgs e = new KeyEventArgs(keyData);
                KeyDown(this, e);
                handled = handled || e.Handled;
            }
        }

        private void OnKeyPress(uint vkCode, uint scanCode, uint flags, ref bool handled)
        {
            if (KeyPress != null)
            {
                bool isDownShift = ((NativeMethods.GetKeyState(Keys.Shift) & 0x80) == 0x80 ? true : false);
                bool isDownCapslock = (NativeMethods.GetKeyState(Keys.CapsLock) != 0 ? true : false);
                byte[] keyState = new byte[256];
                NativeMethods.GetKeyboardState(keyState);
                byte[] inBuffer = new byte[2];
                if (NativeMethods.ToAscii(vkCode, scanCode, keyState, inBuffer, flags) == 1)
                {
                    char key = (char)inBuffer[0];
                    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                    KeyPressEventArgs e = new KeyPressEventArgs(key);
                    KeyPress(this, e);
                    handled = handled || e.Handled;
                }
            }
        }

        private void OnKeyUp(int vkCode, ref bool handled)
        {
            Keys keyData = (Keys)vkCode;

            if (KeysDown.Contains(keyData))
            {
                KeysDown.Remove(keyData);
            }

            if (KeyUp != null)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                KeyUp(this, e);
                handled = handled || e.Handled;
            }
        }

        protected override bool ProcessEvent(IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;
            NativeEnums.WM message = (NativeEnums.WM)wParam;

            if (hooker.GetType() == typeof(GlobalHook))
            {
                NativeStructs.KBDLLHOOKSTRUCT khStruct = (NativeStructs.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.KBDLLHOOKSTRUCT));
                //raise KeyDown
                if (message == NativeEnums.WM.KEYDOWN || message == NativeEnums.WM.SYSKEYDOWN)
                {
                    OnKeyDown(khStruct.vkCode, ref handled);
                }
                // raise KeyPress
                if (message == NativeEnums.WM.KEYDOWN)
                {
                    OnKeyPress((uint)khStruct.vkCode, (uint)khStruct.scanCode, (uint)khStruct.flags, ref handled);
                }
                // raise KeyUp
                if (message == NativeEnums.WM.KEYUP || message == NativeEnums.WM.SYSKEYUP)
                {
                    OnKeyUp(khStruct.vkCode, ref handled);
                }
            }
            else
            {
                const uint maskKeydown = 0x40000000;         // for bit 30
                const uint maskKeyup = 0x80000000;           // for bit 31
                const uint maskScanCode = 0xff0000;          // for bit 23-16

                int vkCode = wParam.ToInt32();

                uint flags = 0u;
                if (Utilities.Is64BitApplication())
                {
                    flags = Convert.ToUInt32(lParam.ToInt64());
                }
                else
                {
                    flags = (uint)lParam.ToInt64();
                }

                bool wasKeyDown = (flags & maskKeydown) > 0;
                //bit 31 Specifies the transition state. The value is 0 if the key is being pressed and 1 if it is being released.
                bool isKeyReleased = (flags & maskKeyup) > 0;

                int scanCode = checked((int)(flags & maskScanCode));

                if (!isKeyReleased)
                {
                    OnKeyDown(vkCode, ref handled);
                    OnKeyPress((uint)vkCode, (uint)scanCode, flags, ref handled);
                }
                if (isKeyReleased)
                {
                    OnKeyUp(vkCode, ref handled);
                }
            }
            return handled;
        }
    }
}
