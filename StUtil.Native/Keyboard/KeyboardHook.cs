using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using StUtil.Internal.Native;
namespace StUtil.Native.Keyboard
{
    /// <summary>
    /// Listens keyboard globally.
    /// 
    /// <remarks>Uses WH_KEYBOARD_LL.</remarks>
    /// </summary>
    public class KeyboardHook : IDisposable
    {
        public List<Keys> KeysDown { get; private set; }
        public bool Asynchronous { get; set; }

        /// <summary>
        /// Fired when any of the keys is pressed down.
        /// </summary>
        public event EventHandler<RawKeyEventArgs> KeyDown;
        /// <summary>
        /// Hook ID
        /// </summary>
        private IntPtr hookId = IntPtr.Zero;

        /// <summary>
        /// Asynchronous callback hook.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        private delegate bool KeyboardCallbackAsync(NativeEnums.KeyEvent keyEvent, int vkCode);

        /// <summary>
        /// Fired when any of the keys is released.
        /// </summary>
        public event EventHandler<RawKeyEventArgs> KeyUp;
        /// <summary>
        /// Event to be invoked asynchronously (BeginInvoke) each time key is pressed.
        /// </summary>
        private KeyboardCallbackAsync hookedKeyboardCallbackAsync;

        /// <summary>
        /// Contains the hooked callback in runtime.
        /// </summary>
        private NativeUtils.LowLevelKeyboardProc hookedLowLevelKeyboardProc;

        /// <summary>
        /// Creates global keyboard listener.
        /// </summary>
        public KeyboardHook()
        {
            Asynchronous = true;

            KeysDown = new List<Keys>();

            // We have to store the HookCallback, so that it is not garbage collected runtime
            hookedLowLevelKeyboardProc = (NativeUtils.LowLevelKeyboardProc)LowLevelKeyboardProc;

            // Set the hook
            hookId = NativeUtils.SetHook(hookedLowLevelKeyboardProc);

            // Assign the asynchronous callback event
            hookedKeyboardCallbackAsync = new KeyboardCallbackAsync(KeyboardListener_KeyboardCallbackAsync);
        }

        /// <summary>
        /// Destroys global keyboard listener.
        /// </summary>
        ~KeyboardHook()
        {
            Dispose();
        }

        /// <summary>
        /// Actual callback hook.
        /// 
        /// <remarks>Calls asynchronously the asyncCallback.</remarks>
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr LowLevelKeyboardProc(int nCode, UIntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam.ToUInt32() == (int)NativeEnums.KeyEvent.WM_KEYDOWN ||
                    wParam.ToUInt32() == (int)NativeEnums.KeyEvent.WM_KEYUP ||
                    wParam.ToUInt32() == (int)NativeEnums.KeyEvent.WM_SYSKEYDOWN ||
                    wParam.ToUInt32() == (int)NativeEnums.KeyEvent.WM_SYSKEYUP)
                {
                    if (Asynchronous)
                    {
                        hookedKeyboardCallbackAsync.BeginInvoke((NativeEnums.KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam), null, null);
                    }
                    else
                    {
                        if (KeyboardListener_KeyboardCallbackAsync((NativeEnums.KeyEvent)wParam.ToUInt32(), Marshal.ReadInt32(lParam)))
                        {
                            return (System.IntPtr)1;
                        }
                    }
                }
            }
            return NativeMethods.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// HookCallbackAsync procedure that calls accordingly the KeyDown or KeyUp events.
        /// </summary>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        bool KeyboardListener_KeyboardCallbackAsync(NativeEnums.KeyEvent keyEvent, int vkCode)
        {
            RawKeyEventArgs evt = null;
            Keys k = (Keys)vkCode;
            switch (keyEvent)
            {
                // KeyDown events
                case NativeEnums.KeyEvent.WM_KEYDOWN:
                    if (KeyDown != null)
                    {
                        evt = new RawKeyEventArgs(vkCode, false);
                        KeyDown(this, evt);
                    }
                    if (!KeysDown.Contains(k))
                    {
                        KeysDown.Add(k);
                    }
                    break;
                case NativeEnums.KeyEvent.WM_SYSKEYDOWN:
                    if (KeyDown != null)
                    {
                        evt = new RawKeyEventArgs(vkCode, true);
                        KeyDown(this, evt);
                    }
                    if (!KeysDown.Contains(k))
                    {
                        KeysDown.Add(k);
                    }
                    break;
                // KeyUp events
                case NativeEnums.KeyEvent.WM_KEYUP:
                    if (KeyUp != null)
                    {
                        evt = new RawKeyEventArgs(vkCode, false);
                        KeyUp(this, evt);
                    }
                    KeysDown.Remove(k);
                    break;
                case NativeEnums.KeyEvent.WM_SYSKEYUP:
                    if (KeyUp != null)
                    {
                        evt = new RawKeyEventArgs(vkCode, true);
                        KeyUp(this, evt);
                    }
                    KeysDown.Remove(k);
                    break;
                default:
                    break;
            }
            return evt.Handled;
        }

        /// <summary>
        /// Disposes the hook.
        /// <remarks>This call is required as it calls the UnhookWindowsHookEx.</remarks>
        /// </summary>
        public void Dispose()
        {
            NativeMethods.UnhookWindowsHookEx(hookId);
        }
    }
}