using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Hooks
{
    public class KeyboardHook : WindowsHook
    {
        public HashSet<Keys> KeysDown { get; private set; }

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
                keyData |= ((NativeMethods.GetKeyState(NativeConsts.VK_SHIFT) & 0x80) == 0x80 ? Keys.Shift : Keys.None);
                keyData |= ((NativeMethods.GetKeyState(NativeConsts.VK_CONTROL) & 0x80) == 0x80 ? Keys.Control : Keys.None);
                keyData |= ((NativeMethods.GetKeyState(NativeConsts.VK_MENU) & 0x80) == 0x80 ? Keys.Menu : Keys.None);
                KeyEventArgs e = new KeyEventArgs(keyData);
                KeyDown(this, e);
                handled = handled || e.Handled;
            }
        }

        private void OnKeyPress(int vkCode, int scanCode, int flags, ref bool handled)
        {
            if (KeyPress != null)
            {
                bool isDownShift = ((NativeMethods.GetKeyState(NativeConsts.VK_SHIFT) & 0x80) == 0x80 ? true : false);
                bool isDownCapslock = (NativeMethods.GetKeyState(NativeConsts.VK_CAPITAL) != 0 ? true : false);
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

        protected override bool ProcessEvent(int wParam, IntPtr lParam)
        {
            bool handled = false;

            if (hooker.GetType() == typeof(GlobalHook))
            {
                NativeStructs.KBDLLHOOKSTRUCT khStruct = (NativeStructs.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.KBDLLHOOKSTRUCT));
                //raise KeyDown
                if (wParam == NativeConsts.WM_KEYDOWN || wParam == NativeConsts.WM_SYSKEYDOWN)
                {
                    OnKeyDown(khStruct.vkCode, ref handled);
                }
                // raise KeyPress
                if (wParam == NativeConsts.WM_KEYDOWN)
                {
                    OnKeyPress(khStruct.vkCode, khStruct.scanCode, khStruct.flags, ref handled);
                }
                // raise KeyUp
                if (wParam == NativeConsts.WM_KEYUP || wParam == NativeConsts.WM_SYSKEYUP)
                {
                    OnKeyUp(khStruct.vkCode, ref handled);
                }
            }
            else
            {
                const uint maskKeydown = 0x40000000;         // for bit 30
                const uint maskKeyup = 0x80000000;           // for bit 31
                const uint maskScanCode = 0xff0000;          // for bit 23-16

                int vkCode = wParam;

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

                int scanCode = checked( ( int )( flags & maskScanCode ) );

                if (!isKeyReleased)
                {
                    OnKeyDown(vkCode, ref handled);
                    OnKeyPress(vkCode, scanCode,(int)flags, ref handled);
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
