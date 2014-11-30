using StUtil.Native.Hook;
using StUtil.Native.Input.Hook;
using StUtil.Native.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public static class Keyboard
    {
        /// <summary>
        /// Occurs when the user presses a key down
        /// </summary>
        public static event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when the user presses and releases
        /// </summary>
        public static event KeyPressEventHandler KeyPress;

        /// <summary>
        /// Occurs when the user releases a key
        /// </summary>
        public static event KeyEventHandler KeyUp;

        private static bool enableRasingEvents = false;
        private static KeyboardHook hook = new KeyboardHook(new GlobalHook());

        /// <summary>
        /// Gets or sets a value indicating whether events are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if events should be raised; otherwise, <c>false</c>.
        /// </value>
        public static bool EnableRasingEvents
        {
            get
            {
                return enableRasingEvents;
            }
            set
            {
                if (enableRasingEvents != value)
                {
                    if (value)
                    {
                        hook.SetHook();
                    }
                    else
                    {
                        hook.RemoveHook();
                    }
                }
            }
        }

        static Keyboard()
        {
            hook.KeyDown += hook_KeyDown;
            hook.KeyPress += hook_KeyPress;
            hook.KeyUp += hook_KeyUp;
        }

        private static void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyDown != null) KeyDown(sender, e);
        }

        private static void hook_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (KeyPress != null) KeyPress(sender, e);
        }

        private static void hook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyUp != null) KeyUp(sender, e);
        }

        /// <summary>
        /// Presses the specified button down.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Down(Keys button)
        {
            NativeMethods.keybd_event((byte)button, 0, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Presses the specified button down.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="button">The button.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Down(IntPtr hWnd, Keys button)
        {
            IntPtr lParam = new IntPtr(0x00000001 | (NativeMethods.MapVirtualKey((uint)button, 0) << 16));
            NativeUtilities.DispatchMessage(hWnd, (int)NativeEnums.WM.KEYDOWN, new IntPtr((int)button), lParam, true);
        }

        /// <summary>
        /// Releases the specified button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Up(Keys button)
        {
            const uint KEYEVENTF_KEYUP = 0x02;
            NativeMethods.keybd_event((byte)button, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        /// <summary>
        /// Releases the specified button.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="button">The button.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Up(IntPtr hWnd, Keys button)
        {
            IntPtr lParam = new IntPtr(0x00000001 | (StUtil.Native.Internal.NativeMethods.MapVirtualKey((uint)button, 0) << 16));
            NativeUtilities.DispatchMessage(hWnd, (int)NativeEnums.WM.KEYUP, new IntPtr((int)button), lParam, true);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sleep">The sleep.</param>
        public static void Press(Keys key, int sleep)
        {
            Down(key);
            Thread.Sleep(sleep);
            Up(key);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="key">The key.</param>
        /// <param name="sleep">The sleep.</param>
        public static void Press(IntPtr hWnd, Keys key, int sleep)
        {
            Down(hWnd, key);
            Thread.Sleep(sleep);
            Up(hWnd, key);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Press(Keys key)
        {
            Press(key, 0);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="key">The key.</param>
        public static void Press(IntPtr hWnd, Keys key)
        {
            Press(hWnd, key, 0);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="sleep">The sleep.</param>
        public async static Task PressAsync(Keys key, int sleep)
        {
            Down(key);
            Task t = Task.Delay(sleep);
            await t.ContinueWith(task => Up(key));
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public async static Task PressAsync(Keys key)
        {
            await PressAsync(key, 0);
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="key">The key.</param>
        /// <param name="sleep">The sleep.</param>
        public async static Task PressAsync(IntPtr hWnd, Keys key, int sleep)
        {
            Down(hWnd, key);
            Task t = Task.Delay(sleep);
            await t.ContinueWith(task => Up(hWnd, key));
        }

        /// <summary>
        /// Presses the specified key.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="key">The key.</param>
        public async static Task PressAsync(IntPtr hWnd, Keys key)
        {
            await PressAsync(hWnd, key, 0);
        }
    }
}